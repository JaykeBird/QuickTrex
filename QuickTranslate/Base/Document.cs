using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTranslate.Base
{
    /// <summary>
    /// A representation of a translation file.
    /// </summary>
    public class Document
    {
        /// <summary>
        /// The list of categories within the file.
        /// </summary>
        public List<Category> Categories { get; set; } = new List<Category>();

        /// <summary>
        /// The properties associated with the file.
        /// </summary>
        public Properties Properties { get; set; } = new Properties();

        /// <summary>
        /// The name of the file, used to mark its location in the filesystem.
        /// </summary>
        public string Filename { get; set; } = "";

    }
}
