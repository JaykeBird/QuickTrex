using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTranslate.Base
{
    /// <summary>
    /// A structure storing properties and extra data about this particular translation file.
    /// </summary>
    public class Properties
    {
        
        /// <summary>
        /// The language the translations are written in.
        /// </summary>
        public string Language { get; set; } = "";

        /// <summary>
        /// The author of the product using the translations.
        /// </summary>
        public string Author { get; set; } = "";

        /// <summary>
        /// The product using the translations.
        /// </summary>
        public string Product { get; set; } = "";

        /// <summary>
        /// The author of the translations.
        /// </summary>
        public string Translator { get; set; } = "";

        /// <summary>
        /// A short-text string containing a way to contact the translation author, if desired.
        /// </summary>
        public string TranslatorContact { get; set; } = "";

        /// <summary>
        /// A long-text explanation about the file's purpose or containing any extra data or notes.
        /// </summary>
        public string Description { get; set; } = "";
    }
}
