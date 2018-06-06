using System;
using System.Collections.Generic;
using System.Resources;
using QuickTranslate.Base;

namespace QuickTranslate.Io
{
    public static class ResxExport
    {
        /// <summary>
        /// Export a list of strings to a Resx file, an XML-based file format used in .NET applications.
        /// </summary>
        /// <param name="file">The path to create the file at.</param>
        /// <param name="categories">The list of strings, organized into categories.</param>
        public static void ExportToResx(string file, List<Category> categories)
        {
            ResXResourceWriter resX = new ResXResourceWriter(file);

            foreach (Category cat in categories)
            {
                string name = cat.Name + ".";

                if (name == ".")
                {
                    name = "";
                }

                foreach (Translation item in cat.Translations)
                {
                    resX.AddResource(name + item.Id, item.TranslatedItem);
                }
            }

            resX.Generate();
            resX.Close();
        }

        /// <summary>
        /// Export a list of strings to a .resources file, a binary file format used in .NET applications.
        /// </summary>
        /// <param name="file">The path to create the file at.</param>
        /// <param name="categories">The list of strings, organized into categories.</param>
        public static void ExportToBinaryRes(string file, List<Category> categories)
        {
            ResourceWriter res = new ResourceWriter(file);

            foreach (Category cat in categories)
            {
                string name = cat.Name + ".";

                if (name == ".")
                {
                    name = "";
                }

                foreach (Translation item in cat.Translations)
                {
                    res.AddResource(name + item.Id, item.TranslatedItem);
                }
            }

            res.Generate();
            res.Close();
        }

    }
}
