using System;
using System.Collections.Generic;

namespace Marko.Utils.Services
{
    public class DiServiceConfiguration
    {
        public IEnumerable<DiServiceItem> Transient { get; set; }
    }
}
