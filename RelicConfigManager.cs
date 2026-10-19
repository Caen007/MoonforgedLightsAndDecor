using BepInEx.Configuration;

namespace Moonforged.LightsAndDecor
{
    /// <summary>
    /// Manages local BepInEx configuration entries for Moonforged Lights and Decor.
    /// </summary>
    public static class RelicConfigManager
    {
        public static ConfigEntry<string> CustomLampColor { get; private set; }
        public static ConfigEntry<bool> EnableFlare { get; private set; }
        public static ConfigEntry<float> BulbBrightness { get; private set; }

        public static void Init(string modName, ConfigFile config)
        {
            // Local config only. No server-side config sync is used by this mod.
            BulbBrightness = config.Bind(
                "Lamps", "BulbBrightness", 1.5f,
                new ConfigDescription("Bulb emission brightness, separate from ground lighting. Lower values preserve more color; higher values create stronger glow but may wash out colors. Applies immediately when changed.",
                    new AcceptableValueRange<float>(0f, 10f))
            );

            EnableFlare = AddEntry(
                config, "Lamps", "EnableFlare",
                true,
                "Enable the orange flare particle effects around lamp bulbs. Disable to hide all lamp flares without changing lamp lighting or colors. Applies immediately when this setting changes."
            );

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
