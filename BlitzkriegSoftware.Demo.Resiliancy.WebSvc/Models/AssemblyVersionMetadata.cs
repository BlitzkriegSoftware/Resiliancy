using System;

namespace BlitzkriegSoftware.Demo.Resiliancy.WebSvc.Models
{
    /// <summary>
    /// Model: Assembly Version Metadata
    /// </summary>
    public class AssemblyVersionMetadata
    {
        /// <summary>
        /// Gets or sets copyright
        /// </summary>
        public string Copyright { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets company
        /// </summary>
        public string Company { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets description
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets semantic Version <para>See: semver.org</para>
        /// </summary>
        public string SemanticVersion { get; set; } = string.Empty;

        /// <summary>
        /// Gets major version as it occurs on the path
        /// </summary>
        public string MajorVersion
        {
            get
            {
                var version = "1";
                if (!string.IsNullOrWhiteSpace(this.SemanticVersion))
                {
                    var data = this.SemanticVersion.Split(new char[] { '.' });
                    if (data.Length > 1)
                    {
                        version = data[0];
                    }
                }

                return $"v{version}";
            }
        }

        /// <summary>
        /// Gets or sets product
        /// </summary>
        public string Product { get; set; } = string.Empty;

        /// <summary>
        /// Put the Uri to your release notes here
        /// </summary>
        public Uri ReleaseNotesUrl { get; set; } = new Uri("https://github.com/BlitzkriegSoftware/Resiliancy");

        /// <summary>
        /// Team Name 
        /// </summary>
        public string TeamName { get; set; } = "Blitzkrieg Software";

        /// <summary>
        /// TeamEmail
        /// </summary>
        public string TeamEMail { get; set; } = "stuart.t.williams@outlook.com";

        /// <summary>
        /// Property Set
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="value">value</param>
        public void PropertySet(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            switch (name.ToLowerInvariant().Trim())
            {
                case "assemblycopyrightattribute": this.Copyright = value; break;
                case "assemblycompanyattribute": this.Company = value; break;
                case "assemblydescriptionattribute": this.Description = value; break;
                case "assemblyfileversionattribute": this.SemanticVersion = value; break;
                case "assemblyproductattribute": this.Product = value; break;
                case "gtpteam": this.TeamName = value; break;
                case "gtpemail": this.TeamEMail = value; break;
                case "gtpurl": this.ReleaseNotesUrl = new Uri(value); break;
            }
        }

        /// <summary>
        /// Formatted String
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{this.Product} {this.SemanticVersion} by {this.TeamName}, {this.TeamEMail}\n{this.Copyright}\n{this.ReleaseNotesUrl}\n{this.Description}";
        }

    }
}
