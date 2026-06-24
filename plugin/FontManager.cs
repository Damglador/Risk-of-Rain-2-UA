using System;
using System.Reflection;
using MonoMod.RuntimeDetour;
using TMPro;
using UnityEngine;

namespace Risk_of_Rain_2_Ukrainian;

public class FontManager
{
    public static AssetBundle assetBundle = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("Risk_of_Rain_2_Ukrainian.ukrainianfont"));
    public static TMP_FontAsset fontBomb;
    public static Material dropshadowMaterial;
    public static Material hologramMaterial;
    public static TMP_FontAsset fontBombDefault;

    public FontManager()
    {
        fontBomb = assetBundle.LoadAsset<TMP_FontAsset>("Assets/BOMBARD+Ukrainian.asset");
        dropshadowMaterial = assetBundle.LoadAsset<Material>("Assets/BOMBARD+Ukrainian.dropshadow.mat");
        hologramMaterial = assetBundle.LoadAsset<Material>("Assets/BOMBARD+Ukrainian.hologram.mat");

        fontBombDefault = Resources.Load<TMP_FontAsset>("TmpFonts/Bombardier/tmpBombDropShadow");

        On.RoR2.UI.HGTextMeshProUGUI.OnCurrentLanguageChanged += HGTextMeshProUGUI_OnCurrentLanguageChanged;
        _ = new Hook(typeof(TextMeshProUGUI).GetMethod("LoadFontAsset", (BindingFlags)(-1)), OnLoadFontAsset);
        _ = new Hook(typeof(TextMeshPro).GetMethod("LoadFontAsset", (BindingFlags)(-1)), OnLoadFontAsset);
    }

    public void OnLoadFontAsset(Action<TMP_Text> orig, TMP_Text self)
    {
        orig(self);
        if (self.font == fontBombDefault)
        {
            self.font = fontBomb;
            if (self is TextMeshProUGUI)
                self.fontMaterial = dropshadowMaterial;
            if (self is TextMeshPro)
                self.fontMaterial = hologramMaterial;
        }
    }

    public void HGTextMeshProUGUI_OnCurrentLanguageChanged(On.RoR2.UI.HGTextMeshProUGUI.orig_OnCurrentLanguageChanged orig)
    {
        orig.Invoke();
        if (fontBomb)
            RoR2.UI.HGTextMeshProUGUI.defaultLanguageFont = fontBomb;
    }
}
