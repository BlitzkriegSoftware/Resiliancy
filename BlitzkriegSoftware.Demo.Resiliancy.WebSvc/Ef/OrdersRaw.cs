using System;

namespace BlitzkriegSoftware.Demo.Resiliancy.WebSvc.Ef
{
    /// <summary>
    /// Table: OrdersRaw (ETL)
    /// </summary>
    public partial class OrdersRaw
    {
        /// <summary>
        /// Ref: To Customer
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Ref: To Product
        /// </summary>
        public long? ProductId { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        public int? Quantity { get; set; }
        /// <summary>
        /// Order Date
        /// </summary>
        public DateTime? OrderDate { get; set; }
    }
}
