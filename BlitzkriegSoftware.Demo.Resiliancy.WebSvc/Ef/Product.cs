using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlitzkriegSoftware.Demo.Resiliancy.WebSvc.Ef
{
    /// <summary>
    /// Table: Product
    /// </summary>
    public partial class Product
    {
        /// <summary>
        /// CTOR
        /// </summary>
        public Product()
        {
            OrderDetail = new HashSet<OrderDetail>();
        }

        /// <summary>
        /// PK
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        /// (sic)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// (sic)
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// (sic)
        /// </summary>
        public bool? IsActive { get; set; }
        /// <summary>
        /// (sic)
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Collection: OrderDetail
        /// </summary>
        public virtual ICollection<OrderDetail> OrderDetail { get; set; }
    }
}
