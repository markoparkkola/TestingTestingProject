using System;
using System.Collections.Generic;
using System.Text;

namespace Marko.Utils.Configuration
{
    /// <summary>
    /// Options for configuring DI.
    /// </summary>
    public class DiConfigurationOptions
    {
        /// <summary>
        /// Connection string where settings are to be loaded.
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// Set this to match the environment target configured in the database. For example "DEBUG".
        /// </summary>
        public string Environment { get; set; }
    }
}
