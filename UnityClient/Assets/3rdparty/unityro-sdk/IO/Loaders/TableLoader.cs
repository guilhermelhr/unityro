using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ROIO.Loaders {
    public class TableLoader {

        private static readonly Regex RegexComments = new Regex(@"\n(\/\/[^\n]+)", RegexOptions.Multiline);

        public static IEnumerable<object> LoadTable(string data, int size) {
            //remove comments
            string content = RegexComments.Replace("\n" + data, "");
            string[] elements = content.Split('#');
            string[] args = new string[size + 1];

            for (int i = 0; i < elements.Length; i++) {
                if (i % size == 0) {
                    if (i != 0) {
                        yield return args;
                    }
                    args[i % size] = i.ToString();
                }

                args[(i % size) + 1] = elements[i].Trim();
            }
        }
    }
}
