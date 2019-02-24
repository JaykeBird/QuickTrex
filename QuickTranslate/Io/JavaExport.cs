using QuickTranslate.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTranslate.Io
{
    public static class JavaExport
    {

        public static void ExportJavaMap(string file, List<Category> categories, string className, bool hashtable = true)
        {
            StringBuilder sb = new StringBuilder();
            if (hashtable)
            {
                sb.AppendLine("import java.util.Hashtable;");
            }
            else
            {
                sb.AppendLine("import java.util.HashMap;");
                sb.AppendLine("import java.util.Map;");
            }
            sb.AppendLine();
            sb.AppendLine("public class " + className);
            sb.AppendLine("{");
            sb.AppendLine("    // String resources");
            if (hashtable)
            {
                sb.AppendLine("    static final Hashtable<String, String> items = new Hashtable<string, string>()");
            }
            else
            {
                sb.AppendLine("    static final HashMap<String, String> items = new HashMap<string, string>()");
            }
            sb.AppendLine("    {{");

            foreach (Category cat in categories)
            {
                string name = cat.Name + ".";

                if (name == ".")
                {
                    name = "";
                }

                foreach (Translation item in cat.Translations)
                {
                    sb.AppendLine("        put(\"" + name + item.Id + "\", \"" + item.TranslatedItem + "\");");
                }
            }

            sb.AppendLine("    }};");
            sb.AppendLine("    ");
            sb.AppendLine("    public static String getItem(String id)");
            sb.AppendLine("    {");
            sb.AppendLine("        return items.get(id);");
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
