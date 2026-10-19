using UnityEngine;

namespace Moonforged.LightsAndDecor
{
    public class LampColorSwitcher : MonoBehaviour, Interactable, Hoverable
    {
        private int index = 0;
        private int customColorPacked = 0;
        private const int OFF_INDEX = -1;
        private const int CUSTOM_INDEX = 4;

        private Light[] lights;
        private Renderer[] allRenderers;
        private Renderer[] bulbRenderers;   // ONLY the bulb meshes

        private LightFlicker[] flickers;
        private LightLod[] lods;
        private ParticleSystem[] particles;
        private Transform[] flareObjects;

        private ZNetView znv;

        // ---- COLOR CYCLE ----
        private readonly Color[] colors = new Color[]
        {
            new Color(1.00f, 0.82f, 0.28f),   // Yellow
            new Color(0.22f, 0.85f, 0.32f),   // Green
            new Color(0.28f, 0.48f, 0.95f),   // Blue
            new Color(0.95f, 0.30f, 0.70f)    // Dvergr Pink
        };

        private readonly string[] names = new string[]
        {
            "Yellow", "Green", "Blue", "Dvergr Pink"
        };

        private const string ZDO_KEY = "LampColorIndex";
        private const string ZDO_CUSTOM_COLOR = "LampCustomColor";
        private const string ZDO_INIT = "initialized";

        void Awake()
        {
            lights = GetComponentsInChildren<Light>(true);
            allRenderers = GetComponentsInChildren<Renderer>(true);

            // ONLY grab renderers that contain the bulb material
            bulbRenderers = System.Array.FindAll(allRenderers, r =>
            {
                foreach (var m in r.sharedMaterials)
                {
                    if (m != null && m.name.StartsWith("M_Bulb"))
                        return true;
                }
                return false;
            });

            flickers = GetComponentsInChildren<LightFlicker>(true);
            lods = GetComponentsInChildren<LightLod>(true);
            particles = GetComponentsInChildren<ParticleSystem>(true);
            flareObjects = System.Array.FindAll(GetComponentsInChildren<Transform>(true), t =>
                t != transform && string.Equals(t.name, "flare", System.StringComparison.OrdinalIgnoreCase));
            if (RelicConfigManager.EnableFlare != null)
                RelicConfigManager.EnableFlare.SettingChanged += OnFlareSettingChanged;
            ApplyFlareState();
            if (RelicConfigManager.BulbBrightness != null)
                RelicConfigManager.BulbBrightness.SettingChanged += OnBulbBrightnessChanged;

            Collider col = GetComponentInChildren<Collider>();
            if (col == null)
            {
                var c = gameObject.AddComponent<CapsuleCollider>();
                c.isTrigger = true;
                c.radius = 0.25f;
                c.height = 1.2f;
            }

            znv = GetComponent<ZNetView>();
            if (!znv) return;

            znv.Register<int, int>("SetLampState", RPC_SetLampState);
            znv.Register<int>("RequestLampCycle", RPC_RequestLampCycle);

            var zdo = znv.GetZDO();
            if (zdo == null) return;

            if (znv.IsOwner())
            {
                if (!zdo.GetBool(ZDO_INIT))
                {
                    index = Random.Range(0, colors.Length);
                    customColorPacked = PackColor(RelicConfigManager.GetCustomLampColor());
                    zdo.Set(ZDO_KEY, index);
                    zdo.Set(ZDO_CUSTOM_COLOR, customColorPacked);
                    zdo.Set(ZDO_INIT, true);
                }
            }

            index = NormalizeIndex(zdo.GetInt(ZDO_KEY, index));
            customColorPacked = zdo.GetInt(ZDO_CUSTOM_COLOR, PackColor(RelicConfigManager.GetCustomLampColor()));

            ApplyState();
        }

        public bool Interact(Humanoid user, bool hold, bool alt)
        {
            if (hold) return false;
            if (!znv) return false;

            var zdo = znv.GetZDO();
            if (zdo == null) return false;

            int requestedCustomColor = PackColor(RelicConfigManager.GetCustomLampColor());

            if (znv.IsOwner())
                CycleLamp(requestedCustomColor);
            else
                znv.InvokeRPC("RequestLampCycle", requestedCustomColor);

            return true;
        }

        public bool Interact(Humanoid user, bool hold) =>
            Interact(user, hold, false);

        public bool UseItem(Humanoid user, ItemDrop.ItemData item) => false;

        private void RPC_RequestLampCycle(long sender, int requestedCustomColor)
        {
            if (!znv || !znv.IsOwner()) return;
            CycleLamp(requestedCustomColor);
        }

        private void CycleLamp(int requestedCustomColor)
        {
            var zdo = znv.GetZDO();
            if (zdo == null) return;

            if (index == OFF_INDEX)
            {
                index = 0;
            }
            else if (index < CUSTOM_INDEX)
            {
                index++;
            }
            else
            {
                index = OFF_INDEX;
            }

            if (index == CUSTOM_INDEX)
                customColorPacked = requestedCustomColor;

            zdo.Set(ZDO_KEY, index);
            zdo.Set(ZDO_CUSTOM_COLOR, customColorPacked);
            znv.InvokeRPC(ZNetView.Everybody, "SetLampState", index, customColorPacked);
            ApplyState();
        }

        private void RPC_SetLampState(long sender, int newState, int newCustomColor)
        {
            index = NormalizeIndex(newState);
            customColorPacked = newCustomColor;
            ApplyState();
        }

        private int NormalizeIndex(int value)
        {
            if (value == OFF_INDEX) return OFF_INDEX;
            if (value < 0 || value > CUSTOM_INDEX) return 0;
            return value;
        }

        private void ApplyState()
        {
            index = NormalizeIndex(index);

            if (index == OFF_INDEX)
            {
                // lights OFF
                foreach (var l in lights)
                    if (l) l.enabled = false;

                foreach (var f in flickers)
                    if (f) f.enabled = false;

                foreach (var ld in lods)
                    if (ld) ld.enabled = false;

                foreach (var p in particles)
                {
                    if (!p) continue;
                    p.Stop(true);
                    p.gameObject.SetActive(false);
                }

                // only bulbs get emission control
                foreach (var r in bulbRenderers)
                {
                    foreach (var m in r.materials)
                    {
                        if (m.HasProperty("_EmissionColor"))
                        {
                            m.DisableKeyword("_EMISSION");
                            m.SetColor("_EmissionColor", Color.black);
                        }
                    }
                }

                return;
            }

            // ON: set light color
            Color c = index == CUSTOM_INDEX ? UnpackColor(customColorPacked) : colors[index];

            foreach (var l in lights)
            {
                if (!l) continue;
                l.enabled = true;
                l.color = c;
            }

            foreach (var f in flickers)
                if (f) f.enabled = true;

            foreach (var ld in lods)
                if (ld) ld.enabled = true;

            foreach (var p in particles)
            {
                if (!p) continue;
                p.gameObject.SetActive(true);
                p.Play();
            }

            // ONLY affect bulb materials
            foreach (var r in bulbRenderers)
            {
                foreach (var m in r.materials)
                {
                    if (m == null || !m.name.StartsWith("M_Bulb")) continue;
                    TintBulbMaterial(m, c);
                    if (m.HasProperty("_EmissionColor"))
                    {
                        m.EnableKeyword("_EMISSION");
                        m.SetColor("_EmissionColor", c * GetBulbBrightness());
                    }
                }
            }

            ApplyFlareState();
        }

        public string GetHoverText()
        {
            index = NormalizeIndex(index);

            if (index == OFF_INDEX)
                return "[E] Turn On";

            string colorName = index == CUSTOM_INDEX ? "Custom" : names[index];
            return $"[E] Change color ({colorName})";
        }

        private static int PackColor(Color color)
        {
            Color32 c = color;
            return (c.r << 16) | (c.g << 8) | c.b;
        }

        private static Color UnpackColor(int packed)
        {
            byte r = (byte)((packed >> 16) & 0xFF);
            byte g = (byte)((packed >> 8) & 0xFF);
            byte b = (byte)(packed & 0xFF);
            return new Color32(r, g, b, 255);
        }

        public string GetHoverName() => "Lamp";
        public float GetHoverOffset() => 0f;

        private void OnFlareSettingChanged(object sender, System.EventArgs e)
        {
            ApplyFlareState();
        }

        private void ApplyFlareState()
        {
            if (flareObjects == null) return;
            bool enabled = index != OFF_INDEX &&
                (RelicConfigManager.EnableFlare == null || RelicConfigManager.EnableFlare.Value);
            foreach (var flare in flareObjects)
            {
                if (!flare) continue;
                if (!enabled)
                {
                    foreach (var particle in flare.GetComponentsInChildren<ParticleSystem>(true))
                        if (particle) particle.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
                }
                flare.gameObject.SetActive(enabled);
                if (enabled)
                {
                    foreach (var particle in flare.GetComponentsInChildren<ParticleSystem>(true))
                        if (particle && particle.gameObject.activeInHierarchy && !particle.isPlaying) particle.Play(false);
                }
            }
        }

        private static float GetBulbBrightness()
        {
            return RelicConfigManager.BulbBrightness != null ? RelicConfigManager.BulbBrightness.Value : 1.5f;
        }

        private static void TintBulbMaterial(Material material, Color color)
        {
            if (material.HasProperty("_Color"))
            {
                Color tint = color;
                tint.a = material.GetColor("_Color").a;
                material.SetColor("_Color", tint);
            }
            if (material.HasProperty("_BaseColor"))
            {
                Color tint = color;
                tint.a = material.GetColor("_BaseColor").a;
                material.SetColor("_BaseColor", tint);
            }
        }

        private void OnBulbBrightnessChanged(object sender, System.EventArgs e)
        {
            if (bulbRenderers == null) return;
            Color color = index == CUSTOM_INDEX ? UnpackColor(customColorPacked) : colors[NormalizeIndex(index) == OFF_INDEX ? 0 : NormalizeIndex(index)];
            foreach (var renderer in bulbRenderers)
            {
                if (!renderer) continue;
                foreach (var material in renderer.materials)
                {
                    if (material == null || !material.name.StartsWith("M_Bulb")) continue;
                    if (material.HasProperty("_EmissionColor"))
                        material.SetColor("_EmissionColor", index == OFF_INDEX ? Color.black : color * GetBulbBrightness());
                }
            }
        }

        private void OnDestroy()
        {
            if (RelicConfigManager.EnableFlare != null)
                RelicConfigManager.EnableFlare.SettingChanged -= OnFlareSettingChanged;
            if (RelicConfigManager.BulbBrightness != null)
                RelicConfigManager.BulbBrightness.SettingChanged -= OnBulbBrightnessChanged;
        }
    }
}