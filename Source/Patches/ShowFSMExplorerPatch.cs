using HarmonyLib;
using HutongGames.PlayMaker;
using UnityExplorer.CacheObject;
using UnityExplorer.CacheObject.Views;
using UniverseLib;

namespace KarmelitaPrime.Source.Patches;

[HarmonyPatch]
public class ShowFSMExplorerPatch
{
    //Code yoinked from joeswanson in the HK modding server
    [HarmonyPrefix]
    [HarmonyPatch(typeof(CacheObjectBase), "SetValueState")]
    public static bool blehhh(CacheObjectBase __instance, CacheObjectCell cell, CacheObjectBase.ValueStateArgs args)
    {
        if (cell is not CacheListEntryCell listEntry)
                return true;

        void Convert(string name)
        {
            AccessTools.Property(typeof(CacheObjectBase), "ValueLabelText")
                    .SetValue(
                        __instance, 
                        UniverseLib.Utility.ToStringUtility.ToStringWithType(
                        __instance.Value, 
                        __instance.FallbackType, 
                        true
                    )
                + $" - <i><color=#b0edff>{name}</color></i>"
            );
        }

        switch (__instance.Value)
        {
            case FsmState fsm: Convert(fsm.Name); break;
            case FsmEvent fsm: Convert(fsm.Name); break;
            case FsmStateAction fsm: Convert(fsm.Name); break;
            case FsmVar fsm: Convert(fsm.NamedVar.Name); break;
        }
        return true;
    }
}