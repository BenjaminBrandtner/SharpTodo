using Newtonsoft.Json;
using System;
using System.Dynamic;
using System.IO;

namespace Backend
{
    public class ConfigManager
    {
        private JsonSerializer serializer;

        public ConfigManager()
        {
            serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
        }

        /// <summary>
        /// Reads config.ini. If no such file exists, returns a config object with empty Properties.
        /// </summary>
        /// <returns></returns>
        public dynamic Read()
        {
            try
            {
                using(StreamReader streamReader = new StreamReader("config.ini", System.Text.Encoding.UTF8))
                {
                    return serializer.Deserialize(streamReader, typeof(ExpandoObject));
                }
            }
            catch (FileNotFoundException)
            {
                dynamic standardSettings = new ExpandoObject();
                standardSettings.UserID = "";
                standardSettings.ApiToken = "";

                return standardSettings;
            }
        }

        /// <summary>
        /// Writes the config object to config.ini.
        /// </summary>
        /// <param name="config"></param>
        public void Write(dynamic config)
        {
            using (StreamWriter streamWriter = new StreamWriter("config.ini", false, System.Text.Encoding.UTF8))
            {
                serializer.Serialize(streamWriter, config);
            }
        }
    }
}