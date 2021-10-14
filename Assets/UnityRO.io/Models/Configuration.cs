using System.Collections.Generic;

namespace ROIO {
    public class Configuration {
        public string root;
        public List<string> grf;
        public string system;
    }

    public static class ConfigurationExtensions {

        public static string GetSystemPath(this Configuration configuration) {
            return $"{configuration.root}{configuration.system}";
        }
    }
}
