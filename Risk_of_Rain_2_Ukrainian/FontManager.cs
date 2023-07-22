using System;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using MonoMod.RuntimeDetour;
using TMPro;
using UnityEngine;

namespace Risk_of_Rain_2_Ukrainian;

public class FontManager
{
    public FontManager()
    {
        FontManager.fontBomb = FontManager.assetBundle.LoadAsset<TMP_FontAsset>("Assets/BOMBARD+Ukrainian.asset");
        FontManager.fontBombDefault = Resources.Load<TMP_FontAsset>("TmpFonts/Bombardier/tmpBombDropShadow");
        FontManager.fontDamageDefault = Resources.Load<GameObject>("Prefabs/Effects/BearProc").transform.Find("TextCamScaler/TextRiser/TextMeshPro").gameObject.GetComponent<TextMeshPro>().font;

        On.RoR2.UI.HGTextMeshProUGUI.OnCurrentLanguageChanged += new On.RoR2.UI.HGTextMeshProUGUI.hook_OnCurrentLanguageChanged(this.HGTextMeshProUGUI_OnCurrentLanguageChanged);
        new Hook(typeof(TextMeshProUGUI).GetMethod("LoadFontAsset", (BindingFlags)(-1)), new Action<Action<TextMeshProUGUI>, TextMeshProUGUI>(delegate (Action<TextMeshProUGUI> orig, TextMeshProUGUI self)
        {
            orig(self);
            bool flag = self.font == FontManager.fontBombDefault;
            if (flag)
                self.font = FontManager.fontBomb;
        }));
    }

    public void HGTextMeshProUGUI_OnCurrentLanguageChanged(On.RoR2.UI.HGTextMeshProUGUI.orig_OnCurrentLanguageChanged orig)
    {
        orig.Invoke();
        bool flag = FontManager.fontBomb;
        if (flag)
            RoR2.UI.HGTextMeshProUGUI.defaultLanguageFont = FontManager.fontBomb;
    }
    public static AssetBundle assetBundle = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("Risk_of_Rain_2_Ukrainian.ukrainianfont"));
    public static TMP_FontAsset fontBomb;
    public static TMP_FontAsset fontBombDefault;
    public static TMP_FontAsset fontDamage;
    public static TMP_FontAsset fontDamageDefault;
}