using System.Collections.Generic;

namespace ROIO {
    public class Configuration {
        public string root;
        public List<string> grf;
        public string system;
        public string bgm;

        public string SystemPath => $"{root}{system}";
        public string BgmPath => $"{root}{bgm}";
    }
}
