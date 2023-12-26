using Hardware4You.Models;
using Microsoft.EntityFrameworkCore;

namespace Hardware4You.Data
{
    public class BuyingHistoryService : IBuyingHistoryService
    {
        private BuyingHistoryContext _context;

        public BuyingHistoryService(BuyingHistoryContext context)
        {
            _context = context;
        }

        public IEnumerable<BuyingHistory> GetListBuyingHistory()
        {
            try
            {
                return _context.ListBuyingHistory.ToList();
            }
            catch
            {
                throw;
            }
        }

        public void DeleteBuyingHistory(long id)
        {
            try
            {
                BuyingHistory buyingHistory = _context.ListBuyingHistory.Find(id);
                _context.ListBuyingHistory.Remove(buyingHistory);
                _context.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public void InsertBuyingHistory(BuyingHistory buyingHistory)
        {
            try
            {
                buyingHistory.Id = _context.ListBuyingHistory.Count() + 1;

                _context.ListBuyingHistory.Add(buyingHistory);
                _context.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public BuyingHistory SingleBuyingHistory(long id)
        {
            throw new NotImplementedException();
        }

        public void UpdateBuyingHistory(long id, BuyingHistory buyingHistory)
        {
            try
            {
                var local = _context.Set<BuyingHistory>().Local.FirstOrDefault(entry => entry.Id.Equals(buyingHistory.Id));

                if (local != null)
                {
                    _context.Entry(local).State = EntityState.Detached;
                }
                _context.Entry(buyingHistory).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
    }
}
