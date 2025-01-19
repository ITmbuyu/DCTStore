using System.Collections.Generic;
using DCTStore.Data.Migrations;
using DCTStore.Repository;
using Newtonsoft.Json;

namespace DCTStore.Models
{
    public class Cart
    {
        private readonly CartItemRepository _cartItemRepository;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public Cart(CartItemRepository cartItemRepository, IHttpContextAccessor httpContextAccessor)
        {
            _cartItemRepository = cartItemRepository;
			_httpContextAccessor = httpContextAccessor;
        }

		private List<CartItem> Items
		{
			get
			{
				var session = _httpContextAccessor.HttpContext.Session;
				var cartItems = session.GetString("Cart");

				if (cartItems != null)
				{
					return JsonConvert.DeserializeObject<List<CartItem>>(cartItems);
				}

				return new List<CartItem>();
			}
			set
			{
				var session = _httpContextAccessor.HttpContext.Session;
				session.SetString("Cart", JsonConvert.SerializeObject(value));
			}
		}

		public void AddItem(int id, int itemId, string? ItemCategory = null, string title = "", string mediaLink = "", string? videoMediaLink = null, string imagemedia = "")
		{
			// Check if the item already exists in the cart
			if (Items.Any(item => item.Id == id))
			{
				return; // Item already in the cart, do not add it again
			}

			// If ItemCategory is provided, use it; otherwise, determine based on itemId
			string itemCategory = ItemCategory ?? "";
			switch (itemId)
			{
				case 1:
					itemCategory = "Lyric";
					break;
				case 2:
					itemCategory = "Sermon";
					break;
				case 3:
					itemCategory = "Music";
					break;
				case 4:
					itemCategory = "SermonType";
					// Fetch all sermon links linked to the sermon type
					var sermonLinks = _cartItemRepository.GetSermonLinksForType(id);

					// Add each sermon link to the cart
					Items.AddRange(sermonLinks.Select(sermonLink => new CartItem
					{
						Id = sermonLink.Id,
						ItemId = sermonLink.Id,
						ItemCategory = itemCategory,
						Title = sermonLink.Title,
						MediaLink = sermonLink.MediaLink,
						VideoMediaLink = sermonLink.VideoMediaLink,
						imagemedia = sermonLink.MediaLink
					}));
					break;
				default:
					break;
			}

			// Add the current item to the cart if the category is valid
			var newItem = new CartItem
			{
				Id = id,
				ItemId = itemId,
				ItemCategory = itemCategory,  // Ensure the correct itemCategory is assigned here
				Title = title,
				MediaLink = mediaLink,
				VideoMediaLink = videoMediaLink,
				imagemedia = imagemedia
			};

			var currentItems = Items;
			currentItems.Add(newItem);
			Items = currentItems;  // Explicitly set the Items property
		}



		public void RemoveItem(int cartItemId)
        {
            var itemToRemove = Items.FirstOrDefault(item => item.Id == cartItemId);

            if (itemToRemove != null)
            {
                Items.Remove(itemToRemove);
            }
        }

        // Add a method to get all items in the cart
        public List<CartItem> GetItems()
        {
            return Items;
        }

        public void Clear()
        {
            Items.Clear();
        }

		//ADD A METHOD TO GET TOTAL ITEMS IN CART
		public int TotalItems()
		{
			return Items.Count;
		}
    }
}
