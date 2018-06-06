using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTranslate.Base
{
    /// <summary>
    /// Defines a collection of related translatable strings.
    /// </summary>
    /// <remarks>
    /// It's a good practice to use categories to organize your strings, such as a category for all strings related to a certain function or found in a certain location. It makes locating strings
    /// easier for you (and your translators), and also makes identifying them in code easier as well.
    /// </remarks>
    public class Category
    {
        /// <summary>
        /// Create a new, blank category.
        /// </summary>
        public Category()
        {
            // empty constructor
        }

        /// <summary>
        /// Create a new category with a name preset.
        /// </summary>
        /// <param name="name"></param>
        public Category(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The name of the category.
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// A list of translatable strings in the category.
        /// </summary>
        public List<Translation> Translations { get; set; } = new List<Translation>();

        /// <summary>
        /// Add a string to the category
        /// </summary>
        /// <param name="translation">The translatable string to add.</param>
        public void AddTranslation(Translation translation)
        {
            Translations.Add(translation);
        }

        /// <summary>
        /// Add a string to the category
        /// </summary>
        /// <param name="id">The identifying name of the string.</param>
        /// <param name="translated">The value of the string.</param>
        public void AddTranslation(string id, string translated)
        {
            Translations.Add(new Translation(id, translated));
        }

        /// <summary>
        /// Insert a string into the category at a specified position.
        /// </summary>
        /// <param name="index">The zero-based index to insert the string. Use -1 to insert at the end (same as using <see cref="AddTranslation(Translation)"/>).</param>
        /// <param name="translation">The translatable string to insert.</param>
        /// <returns></returns>
        public bool InsertTranslation(int index, Translation translation)
        {
            //int index = GetTranslationIndex(baseId);

            if (index != -1)
            {
                try
                {
                    Translations.Insert(index, translation);
                }
                catch (ArgumentOutOfRangeException)
                {
                    Translations.Add(translation);
                }
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Insert a string into the category at a specified location.
        /// </summary>
        /// <param name="index">The zero-based index to insert the string. Use -1 to insert at the end (same as using <see cref="AddTranslation(string, string)"/>).</param>
        /// <param name="id">The identifying name for the string.</param>
        /// <param name="translated">The value of the string.</param>
        /// <returns></returns>
        public bool InsertTranslation(int index, string id, string translated)
        {
            return InsertTranslation(index, new Translation(id, translated));
        }

        /// <summary>
        /// Get the value of a translatable string.
        /// </summary>
        /// <param name="id">The identifying name of the string.</param>
        /// <returns></returns>
        public string GetTranslationString(string id)
        {
            foreach (Translation item in Translations)
            {
                if (item.Id == id)
                {
                    return item.TranslatedItem;
                }
            }

            return null;
        }

        /// <summary>
        /// Get a translatable string.
        /// </summary>
        /// <param name="id">The identifying name of the string.</param>
        /// <returns></returns>
        public Translation GetTranslation(string id)
        {
            foreach (Translation item in Translations)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// Get the location (index) of a translatable string within the category's list.
        /// </summary>
        /// <param name="id">The identifying name of the string.</param>
        /// <returns></returns>
        public int GetTranslationIndex(string id)
        {
            int index = -1;

            foreach (Translation item in Translations)
            {
                if (item.Id == id)
                {
                    break;
                }
                index++;
            }

            return index;
        }

        /// <summary>
        /// Remove a translatable string from the category.
        /// </summary>
        /// <param name="id">The identifying name of the string.</param>
        /// <returns></returns>
        public bool RemoveTranslation(string id)
        {
            Translation itemToRemove = null;

            foreach (Translation item in Translations)
            {
                if (item.Id == id)
                {
                    itemToRemove = item;
                }
            }

            if (itemToRemove != null)
            {
                return Translations.Remove(itemToRemove);
            }
            else
            {
                return false;
            }
        }

    }
}
