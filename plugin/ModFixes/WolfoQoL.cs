using System.Collections.Generic;
using R2API;
using RoR2;

namespace Risk_of_Rain_2_Ukrainian.ModFixes;

public class WolfoQoL
{
    public WolfoQoL()
    {
        On.RoR2.Language.SetCurrentLanguage += UpdateBoostedNameTokens;
    }

    public static void UpdateBoostedNameTokens(On.RoR2.Language.orig_SetCurrentLanguage orig, string newLanguage)
    {
        orig(newLanguage);
        Dictionary<string, Gender> chefTokens = new();
        chefTokens.Add("CHEF_PRIMARY_NAME", Gender.Neu);
        chefTokens.Add("CHEF_SECONDARY_NAME", Gender.Neu);
        chefTokens.Add("CHEF_SECONDARY_ALT_NAME", Gender.Fem);
        chefTokens.Add("CHEF_UTILITY_NAME", Gender.Neu);
        chefTokens.Add("CHEF_UTILITY_ALT_NAME", Gender.Default);

        foreach (var token in chefTokens)
        {
            var formatToken = "CHEF_BOOSTED_FORMAT";
            var text = GrammaticalGenderManager.GetStringFormatted(
                formatToken, token.Value, Language.GetString(token.Key));
            LanguageAPI.AddOverlay(token.Key + "_B", text);
        }

        Dictionary<string, Gender> fiendTokens = new();
        fiendTokens.Add("VOIDSURVIVOR_PRIMARY_NAME", Gender.Neu);
        fiendTokens.Add("VOIDSURVIVOR_SECONDARY_NAME", Gender.Fem);
        fiendTokens.Add("VOIDSURVIVOR_UTILITY_NAME", Gender.Default);
        fiendTokens.Add("VOIDSURVIVOR_SPECIAL_NAME", Gender.Neu);

        foreach (var token in fiendTokens)
        {
            var formatToken = "VOIDFIEND_BOOSTED_FORMAT";
            var text = GrammaticalGenderManager.GetStringFormatted(
                formatToken, token.Value, Language.GetString(token.Key));
            LanguageAPI.AddOverlay(token.Key + "_B", text);
        }
    }
}
