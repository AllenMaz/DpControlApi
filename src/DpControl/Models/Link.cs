using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Models
{
    public class Link
    {
        /// <summary>
        /// Relation
        /// </summary>
        public string Rel { get; set; }

        /// <summary>
        /// HTTP Method
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Relation Url
        /// </summary>
        public string Href { get; set; }

        public Link(string rel,string method,string href)
        {
            Rel = rel;
            Method = method;
            Href = href;
        }
    }
}
