using System.Collections.Generic;
using System.Linq;
using Jotunn.Entities;
using Jotunn.Configs;
using UnityEngine;
using UnityEngine.Rendering;
using Jotunn.Managers;
using Moonforged.LightsAndDecor;
using BepInEx.Configuration;

namespace Moonforged.LightsAndDecor
{
    public class RelicRegistration
    {
        public string PrefabName;
        public string DisplayName;
        public RequirementConfig[] Requirements;
        public string Description;
        public string Category;
        public int Comfort;

        public RelicRegistration(string prefab, string display, RequirementConfig[] reqs, string desc, string cat, int comfort = 0)
        {
            PrefabName = prefab;
            DisplayName = display;
            Requirements = reqs;
            Description = desc;
            Category = cat;
            Comfort = comfort;
        }
    }

    public static class RelicRegistrar
    {
        private static bool wasAlreadyRegistered = false; // HOTFIX stays untouched

        // Configurable hammer categories
        private static ConfigEntry<string> CustomHammerTabConfig;
        private static ConfigEntry<string> FurnitureCategoryConfig;
        private static ConfigEntry<string> BuildingCategoryConfig;
        private static ConfigEntry<string> ClutterCategoryConfig;
        private static ConfigEntry<string> StatuesCategoryConfig;

        public static readonly List<RelicRegistration> AllRegistrations = new()
        {
            //--------------------- Furniture
            
            // 1.
            new RelicRegistration("M_FineWood_Throne","Moonforged FineWood Throne", new[]{
                new RequirementConfig("FineWood",20)
            },"A custom decorative piece.","furniture", 2),

            // 2.

new RelicRegistration("M_Werewolf_Throne","Moonforged Werewolf Throne", new[]{
    new RequirementConfig("WolfPelt",5),
    new RequirementConfig("TrophyFenring",3),
    new RequirementConfig("Iron",5)
},"A custom decorative piece.","furniture", 2),

// 3.

new RelicRegistration("M_Wolf_Throne","Moonforged Geirrhafa`s Throne", new[]{
    new RequirementConfig("FineWood",5),
    new RequirementConfig("WolfPelt",5),
    new RequirementConfig("Silver",5),
    new RequirementConfig("TrophyCultist_Hildir",1)
},"A custom decorative piece.","furniture", 2),

// 4.

new RelicRegistration("M_Iron_Throne","Moonforged Iron Throne", new[]{
    new RequirementConfig("Iron",10),
    new RequirementConfig("Bronze",10)

},"A custom decorative piece.","furniture", 2),

                
            
            // ---------------------- Rugs ------------------------


            // 5.
            new RelicRegistration("M_PersianRug_1","Persian Rug I", new[]{
                new RequirementConfig("JuteRed",4),new RequirementConfig("DeerHide",4)
            },"A custom decorative piece.","furniture"),

            // 6.
            new RelicRegistration("M_PersianRug_2","Persian Rug II", new[]{
                new RequirementConfig("JuteRed",2),new RequirementConfig("DeerHide",4)
            },"A custom decorative piece.","furniture"),

            
            
            // 7.
            new RelicRegistration("M_PersianRug_4","Regal Persian Rug", new[]{
                new RequirementConfig("JuteRed",2),new RequirementConfig("DeerHide",4)
            },"A custom decorative piece.","furniture"),

            // 8.
            new RelicRegistration("M_Jute_Stairs_Rug","Jute Stair Rug", new[]{
                new RequirementConfig("JuteRed",4),new RequirementConfig("DeerHide",4)
            },"A custom decorative piece.","furniture"),

            // 9.
            new RelicRegistration("M_Jute_Stairs_Rug_Blue","Blue Jute Stair Rug", new[]{
                new RequirementConfig("JuteBlue",2),new RequirementConfig("DeerHide",4)
            },"A custom decorative piece.","furniture"),

            // 10.
            new RelicRegistration("M_Jute_Stairs_Rug_Blue_Runed","Runed Blue Jute Stair Rug", new[]{
                new RequirementConfig("JuteBlue",2),new RequirementConfig("DeerHide",4)
            },"A custom decorative piece.","furniture"),

            // 11.
            new RelicRegistration("M_Jute_Stairs_Rug_Small","Small Jute Stair Rug", new[]{
                new RequirementConfig("JuteRed",2),new RequirementConfig("DeerHide",4)
            },"A custom decorative piece.","furniture"),

            // 12.
            new RelicRegistration("M_Jute_Stairs_Runed","Runed Jute Stair Rug", new[]{
                new RequirementConfig("JuteRed",4),new RequirementConfig("DeerHide",4)
            },"A custom decorative piece.","furniture"),

            // 13.
            new RelicRegistration("M_Stairs_Rug_Wool","Wool Stair Rug", new[]{
                new RequirementConfig("DeerHide",4)
            },"A custom decorative piece.","furniture"),

            // 14.
            new RelicRegistration("M_Stairs_Rug_Green_Runed","Runed Green Stair Rug", new[]{
                new RequirementConfig("JuteRed",4), new RequirementConfig("Ooze",1)
            },"A custom decorative piece.","furniture"),

            // 15.
            new RelicRegistration("M_Stairs_Rug_Green","Green Stair Rug", new[]{
                new RequirementConfig("JuteRed",4), new RequirementConfig("Ooze",1)
            },"A custom decorative piece.","furniture"),

            // 16.
            new RelicRegistration("M_BlueRug","Blue Rug", new[]{
                new RequirementConfig("JuteBlue",4),new RequirementConfig("DeerHide",4)
            },"A custom decorative piece.","furniture"),

            // 17.
            new RelicRegistration("M_Red_2m_semiround_rug","Red Semiround Rug", new[]{
                new RequirementConfig("JuteRed",4),new RequirementConfig("DeerHide",4)
            },"A custom decorative piece.","furniture"),

            // 18.
            new RelicRegistration("M_2m_semiround_rug","Brown Semiround Rug", new[]{
                new RequirementConfig("DeerHide",4)
            },"A custom decorative piece.","furniture"),

            // 19.
            new RelicRegistration("M_Jute_Round_Red_Rug","Round Red Jute Rug", new[]{
                new RequirementConfig("JuteRed",4),new RequirementConfig("DeerHide",4)
            },"A custom decorative piece.","furniture"),

                        // 20.
            new RelicRegistration("M_TrollRoundRug","Troll Hide Round Rug", new[]{
                new RequirementConfig("TrollHide",5),new RequirementConfig("DeerHide",5)
            },"A custom decorative piece.","furniture"),

                                    // 21.
            new RelicRegistration("M_TrollHideRug","Troll Hide Rug", new[]{
                new RequirementConfig("TrollHide",10),new RequirementConfig("DeerHide",5)
            },"A custom decorative piece.","furniture"),

                                                // 22.
            new RelicRegistration("M_TrollSemiRoundRug","Troll Hide Semi Round Rug", new[]{
                new RequirementConfig("TrollHide",2),new RequirementConfig("DeerHide",5)
            },"A custom decorative piece.","furniture"),

                                                            // 23.
            new RelicRegistration("M_TrollSemiRoundRugRuned","Troll Hide Runed Semi Round Rug", new[]{
                new RequirementConfig("TrollHide",2),new RequirementConfig("GreydwarfEye",2),
            },"A custom decorative piece.","furniture"),

                                                                        // 24.
            new RelicRegistration("M_TrollPeltRug","Troll Pelt Rug", new[]{
                new RequirementConfig("TrollHide",10), new RequirementConfig("TrophyForestTroll",1)
            },"A custom decorative piece.","furniture"),

            // 25.
            new RelicRegistration("M_Jute_Red_Simple_Rug","Simple Red Jute Rug", new[]{
                new RequirementConfig("JuteRed",2),new RequirementConfig("DeerHide",5)
            },"A custom decorative piece.","furniture"),

            // ---------------------- Porcelain / Tea Set ---------

            // 26.
            new RelicRegistration("M_AsianTeaSetPlate","Tea Plate", new[]{
                new RequirementConfig("Wood",1), new RequirementConfig("FineWood",1)
            },"A custom decorative piece.","clutter"),

            // 27.
            new RelicRegistration("M_bigplate","Large Porcelain Plate", new[]{
                new RequirementConfig("Stone",5), new RequirementConfig("Resin",2)
            },"A custom decorative piece.","clutter"),

            // 28.
            new RelicRegistration("M_cup1","Porcelain Tea Cup", new[]{
                new RequirementConfig("Stone",5), new RequirementConfig("Resin",2)
            },"A custom decorative piece.","clutter"),

            // 29.
            new RelicRegistration("M_justplate1","Large Porcelain Plate II", new[]{
                new RequirementConfig("Stone",5), new RequirementConfig("Resin",2)
            },"A custom decorative piece.","clutter"),

            // 30.
            new RelicRegistration("M_justcup1","Porcelain Tea Cup II", new[]{
                new RequirementConfig("Stone",5), new RequirementConfig("Resin",2)
            },"A custom decorative piece.","clutter"),

            // 31.
            new RelicRegistration("M_teapot","Porcelain Teapot", new[]{
                new RequirementConfig("Stone",5), new RequirementConfig("Resin",2)
            },"A custom decorative piece.","clutter"),

            // 32.
            new RelicRegistration("M_AsianTeaPot","Asian Teapot", new[]{
                new RequirementConfig("Stone",5), new RequirementConfig("Resin",2)
            },"A custom decorative piece.","clutter"),

            // 33.
            new RelicRegistration("M_AsianTeaCup","Asian Tea Cup", new[]{
                new RequirementConfig("Stone",5), new RequirementConfig("Resin",2)
            },"A custom decorative piece.","clutter"),

            // 34.
            new RelicRegistration("M_AsianTeaPotLid","Teapot Lid", new[]{
                new RequirementConfig("Stone",5), new RequirementConfig("Resin",2)
            },"A custom decorative piece.","clutter"),

            // 35.
            new RelicRegistration("M_ChineseVase","Chinese Vase", new[]{
                new RequirementConfig("Crystal",5), new RequirementConfig("Resin",2)
            },"A custom decorative piece.","clutter"),

            // 36.
            new RelicRegistration("M_ChineseTeaSet","Chinese Tea Set", new[]{
                new RequirementConfig("Stone",5), new RequirementConfig("Resin",2)
            },"A custom decorative piece.","clutter"),

            // ---------------------- Lamps / Bench / Bin ---------

            // 37.
            new RelicRegistration("M_Classic_Lamp","Classic Street Lamp", new[]{
                new RequirementConfig("Iron",2), new RequirementConfig("Wood",4), new RequirementConfig("Resin",4)
            },"","building"),

            // 38.
            new RelicRegistration("M_Japanese_Lamp","Japanese Street Lamp", new[]{
                new RequirementConfig("Iron",2), new RequirementConfig("Wood",4), new RequirementConfig("Resin",4)
            },"","building"),

            // 39.
            new RelicRegistration("M_Classic_Iron_Bench","Classic Iron Bench", new[]{
                new RequirementConfig("Iron",2), new RequirementConfig("Wood",4)
            },"Dvergr Forged Classic Iron Bench.","furniture"),

            // 40.
            new RelicRegistration("M_Classic_Double_Lamp","Classic Double Lamp", new[]{
                new RequirementConfig("Iron",2), new RequirementConfig("Wood",4), new RequirementConfig("Resin",4)
            },"Dvergr Forged Classic Iron 2x Lamp.","building"),

            // 41.
            new RelicRegistration("M_Classic_Double_Lamp_2","Classic Double Lamp II", new[]{
                new RequirementConfig("Iron",2), new RequirementConfig("Wood",4), new RequirementConfig("Resin",4)
            },"Dvergr Forged Classic Iron 2x Lamp.","building"),

            // 42.
            new RelicRegistration("M_classic_bin","Classic Garden Bin", new[]{
                new RequirementConfig("Iron",2), new RequirementConfig("Wood",4)
            },"Dvergr Forged Classic Iron Bin.","furniture"),

            // 43.
            new RelicRegistration("M_Classic_Single_Lamp","Classic Single Lamp", new[]{
                new RequirementConfig("Iron",2), new RequirementConfig("Wood",4), new RequirementConfig("Resin",4)
            },"Dvergr Forged Classic Iron 1x Lamp.","building"),

            // 44.
            new RelicRegistration("M_Chinese_Lamp","Chinese Lamp", new[]{
                new RequirementConfig("Iron",2), new RequirementConfig("Wood",4), new RequirementConfig("Resin",4)
            },"","building"),

            // 45.
            new RelicRegistration("M_classic_quad_light","Classic Quad Lamp", new[]{
                new RequirementConfig("Iron",2), new RequirementConfig("Wood",4), new RequirementConfig("Resin",4)
            },"Dvergr Forged Classic Iron 4x Lamp.","building"),

            // 46.
            new RelicRegistration("M_Classic_Wall_Lamp","Classic Wall Lamp", new[]{
                new RequirementConfig("Iron",2), new RequirementConfig("Wood",4), new RequirementConfig("Resin",4)
            },"Dvergr Forged Classic Iron Wall Lamp.","building"),

            // 47.
            new RelicRegistration("M_Classic_Wall_Lamp_2","Classic Wall Lamp II", new[]{
                new RequirementConfig("Iron",2), new RequirementConfig("Wood",4), new RequirementConfig("Resin",4)
            },"Dvergr Forged Classic Iron Wall Lamp.","building"),

            // ---------------------- Cannons ---------------------

            // 48.
            new RelicRegistration("M_Pirate_Ship_Cannon","Pirate Cannon", new[]{
                new RequirementConfig("Iron",10), new RequirementConfig("Wood",4), new RequirementConfig("Resin",4)
            },"","building"),

            // 49.
            new RelicRegistration("M_Pirate_Ship_Cannon_Balls","Cannonballs", new[]{
                new RequirementConfig("Iron",10), new RequirementConfig("Wood",4), new RequirementConfig("Resin",4)
            },"","building"),

            // 50.
            new RelicRegistration("M_M1857_12Pounder_Cannon_Cannonballs","12-Pounder Cannonballs", new[]{
                new RequirementConfig("Iron",10), new RequirementConfig("Wood",4), new RequirementConfig("Resin",4)
            },"","building"),

            // 51.
            new RelicRegistration("M_M1857_12Pounder_Cannon","12-Pounder Cannon", new[]{
                new RequirementConfig("Iron",10), new RequirementConfig("Wood",4), new RequirementConfig("Resin",4)
            },"","building"),

            // 52.
            new RelicRegistration("M_BookShelf","Ancient Root Bookshelf", new[]{
                new RequirementConfig("FineWood",20), new RequirementConfig("RoundLog",5), new RequirementConfig("Resin",4), new RequirementConfig("ElderBark",5)
            },"","building"),

            
            // 53.
            new RelicRegistration("M_VikingBed","Viking Chieftain’s Bed", new[]{
                new RequirementConfig("FineWood",20), new RequirementConfig("WolfPelt",5), new RequirementConfig("LoxPelt",5), new RequirementConfig("IronNails",20)
            },"","building"),
            
            // 53.
            new RelicRegistration("M_Dreamkeel","Moonforged Dreamkeel", new[]{
                new RequirementConfig("FineWood",5), new RequirementConfig("WolfPelt",1), new RequirementConfig("LoxPelt",2), new RequirementConfig("IronNails",4)
            },"","building"),

            // 54.
            new RelicRegistration("M_LeafChair","Moonforged Leaf Chair", new[]{
                new RequirementConfig("FineWood",5), new RequirementConfig("Dandelion",10), new RequirementConfig("Wood",5)
            },"","building"),

            // 55.
            new RelicRegistration("M_Viking_Greathall_Table","Moonforged Viking Greathall Table", new[]{
                new RequirementConfig("FineWood",20), new RequirementConfig("RoundLog",10), new RequirementConfig("Wood",25),new RequirementConfig("IronNails",10)
            },"","building"),

            // 56.
            new RelicRegistration("M_GargoyleLamp","Moonforged Gargoyle Wall Lamp", new[]{
                new RequirementConfig("Iron",2), new RequirementConfig("Resin",10), new RequirementConfig("GreydwarfEye",20)
            },"","building"),


            // 57.
            new RelicRegistration("M_DragonLamp","Moonforged Dragon Wall Lamp", new[]{
                new RequirementConfig("Silver",10), new RequirementConfig("Resin",10), new RequirementConfig("GreydwarfEye",20)
            },"","building"),

            // 58.
            new RelicRegistration("M_CrowLamp","Moonforged Raven Wall Lamp", new[]{
                new RequirementConfig("BlackMetal",2), new RequirementConfig("Resin",10), new RequirementConfig("GreydwarfEye",20)
            },"","building")


        };

        public static void InitConfig(ConfigFile cfg)
        {
            CustomHammerTabConfig = RelicConfigManager.AddEntry(
                cfg, "Categories", "CustomHammerTab",
                "",
                "Optional Hammer tab override for all Moonforged Lights and Decor pieces. Enter any custom tab name here. Leave empty to keep the category settings below."
            );

            FurnitureCategoryConfig = RelicConfigManager.AddEntry(
                cfg, "Categories", "FurnitureCategory",
                "Moonforged Lights and Decor",
                "Hammer tab name for all Moonforged furniture pieces."
            );

            BuildingCategoryConfig = RelicConfigManager.AddEntry(
                cfg, "Categories", "BuildingCategory",
                "Moonforged Lights and Decor",
                "Hammer tab name for all Moonforged building pieces."
            );

            ClutterCategoryConfig = RelicConfigManager.AddEntry(
                cfg, "Categories", "ClutterCategory",
                "Moonforged Lights and Decor",
                "Hammer tab name for all Moonforged clutter pieces."
            );

            StatuesCategoryConfig = RelicConfigManager.AddEntry(
                cfg, "Categories", "StatuesCategory",
                "Moonforged Lights and Decor",
                "Hammer tab name for all Moonforged statue pieces."
            );
        }

        public static IEnumerable<string> GetAllCategories() =>
            AllRegistrations.Select(r => CategoryToTab(r.Category)).Distinct();

        private static string CategoryToTab(string category)
        {
            if (CustomHammerTabConfig != null && !string.IsNullOrWhiteSpace(CustomHammerTabConfig.Value))
                return CustomHammerTabConfig.Value.Trim();

            return category.ToLower() switch
            {
                "statue" => StatuesCategoryConfig != null ? StatuesCategoryConfig.Value : "Moonforged Statues",
                "clutter" => ClutterCategoryConfig != null ? ClutterCategoryConfig.Value : "Moonforged Clutter",
                "building" => BuildingCategoryConfig != null ? BuildingCategoryConfig.Value : "Moonforged Building",
                "furniture" => FurnitureCategoryConfig != null ? FurnitureCategoryConfig.Value : "Moonforged Furniture",
                _ => category
            };
        }

        public static void RegisterAllRelics(AssetBundle bundle)
        {
            if (wasAlreadyRegistered) return;
            foreach (var reg in AllRegistrations)
                RegisterRelic(bundle, reg);
            wasAlreadyRegistered = true;
        }

        private static void RegisterRelic(AssetBundle bundle, RelicRegistration reg)
        {
            if (bundle == null) return;
            GameObject prefab = bundle.LoadAsset<GameObject>(reg.PrefabName);
            if (prefab == null)
            {
                Debug.LogWarning($"[Moonforged Lights and Decor] Missing prefab in AssetBundle: {reg.PrefabName}");
                return;
            }

            prefab.name = reg.PrefabName;

            var znv = prefab.GetComponent<ZNetView>() ?? prefab.AddComponent<ZNetView>();
            znv.m_persistent = true;
            znv.m_syncInitialScale = true;

            if (!prefab.GetComponent<ZSyncTransform>())
                prefab.AddComponent<ZSyncTransform>();

            Piece piece = prefab.GetComponent<Piece>() ?? prefab.AddComponent<Piece>();
            piece.m_name = reg.DisplayName;
            piece.m_description = reg.Description;
            piece.m_groundOnly = false;

            GameObject vfxPlace = null, sfxPlace = null, destroyVFX = null, destroySFX = null;

            string n = reg.PrefabName.ToLowerInvariant();

            bool isWindow = false;
            if (reg.DisplayName != null && reg.DisplayName.ToLower().Contains("window"))
                isWindow = true;
            if (reg.PrefabName.ToLower().Contains("mozaic"))
                isWindow = true;

            HashSet<string> ceramic = new()
            {
                "M_teapot","M_cup1","M_justcup1","M_justplate1",
                "M_bigplate","M_AsianTeaPot","M_AsianTeaCup",
                "M_AsianTeaPotLid","M_ChineseVase"
            };

            bool isCeramic = ceramic.Contains(reg.PrefabName);
            bool isRug = n.Contains("rug") || n.Contains("carpet");
            bool isLamp = n.Contains("lamp") || n.Contains("lantern") || n.Contains("light");
            bool isCannon = n.Contains("cannon");
            bool isBenchOrBin = n.Contains("bench") || n.Contains("bin");

            if (isCeramic)
            {
                vfxPlace = ZNetScene.instance?.GetPrefab("vfx_Place_crystal");
                sfxPlace = ZNetScene.instance?.GetPrefab("sfx_build_hammer_crystal");
                destroySFX = ZNetScene.instance?.GetPrefab("sfx_clay_pot_break");
            }
            else if (isRug)
            {
                vfxPlace = ZNetScene.instance?.GetPrefab("vfx_Place_wood");
                sfxPlace = ZNetScene.instance?.GetPrefab("sfx_build_hammer_wood");
                destroyVFX = ZNetScene.instance?.GetPrefab("vfx_destroyed");
                destroySFX = ZNetScene.instance?.GetPrefab("sfx_wood_break");
            }
            else if (isLamp || isCannon || isBenchOrBin)
            {
                vfxPlace = ZNetScene.instance?.GetPrefab("vfx_Place_stone");
                sfxPlace = ZNetScene.instance?.GetPrefab("sfx_build_hammer_metal");
                destroyVFX = ZNetScene.instance?.GetPrefab("vfx_destroyed");
                destroySFX = ZNetScene.instance?.GetPrefab("sfx_metal_blocked");
            }
            else if (isWindow)
            {
                vfxPlace = ZNetScene.instance?.GetPrefab("vfx_Place_crystal");
                sfxPlace = ZNetScene.instance?.GetPrefab("sfx_build_hammer_crystal");
                destroySFX = ZNetScene.instance?.GetPrefab("sfx_clay_pot_break");
            }
            else
            {
                vfxPlace = ZNetScene.instance?.GetPrefab("vfx_Place_wood");
                sfxPlace = ZNetScene.instance?.GetPrefab("sfx_build_hammer_wood");
                destroyVFX = ZNetScene.instance?.GetPrefab("vfx_destroyed");
                destroySFX = ZNetScene.instance?.GetPrefab("sfx_wood_break");
            }

            var placeFX = new EffectList();
            var placeList = new List<EffectList.EffectData>();
            if (vfxPlace != null) placeList.Add(new EffectList.EffectData { m_prefab = vfxPlace });
            if (sfxPlace != null) placeList.Add(new EffectList.EffectData { m_prefab = sfxPlace });
            placeFX.m_effectPrefabs = placeList.ToArray();
            piece.m_placeEffect = placeFX;

            WearNTear wear = prefab.GetComponent<WearNTear>() ?? prefab.AddComponent<WearNTear>();
            wear.m_health = 1000f;
            wear.m_noRoofWear = true;

            var destroyFX = new EffectList();
            var destroyList = new List<EffectList.EffectData>();
            if (destroyVFX != null) destroyList.Add(new EffectList.EffectData { m_prefab = destroyVFX });
            if (destroySFX != null) destroyList.Add(new EffectList.EffectData { m_prefab = destroySFX });
            destroyFX.m_effectPrefabs = destroyList.ToArray();
            wear.m_destroyedEffect = destroyFX;

            if (reg.Comfort > 0)
                piece.m_comfort = reg.Comfort;

            Sprite icon = bundle.LoadAsset<Sprite>(reg.PrefabName);
            if (icon != null)
                piece.m_icon = icon;

            if (isLamp && prefab.GetComponent<LampColorSwitcher>() == null)
                prefab.AddComponent<LampColorSwitcher>();

            if (isWindow)
            {
                foreach (Renderer r in prefab.GetComponentsInChildren<Renderer>(true))
                {
                    if (!r) continue;
                    r.shadowCastingMode = ShadowCastingMode.On;
                    r.receiveShadows = true;
                }
            }

            bool requiresIron = reg.Requirements.Any(r => r.Item.ToLower() == "iron");
            string craftingStation = requiresIron ? "forge" : "piece_workbench";

            var config = new PieceConfig
            {
                PieceTable = "Hammer",
                Category = CategoryToTab(reg.Category),
                CraftingStation = craftingStation,
                Requirements = reg.Requirements
            };

            PieceManager.Instance.AddPiece(new CustomPiece(prefab, false, config));
        }
    }
}
