using System.Collections.Generic;

namespace BlitzkriegSoftware.Demo.Resiliancy.WebSvc.Ef
{
    /// <summary>
    /// Table: Customer
    /// </summary>
    public partial class Customer
    {
        /// <summary>
        /// CTOR
        /// </summary>
        public Customer()
        {
            Order = new HashSet<Order>();
        }

        /// <summary>
        /// PK
        /// </summary>
        public long CustomerId { get; set; }
        /// <summary>
        /// (sic)
        /// </summary>
        public string NameLast { get; set; }
        /// <summary>
        /// (sic)
        /// </summary>
        public string NameFirst { get; set; }
        /// <summary>
        /// (sic)
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// (sic)
        /// </summary>
        public string Address1 { get; set; }
        /// <summary>
        /// (sic)
        /// </summary>
        public string Address2 { get; set; }
        /// <summary>
        /// (sic)
        /// </summary>
        public string Address3 { get; set; }
        /// <summary>
        /// (sic)
        /// </summary>
        public string Address4 { get; set; }
        /// <summary>
        /// (sic)
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// (sic)
        /// </summary>
        public string StateOrProvence { get; set; }
        /// <summary>
        /// (sic)
        /// </summary>
        public string PostalCode { get; set; }
        /// <summary>
        /// (sic)
        /// </summary>
        public string PhonePrimary { get; set; }
        /// <summary>
        /// (sic)
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// (sic)
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Collection: Order(s)
        /// </summary>
#pragma warning disable CA2227 // Required by Entity Framework
        public virtual ICollection<Order> Order { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
    }

}
