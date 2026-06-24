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
    public static TMP_FontAsset fontBombDefault;

    public FontManager()
    {
        fontBomb = assetBundle.LoadAsset<TMP_FontAsset>("Assets/BOMBARD+Ukrainian.asset");
        fontBombDefault = Resources.Load<TMP_FontAsset>("TmpFonts/Bombardier/tmpBombDropShadow");

        On.RoR2.UI.HGTextMeshProUGUI.OnCurrentLanguageChanged += HGTextMeshProUGUI_OnCurrentLanguageChanged;
        _ = new Hook(typeof(TextMeshProUGUI).GetMethod("LoadFontAsset", (BindingFlags)(-1)), TextMeshProUGUI_OnLoadFontAsset);
        _ = new Hook(typeof(TextMeshPro).GetMethod("LoadFontAsset", (BindingFlags)(-1)), TextMeshPro_OnLoadFontAsset);
    }

    public void TextMeshProUGUI_OnLoadFontAsset(Action<TextMeshProUGUI> orig, TextMeshProUGUI self)
    {
        orig(self);
        if (self.font == fontBombDefault)
            self.font = fontBomb;
    }

    public void TextMeshPro_OnLoadFontAsset(Action<TextMeshPro> orig, TextMeshPro self)
    {
        orig(self);
        if (self.font == fontBombDefault)
            self.font = fontBomb;
    }

    public void HGTextMeshProUGUI_OnCurrentLanguageChanged(On.RoR2.UI.HGTextMeshProUGUI.orig_OnCurrentLanguageChanged orig)
    {
        orig.Invoke();
        if (fontBomb)
            RoR2.UI.HGTextMeshProUGUI.defaultLanguageFont = fontBomb;
    }
}
