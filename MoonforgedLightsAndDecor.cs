using System.Collections;
using System.IO;
using System.Reflection;
using BepInEx;
using UnityEngine;
using Jotunn.Managers;

namespace Moonforged.LightsAndDecor
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    public class MoonforgedLightsAndDecor : BaseUnityPlugin
    {
        public const string PluginGUID = "Moonforged.LightsAndDecor";
        public const string PluginName = "Moonforged Lights and Decor";
        public const string PluginVersion = "1.0.0";

        private AssetBundle relicsBundle;


        private void Awake()
        {
            // INIT CONFIG SYSTEM
            RelicConfigManager.Init(PluginGUID, Config);


            string resourcePath = GetPlatformBundleResourcePath();

            relicsBundle = EmbeddedAssetBundleLoader.LoadBundle(resourcePath);

            if (relicsBundle == null)
            {
                Logger.LogError("Failed to load embedded AssetBundle: " + resourcePath);
                return;
            }


            // Initialize configurable hammer categories (furniture/building/clutter/statues)
            RelicRegistrar.InitConfig(Config);

            foreach (var category in RelicRegistrar.GetAllCategories())
                PieceManager.Instance.AddPieceCategory(category);

            PrefabManager.OnPrefabsRegistered += OnPrefabsRegistered;
        }

        private static string GetPlatformBundleResourcePath()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.OSXEditor:
                    return "Moonforged.LightsAndDecor.mld_windows";

                default:
                    return "Moonforged.LightsAndDecor.mld_windows";
            }
        }

        private void OnDestroy()
        {
            PrefabManager.OnPrefabsRegistered -= OnPrefabsRegistered;
        }

        private void OnPrefabsRegistered()
        {
            StartCoroutine(DelayedRegister(relicsBundle));
        }

        private IEnumerator DelayedRegister(AssetBundle bundle)
        {
            while (ZNetScene.instance == null)
            {
                yield return null;
            }

            RelicRegistrar.RegisterAllRelics(bundle);
        }
    }

    public static class EmbeddedAssetBundleLoader
    {
        public static AssetBundle LoadBundle(string resourcePath)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            {
                if (stream == null)
                {
                    Debug.LogError("AssetBundle resource not found: " + resourcePath);
                    return null;
                }

                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return AssetBundle.LoadFromMemory(memoryStream.ToArray());
                }
            }
        }
    }
}
