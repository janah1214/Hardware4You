using Hardware4You.Models;

namespace Hardware4You.Data
{
    interface IBuyingHistoryService
    {
        IEnumerable<BuyingHistory> GetListBuyingHistory();

        void InsertBuyingHistory(BuyingHistory buyingHistory);

        void UpdateBuyingHistory(long id, BuyingHistory buyingHistory);

        BuyingHistory SingleBuyingHistory(long id);

        void DeleteBuyingHistory(long id);
    }
}