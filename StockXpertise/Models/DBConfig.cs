using System.Xml.Linq;

namespace StockXpertise.Models
{
    public class DBConfig
    {
        public string? Server { get; set; }
        public string? Database { get; set; }
        public string? UserID { get; set; }
        public string? Password { get; set; }
        public string? eggs { get; set;}   


        // should get elements inside configurations.connections.mysql and set config fields
        public static Dictionary<string, string> keysMapping = new Dictionary<string, string>{
                {"server", "Server" },
                {"database", "Database" },
                {"password", "Password" },
                {"user_id", "UserID" },
            };

        public DBConfig() { }

        public DBConfig(string server, string database, string userID, string password)
        {
            Server = server;
            Database = database;
            UserID = userID;
            Password = password;
        }

        public override string ToString()
        {
            string result = "";

            foreach (var key in keysMapping.Keys)
            {

                string? value = this.GetType().GetProperty(keysMapping[key])?.GetValue(this, null)?.ToString();
                string _key = keysMapping[key];
                result += $"{_key}={value};";
            }

            return result;
        }

        public static DBConfig LoadFromXml(string path)
        {
            DBConfig config = new DBConfig();
            XElement xConfig = XElement.Load(path);

            try
            {
                var connection = xConfig.Element("connections")?.Element("mysql");

                if (connection != null)
                {
                    foreach (var element in connection.Elements("add"))
                    {
                        string? key = element.Attribute("name")?.Value;
                        string? value = element.Attribute("value")?.Value;

                        if (key != null && value != null)
                        {
                            if (keysMapping.ContainsKey(key))
                            {
                                string property = keysMapping[key];
                                config.GetType().GetProperty(property)?.SetValue(config, value);
                            }

                        }
                    }
                }
                else
                {
                    throw new Exception("No connection element found");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return config;
        }

    }
}
