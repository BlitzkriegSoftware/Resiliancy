using System;
using System.Collections.Generic;

namespace BlitzkriegSoftware.Demo.Resiliancy.WebSvc.Ef
{

    /// <summary>
    /// Table: Order
    /// </summary>
    public partial class Order
    {
        /// <summary>
        /// CTOR
        /// </summary>
        public Order()
        {
            OrderDetail = new HashSet<OrderDetail>();
        }

        /// <summary>
        /// PK
        /// </summary>
        public long OrderId { get; set; }
        /// <summary>
        /// FK
        /// </summary>
        public long CustomerId { get; set; }
        /// <summary>
        /// (sic)
        /// </summary>
        public DateTime OrderDate { get; set; }
        /// <summary>
        /// (sic)
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Navigation back
        /// </summary>
        public virtual Customer Customer { get; set; }

        /// <summary>
        /// Collection: Order Detail
        /// </summary>
        public virtual ICollection<OrderDetail> OrderDetail { get; set; }
    }
}
