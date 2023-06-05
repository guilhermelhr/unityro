using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace ROIO {
    public static class ConfigurationLoader {

        private const string CFG_NAME = "config.json";
        private static Configuration DefaultConfiguration = new Configuration() {
            root = "C:/",
            grf = new List<string>(),
            system = "System/"
        };

        public static Configuration Init() {
            try {
                JObject configString = FileManager.Load(CFG_NAME) as JObject;
                return JsonConvert.DeserializeObject<Configuration>(configString.ToString());
            } catch {
                SaveDefaultConfigs();
                return DefaultConfiguration;
            }
        }

        private static void SaveDefaultConfigs() {
            var stream = File.Open($"{Application.streamingAssetsPath}/{CFG_NAME}", FileMode.Create);
            var serializedConfigs = JsonConvert.SerializeObject(DefaultConfiguration);
            stream.Write(Encoding.UTF8.GetBytes(serializedConfigs), 0, serializedConfigs.Length);
            stream.Close();
        }
    }
}
