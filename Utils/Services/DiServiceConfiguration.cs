using System;
using System.Collections.Generic;

namespace Marko.Utils.Services
{
    /// <summary>
    /// Configuration type for strong typing.
    /// </summary>
    public class DiServiceConfiguration
    {
        /// <summary>
        /// List of transient services.
        /// </summary>
        public IEnumerable<DiServiceItem> Transient { get; set; }
    }
}
