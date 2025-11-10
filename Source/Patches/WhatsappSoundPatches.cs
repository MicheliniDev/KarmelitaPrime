using HarmonyLib;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KarmelitaPrime.Patches;

[HarmonyPatch]
public class WhatsappSoundPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayAudioEvent), "SpawnAudioEvent")]
    private static void ChangeClipToWhatsapp(ref PlayAudioEvent __instance)
    {
        if (SceneManager.GetActiveScene().name == Constants.KarmelitaSceneName &&
            KarmelitaPrimeMain.Instance.isWhatsapp.Value && 
            __instance.Fsm.Owner.name.Contains("Hunter Queen Boss")
            && __instance.audioClip.Value.name.Contains("scream"))
        {
            __instance.audioClip.Value = KarmelitaPrimeMain.Instance.WhatsappSound;
        }
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(AudioPlayRandomVoiceFromTable), "DoPlayRandomClip")]
    private static bool ChangeVoiceClipToWhatsapp(ref AudioPlayRandomVoiceFromTable __instance)
    {
        if (SceneManager.GetActiveScene().name == Constants.KarmelitaSceneName &&
            KarmelitaPrimeMain.Instance.isWhatsapp.Value && 
            __instance.Fsm.Owner.name.Contains("Hunter Queen Boss") &&
            __instance.audioClipTable.name.Contains("Roar"))
        {
            AudioEvent audioEvent = new AudioEvent()
            {
                Clip = KarmelitaPrimeMain.Instance.WhatsappSound,
                PitchMin = 1f,
                PitchMax = 1f,
                Volume = FSMUtility.LocateFSM(KarmelitaPrimeMain.Instance.wrapper.gameObject, "Control").
                    Fsm.GetFsmGameObject("Audio Loop Voice").Value.
                    GetComponent<AudioSource>().volume + 1.35f
            };
            audioEvent.SpawnAndPlayOneShot(__instance.Owner.transform.position);
            return false;
        }
        return true;
    }
}