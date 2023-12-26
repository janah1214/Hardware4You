namespace Hardware4You.Models
{
    public class BuyingHistory
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Street { get; set; }

        public string ZIPCode { get; set; }

        public string City { get; set; }

        public string ProductId { get; set; }

        public DateTime PickupDate { get; set; }

        public DateTime TimeStamp { get; set; }

        public string State { get; set; }

        public string User { get; set; }
    }
}
