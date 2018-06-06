using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using QuickTranslate.Base;

namespace QuickTranslate.Io
{
    public static class XamlResExport
    {
        /// <summary>
        /// Export the list of strings to a XAML ResourceDictionary, used in applications with a XAML-based UI system.
        /// </summary>
        /// <param name="file">The path to create the file at.</param>
        /// <param name="categories">The list of strings, organized into categories.</param>
        public static void ExportXamlResDictionary(string file, List<Category> categories)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<ResourceDictionary xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"");
            sb.AppendLine("                    xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"");
            sb.AppendLine("                    xmlns:system=\"clr-namespace:System;assembly=mscorlib\">");
            sb.AppendLine();
            sb.AppendLine("    <!-- String resources -->");

            foreach (Category cat in categories)
            {
                string name = cat.Name + ".";

                if (name == ".")
                {
                    name = "";
                }

                foreach (Translation item in cat.Translations)
                {
                    sb.AppendLine("    <system:String x:Key=\"" + name + item.Id + "\">" + item.TranslatedItem + "</system:String>");
                }
            }

            sb.AppendLine();
            sb.AppendLine("</ResourceDictionary>");

            File.WriteAllText(file, sb.ToString());
        }

    }
}
