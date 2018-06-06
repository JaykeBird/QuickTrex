using QuickTranslate.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTranslate.Io
{
    public static class JsonExport
    {
        /// <summary>
        /// Export a list of strings to a JSON file, without separating the categories into distinct elements.
        /// </summary>
        /// <param name="file">The path to create the file at.</param>
        /// <param name="categories">The list of strings, organized into categories.</param>
        public static void ExportBasicJsonFile(string file, List<Category> categories)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("{ \"strings\": {");
            sb.AppendLine("  \"string\": [");

            foreach (Category cat in categories)
            {
                string name = cat.Name + "_";

                if (name == "_")
                {
                    name = "";
                }

                foreach (Translation item in cat.Translations)
                {
                    sb.AppendLine("    {\"name\": \"" + name + item.Id + "\", \"value\": \"" + item.TranslatedItem + "\" },");
                }
            }
            sb.Remove(sb.Length - 3, 1);
            sb.AppendLine("  ]");
            sb.AppendLine("} }");

            File.WriteAllText(file, sb.ToString());
        }

        /// <summary>
        /// Export a list of strings to a JSON file, separating each category into a distinct element.
        /// </summary>
        /// <param name="file">The path to create the file at.</param>
        /// <param name="categories">The list of strings, organized into categories.</param>
        public static void ExportCategorizedJsonFile(string file, List<Category> categories)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("{ \"translations\": {");
            sb.AppendLine("  \"category\": [");

            foreach (Category cat in categories)
            {
                
                sb.AppendLine("    { \"name\": \"" + cat.Name + "\",");
                sb.AppendLine("      \"string\": [");

                foreach (Translation item in cat.Translations)
                {
                    sb.AppendLine("        {\"name\": \"" + item.Id + "\", \"value\": \"" + item.TranslatedItem + "\" },");
                }

                sb.Remove(sb.Length - 3, 1);
                sb.AppendLine("      ]");
                sb.AppendLine("    },");
            }

            sb.Remove(sb.Length - 3, 1);
            sb.AppendLine("  ]");
            sb.AppendLine("} }");

            File.WriteAllText(file, sb.ToString());
        }

    }
}
