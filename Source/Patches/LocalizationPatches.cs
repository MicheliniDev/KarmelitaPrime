using HarmonyLib;
using TeamCherry.Localization;

namespace KarmelitaPrime.Patches;

[HarmonyPatch]
public class LocalizationPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Language), nameof(Language.Get), typeof(string), typeof(string))]
    private static void ChangeKarmelitaBossName(string key, string sheetTitle, ref string __result)
    {
        if (key == Constants.KarmelitaDisplayNameKey)
            __result = Constants.KarmelitaDisplayName;
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Language), nameof(Language.Get), typeof(string), typeof(string))]
    private static void ChangeKarmelitaBossDescription(string key, string sheetTitle, ref string __result)
    {
        if (key == Constants.KarmelitaDescriptionKey)
            __result = Constants.KarmelitaDescription;
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Language), nameof(Language.Get), typeof(string), typeof(string))]
    private static void ChangeKarmelitaBossTitleMain(string key, string sheetTitle, ref string __result)
    {
        if (key == Constants.KarmelitaBossTitleMainKey)
            __result = Constants.KarmelitaBossTitleMain;
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Language), nameof(Language.Get), typeof(string), typeof(string))]
    private static void ChangeKarmelitaBossTitleSuper(string key, string sheetTitle, ref string __result) {
        if (key == Constants.KarmelitaBossTitleSuperKey)
            __result = Constants.KarmelitaBossTitleSuper;
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Language), nameof(Language.Get), typeof(string), typeof(string))]
    private static void ChangeKarmelitaBossTitleSub(string key, string sheetTitle, ref string __result) {
        if (key == Constants.KarmelitaBossTitleSubKey)
            __result = Constants.KarmelitaBossTitleSub;
    }
}