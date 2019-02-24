using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using QuickTranslate.Base;

namespace QuickTranslate.Io
{
    public static class XmlIo
    {
        /// <summary>
        /// Load a document from a file.
        /// </summary>
        /// <param name="file">The path of the file.</param>
        /// <returns></returns>
        public static Document LoadXmlFile(string file)
        {
            XDocument xdoc = XDocument.Load(file);
            List<Category> cats = new List<Category>();

            bool malformed = false;

            foreach (XElement ele in xdoc.Root.Elements("category"))
            {
                Category c = LoadCategory(ele);

                if (c != null) // null check to avoid malformed files
                {
                    cats.Add(c);
                }
                else
                {
                    malformed = true;
                }

            }

            Base.Properties p = new Base.Properties();
            XElement elp = xdoc.Root.Element("properties");

            if (elp != null)
            {
                p.Product = elp.Attribute("product")?.Value ?? "";
                p.Author = elp.Attribute("author")?.Value ?? "";
                p.Language = elp.Attribute("lang")?.Value ?? "";
                p.Translator = elp.Attribute("tlr")?.Value ?? "";
                p.TranslatorContact = elp.Attribute("tlrcontact")?.Value ?? "";
                p.Description = elp.Attribute("desc")?.Value ?? "";
            }

            Document d = new Document
            {
                Filename = file,
                Categories = cats,
                Properties = p,
                HadMalformedElements = malformed
            };

            return d;
        }

        /// <summary>
        /// Load a category from an XML element.
        /// </summary>
        /// <param name="element">The XML element representing the category.</param>
        /// <returns></returns>
        static Category LoadCategory(XElement element)
        {
            Category cat = new Category();

            // make sure the category has a name
            cat.Name = element.Attribute("name")?.Value;

            if (cat.Name != null)
            {

                foreach (XElement item in element.Elements("item"))
                {
                    Translation tl = new Translation
                    {
                        Id = item.Attribute("id")?.Value ?? "",
                        Note = item.Attribute("note")?.Value ?? "", // (older files may not have a note. in that case, just leave it empty)
                        TranslatedItem = item.Value
                    };

                    cat.AddTranslation(tl);
                }

                return cat;
            }
            else
            {
                // if there is no name, just return nothing
                return null;
            }
        }

        /// <summary>
        /// Save a document to a file.
        /// </summary>
        /// <param name="file">The path to create the file at.</param>
        /// <param name="doc">The data of the document.</param>
        public static void SaveXmlFile(string file, Document doc)
        {
            XDocument xdoc = new XDocument();
            XElement root = new XElement("root");

            if (doc.Properties != null)
            {
                Base.Properties p = doc.Properties;

                XElement prop = new XElement("properties");
                prop.SetAttributeValue("product", p.Product);
                prop.SetAttributeValue("author", p.Author);
                prop.SetAttributeValue("lang", p.Language);
                prop.SetAttributeValue("tlr", p.Translator);
                prop.SetAttributeValue("tlrcontact", p.TranslatorContact);
                prop.SetAttributeValue("desc", p.Description);
            }

            foreach (Category cat in doc.Categories)
            {
                XElement cel = new XElement("category");
                cel.SetAttributeValue("name", cat.Name);

                foreach (Translation item in cat.Translations)
                {
                    XElement tre = new XElement("item");
                    tre.SetAttributeValue("id", item.Id);
                    tre.SetAttributeValue("note", item.Note);
                    tre.SetValue(item.TranslatedItem);

                    cel.Add(tre);
                }

                root.Add(cel);
            }

            xdoc.Add(root);
            xdoc.Save(file);
        }

    }
}
