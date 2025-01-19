using DCTStore.Data;
using DCTStore.Models;

namespace DCTStore.Repository
{
    public class CartItemRepository
    {
        private readonly ApplicationDbContext _context;

        public CartItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<CartItem> GetSermonLinksForType(int sermonTypeId)
        {
            // Database query logic to get sermon links based on sermon type
            return _context.Sermons
                           .Where(s => s.SermonTypeId == sermonTypeId)
                           .Select(s => new CartItem
                           {
                               Id = s.SermonId,
                               Title = s.Title,
                               MediaLink = s.SermonMediaLink,
                               VideoMediaLink = s.SermonVideoLink
                           })
                           .ToList();
        }

    }
}
