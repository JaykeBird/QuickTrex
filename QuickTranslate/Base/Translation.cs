using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTranslate.Base
{
    /// <summary>
    /// A representation of a translatable string.
    /// </summary>
    public class Translation
    {

        /// <summary>
        /// The identifying name of the string.
        /// </summary>
        public string Id { get; set; } = "";

        /// <summary>
        /// The value of the string.
        /// </summary>
        public string TranslatedItem { get; set; } = "";

        /// <summary>
        /// A note for translators to consider while making the translation.
        /// </summary>
        public string Note { get; set; } = "";

        /// <summary>
        /// Create a new translatable string with a preset identifying name and value.
        /// </summary>
        /// <param name="id">The identifying name of the string.</param>
        /// <param name="translated">The value of the string.</param>
        public Translation(string id, string translated)
        {
            Id = id;
            TranslatedItem = translated;
        }

        /// <summary>
        /// Create a new translatable string.
        /// </summary>
        public Translation()
        {

        }

    }
}
