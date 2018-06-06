using QuickTranslate.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTranslate.Io
{
    public static class CsvExport
    {
        /// <summary>
        /// Export a list of strings to a comma-separated values text file, a generic and easy-to-use file format.
        /// </summary>
        /// <param name="file">The path to create the file at.</param>
        /// <param name="useColumns">Determine whether to add a row defining column names for the resulting CSV file. Useful if being entered into another system.</param>
        /// <param name="categories">The list of strings, organized into categories.</param>
        public static void ExportCsvFile(string file, bool useColumns, List<Category> categories)
        {
            StringBuilder sb = new StringBuilder();

            if (useColumns)
            {
                sb.AppendLine("Category,Id,Translation");
            }

            foreach (Category cat in categories)
            {
                string name = cat.Name;

                if (name == "")
                {
                    name = "(None)";
                }

                foreach (Translation item in cat.Translations)
                {
                    sb.AppendLine("\"" + name + "\",\"" + item.Id + "\",\"" + item.TranslatedItem + "\"");
                }
            }

            File.WriteAllText(file, sb.ToString());
        }

    }
}
