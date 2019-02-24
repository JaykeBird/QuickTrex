using QuickTranslate.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace QuickTranslate.Io
{
    public static class CsExport
    {
        /// <summary>
        /// Export a list of translations to a C# code file containing a Dictionary.
        /// </summary>
        /// <param name="file">The path at which to create the file.</param>
        /// <param name="categories">The list of strings, organized into categories.</param>
        /// <param name="namespaceName">The name of the namespace to use in the code file.</param>
        /// <param name="className">The name of the class to use in the code file.</param>
        /// <param name="readOnly">Determine if the dictionary should be written as a ReadOnlyDictionary. Only available in .NET Framework 4.5 and later, and on .NET Standard.</param>
        public static void ExportCsharpFile(string file, List<Category> categories, string namespaceName, string className, bool readOnly)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine();
            sb.AppendLine("namespace " + namespaceName);
            sb.AppendLine("{");
            sb.AppendLine("    ");
            sb.AppendLine("    public static class " + className);
            sb.AppendLine("    {");
            sb.AppendLine("        // String resources");
            if (readOnly)
            {
                sb.AppendLine("        static ReadOnlyDictionary<string, string> items = new ReadOnlyDictionary<string, string>()");
            }
            else
            {
                sb.AppendLine("        static Dictionary<string, string> items = new Dictionary<string, string>()");
            }
            sb.AppendLine("        {");

            foreach (Category cat in categories)
            {
                string name = cat.Name + ".";

                if (name == ".")
                {
                    name = "";
                }

                foreach (Translation item in cat.Translations)
                {
                    sb.AppendLine("            { \"" + name + item.Id + "\", \"" + item.TranslatedItem + "\"},");
                }
            }

            sb.AppendLine("        };");
            sb.AppendLine("        ");
            sb.AppendLine("        public static string GetItem(string id)");
            sb.AppendLine("        {");
            sb.AppendLine("            return items[id];");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            string result = sb.ToString();

            // try to avoid invalid string literals
            result = result.Replace("\\", "\\\\");
            result = result.Replace("\"", "\\\"");

            File.WriteAllText(file, result);
        }

    }
}
