using QuickTranslate.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTranslate.Io
{
    public class AxmlExport
    {
        /// <summary>
        /// Export a list of strings to an Android XML string table.
        /// </summary>
        /// <param name="file">The path at which to create the file.</param>
        /// <param name="categories">The list of strings, organized into categories.</param>
        public static void ExportAndroidStringFile(string file, List<Category> categories)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.AppendLine("<resources>");
            sb.AppendLine("    <!-- String resources -->");

            foreach (Category cat in categories)
            {
                string name = cat.Name + "_";

                if (name == "_")
                {
                    name = "";
                }

                foreach (Translation item in cat.Translations)
                {
                    sb.AppendLine("    <string name=\"" + name + item.Id + "\">" + item.TranslatedItem + "</string>");
                }
            }
            sb.AppendLine("</resources>");

            File.WriteAllText(file, sb.ToString());
        }
    }
}
