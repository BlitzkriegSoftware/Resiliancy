using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlitzkriegSoftware.AdoSqlHelper;
using Newtonsoft.Json;

namespace BlitzkriegSoftware.Demo.Resiliancy.WebSvc.Libs
{
    /// <summary>
    /// Utilities
    /// </summary>
    public static class Utilities
    {
        // See: https://www.c-sharpcorner.com/UploadFile/9bff34/3-ways-to-convert-datatable-to-json-string-in-Asp-Net-C-Sharp/

        /// <summary>
        /// Convert to JSON
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <returns>Json</returns>
        public static List<Dictionary<string, string>> ToDictionary(this DataTable dt)
        {
            var d = new List<Dictionary<string, string>>();

            if (SqlHelper.HasRows(dt))
            {
                d = dt.AsEnumerable().Select(
                       row => dt.Columns.Cast<DataColumn>().ToDictionary(
                           column => column.ColumnName,
                           column => row[column].ToString()
                       )).ToList();
            }

            return d;
        }

    }
}
