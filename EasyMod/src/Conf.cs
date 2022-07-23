using BepInEx.Configuration;

namespace EasyMod
{
    class Conf
    {
        ConfigFile Config;

        private static int order = 0;

        public Conf(ConfigFile Config)
        { 
            this.Config = Config;
        }

        public ConfigSet<T> BindSet<T>(string catagory, string name, T defaultValue, string description = null)
        {
            order = order - 1;
            var obj = new ConfigSet<T>(Config, catagory, name, defaultValue, order, description);
            order = order - 1;
            return obj;
        }

        public ConfigOne<T> Bind<T>(string catagory, string name, T defaultValue, string description = null)
        {
            order = order + 1;
            return new ConfigOne<T>(Config, catagory, name, defaultValue, order, description);
        }
    }

    public class ConfigSet<T>
    {

        public ConfigEntry<bool> Toggle;
        public ConfigEntry<T> Conf;

        public ConfigSet(ConfigFile Config, string catagory, string name, T defaultValue, int order, string description = null)
        {
            Toggle = Config.Bind(catagory, $"{name}Toggle", false, new ConfigDescription($"Enable/Disable {name}. Default: false", null, new ConfigurationManagerAttributes { Order = order }));
            Conf = Config.Bind(catagory, $"{name}Value", defaultValue, new ConfigDescription($"{name} value.{(description == null || description == "" ? "" : " " + description)} Default: {defaultValue}", null, new ConfigurationManagerAttributes { Order = order - 1 }));
        }

        public bool IsEnabled()
        {
            return Toggle.Value;
        }

        public T Value()
        {
            return Conf.Value;
        }
    }

    public class ConfigOne<T>
    {
        public ConfigEntry<T> Conf;

        public ConfigOne(ConfigFile Config, string catagory, string name, T defaultValue, int order, string description = null)
        {
            Config.Bind(catagory, name, defaultValue, new ConfigDescription(description, null, new ConfigurationManagerAttributes { Order = order }));
        }

        public T Value()
        {
            return Conf.Value;
        }
    }
}
