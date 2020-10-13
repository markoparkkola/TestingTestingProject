using System;
using System.Collections.Generic;
using System.Text;

namespace Marko.Utils.Services
{
    /// <summary>
    /// Configuration file for strong typing.
    /// </summary>
    public class DiServiceItem
    {
        /// <summary>
        /// Service (interface) namespace and name.
        /// </summary>
        public string Service { get; set; }
        /// <summary>
        /// Implementing type namespace and name.
        /// </summary>
        public string Type { get; set; }
    }
}
