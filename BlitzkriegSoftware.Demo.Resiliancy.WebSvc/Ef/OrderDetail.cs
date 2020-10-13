namespace BlitzkriegSoftware.Demo.Resiliancy.WebSvc.Ef
{
    /// <summary>
    /// Table: Order Detail
    /// </summary>
    public partial class OrderDetail
    {
        /// <summary>
        /// PK
        /// </summary>
        public long OrderDetailId { get; set; }
        /// <summary>
        /// FK
        /// </summary>
        public long OrderId { get; set; }
        /// <summary>
        /// FK
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        /// (sic)
        /// </summary>
        public decimal CostEach { get; set; }
        /// <summary>
        /// (sic)
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Navigation: back to Order
        /// </summary>
        public virtual Order Order { get; set; }
        /// <summary>
        /// Navigation: to Product
        /// </summary>
        public virtual Product Product { get; set; }
    }
}
