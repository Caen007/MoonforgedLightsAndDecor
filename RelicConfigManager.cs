using BepInEx.Configuration;

namespace Moonforged.LightsAndDecor
{
    /// <summary>
    /// Manages local BepInEx configuration entries for Moonforged Lights and Decor.
    /// </summary>
    public static class RelicConfigManager
    {
        public static ConfigEntry<string> CustomLampColor { get; private set; }

        public static void Init(string modName, ConfigFile config)
        {
            // Local config only. No server-side config sync is used by this mod.
            CustomLampColor = AddEntry(
                config, "Lamps", "CustomLampColor",
                "#FF7A00",
                "Custom lamp color added after the built-in Yellow, Green, Blue and Dvergr Pink colors. Use HTML hex format, for example #FF7A00."
            );
        }

        public static UnityEngine.Color GetCustomLampColor()
        {
            if (CustomLampColor != null && UnityEngine.ColorUtility.TryParseHtmlString(CustomLampColor.Value, out UnityEngine.Color color))
                return color;

            return new UnityEngine.Color(1.00f, 0.48f, 0.00f);
        }

        public static ConfigEntry<T> AddEntry<T>(ConfigFile cfg, string section, string key, T defaultValue, string description)
        {
            return cfg.Bind(section, key, defaultValue, description);
        }
    }
}
