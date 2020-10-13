namespace BlitzkriegSoftware.Demo.Resiliancy.WebSvc.Ef
{
    /// <summary>
    /// Table: ErrorLog
    /// </summary>
    public partial class ErrorLog
    {
        /// <summary>
        /// (Auto)
        /// </summary>
        public long ErrorLogId { get; set; }
        /// <summary>
        /// Step #
        /// </summary>
        public string Step { get; set; }
        /// <summary>
        /// (sic)
        /// </summary>
        public string Comment { get; set; }
    }
}
