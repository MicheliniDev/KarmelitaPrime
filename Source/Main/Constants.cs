using System.Collections.Generic;
using InControl.NativeDeviceProfiles;
using TeamCherry.Localization;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KarmelitaPrime;

public static class Constants
{
    #region Localization
    public static string KarmelitaSceneName => "Memory_Ant_Queen";
    public static string KarmelitaDisplayNameKey => "NAME_HUNTER_QUEEN";
    public static string KarmelitaDescriptionKey => "DESC_HUNTER_QUEEN";
    public static string KarmelitaBossTitleMainKey => "HUNTER_QUEEN_BC_MAIN";
    public static string KarmelitaBossTitleSuperKey => "HUNTER_QUEEN_BC_SUPER";
    public static string KarmelitaBossTitleSubKey => "HUNTER_QUEEN_BC_SUB";
    
    public static string KarmelitaDisplayName
    {
        get
        {
            string value = "";
            if (KarmelitaPrimeMain.Instance.isWhatsapp.Value)
            {
                switch (Language.CurrentLanguage())
                {
                    case LanguageCode.EN:
                        value = "Whatsapp Karmelita";
                        break;
                    case LanguageCode.PT:
                        value = "Karmelita Whatsapp";
                        break;
                }
            }
            else
            {
                switch (Language.CurrentLanguage())
                {
                    case LanguageCode.EN:
                        value = "Karmelita Prime";
                        break;
                    case LanguageCode.PT:
                        value = "Karmelita Auge";
                        break;
                }
            }
            return value;
        }
    }
    
    public static string KarmelitaDescription
    {
        get
        {
            string value = "";
            switch (Language.CurrentLanguage())
            {
                case LanguageCode.EN:
                    value = $"The Skarr's most skilled warrior and singer, at the peak of her abilities.";
                    break;
                case LanguageCode.PT:
                    value = $"A guerreira e cantora mais habilidosa dos Skarr, no auge de suas habilidades.";
                    break;
            }
            return value;
        }
    }

    public static string KarmelitaBossTitleSuper
    {
        get
        {
            string value = "";
            if (KarmelitaPrimeMain.Instance.isWhatsapp.Value)
            {
                switch (Language.CurrentLanguage())
                {
                    case LanguageCode.EN:
                        value = "The Messenger";
                        break;
                    case LanguageCode.PT:
                        value = "Olha a mensagem";
                        break;
                }
            }
            else
            {
                switch (Language.CurrentLanguage())
                {
                    case LanguageCode.EN:
                        value = "The Hunter Queen";
                        break;
                    case LanguageCode.PT:
                        value = "A Caçadora Rainha";
                        break;
                }
            }
            return value;
        }
    }
    
    public static string KarmelitaBossTitleMain => KarmelitaDisplayName;
    
    public static string KarmelitaBossTitleSub
    {
        get
        {
            string value = "";
            switch (Language.CurrentLanguage())
            {
                case LanguageCode.EN:
                    value = "By MicheliniDev";
                    break;
                case LanguageCode.PT:
                    value = "Por MicheliniDev";
                    break;
            }
            return value;
        }
    }
    #endregion

    #region Health
    public static int KarmelitaMaxHp => 2500;
    public static float KarmelitaPhase2HpThreshold => KarmelitaMaxHp * 0.80f;
    public static float KarmelitaPhase2_5HpThreshold => KarmelitaMaxHp * 0.60f;
    public static float KarmelitaPhase3HpThreshold => KarmelitaMaxHp * 0.50f;
    #endregion

    #region Attack Speed Multipliers
    
    #region Slash Speed Multipliers
    public static float SlashAnticSpeed => 1.4f;
    public static float Slash1Speed => 1.7f;
    public static float Slash2Speed => 1.5f;
    public static float SlashEndSpeed => 1.25f;
    #endregion
    
    #region Spin Attack Speed Multipliers
    public static float SpinAttackAnticSpeed => 1.5f;
    public static float SpinAttackRecoilSpeed => 1.5f;
    #endregion
    
    #region Sickle Throw Speed Multipliers
    public static float ThrowSpeed => 1.5f;
    public static float ThrowAnticSpeed => 1.5f;
    public static float AirThrowSpeed => 1.5f;
    public static float AirRethrowSpeed => 1.5f;
    public static float RethrowAntic1Speed => 1.5f;
    public static float RethrowAntic2Speed => 1.5f;
    #endregion

    #region Jump Attack Speed Multipliers
    public static float LaunchAnticSpeed => 1.25f;
    public static float LaunchSpeed => 1.4f;
    public static float JumpAnticSpeed => 1.7f;
    public static float JumpSpeed => 1.2f;
    public static float JumpSpinAnticSpeed => 1.2f;
    public static float JumpSpinSpeed => 1.3f;
    public static float JumpAttackLandSpeed => 1.25f;
    #endregion
    
    #region Dash Grind Speed Multipliers
    public static float WallLandSpeed => 1.8f;
    public static float WallDiveSpeed => 1.5f;
    public static float DashGrindSpeed => 1.5f;
    public static float DashGrindSpinSpeed => 1f;
    #endregion
    
    #endregion
    
    #region Spawn Positions
    public static Vector3 PlayerSpawnPosition => new Vector3(181f, 21f, 0f);
    public static Vector3 BenchSpawnPosition => new Vector3(178f, 21f, 0.019f);
    #endregion

    #region Asset Bundles
    public static string[] BundleNames => [
        "localpoolprefabs_assets_areahangareasong",
    ];
    public static string[] AssetNames => [
        "Song Knight Projectile",
    ];
    #endregion
    
    public static readonly string[] StatesToCancelContactDamage =
    [
        //This array is way bigger than I expected it would be :(
        "Start Idle",
        "Movement 1",
        "Movement 2",
        "Movement 3",
        "Movement 4",
        "Movement 5",
        "Evade",
        "Approach",
        "Long Evade",
        "Long Approach",
        "Dash",
        "Stun Start",
        "Stun Air",
        "Stun Land",
        "Stunned",
        "Stun Damage",
        "Damage Recover",
        "Stun Recover",
        "Approach Block",
        "Approach Block Transitioner",
        "Slash Antic",
        "Jump Antic",
        "Spin Attack Land",
        "Throw Fall",
        "Throw Land",
        "Teleport 1 Pre",
        "Teleport 1",
        "Teleport 1 Recovery",
        "Teleport 2 Pre",
        "Teleport 2",
        "Teleport 2 Recovery",
        "Teleport 3 Pre",
        "Teleport 3",
        "Teleport 3 Recovery",
        "Teleport 4 Pre",
        "Teleport 4",
        "Teleport 4 Recovery",
        "Teleport 5 Pre",
        "Teleport 5",
        "Teleport 5 Recovery",
        "Teleport 6 Pre",
        "Teleport 6",
        "Teleport 6 Recovery",
        "Teleport 7 Pre",
        "Teleport 7",
        "Teleport 7 Recovery",
        "Generic Teleport Pre",
        "Generic Teleport",
        "Generic Teleport Recovery",
        "P2 Roar Antic",
        "Phase 3 Knocked",
        "Phase 3 Recovering State",
        "P2 Roar Antic",
        "P2 Roar",
        "P3 Roar Antic",
        "P3 Roar"
    ];

    public static Dictionary<string, float> AnimationSpeedCollection = new Dictionary<string, float>()
    {
        {"Slash Antic", SlashAnticSpeed}, 
        {"Slash 1", Slash1Speed}, 
        {"Slash 2", Slash2Speed},
        {"Slash End", SlashEndSpeed},
        {"Spin Attack Antic", SpinAttackAnticSpeed}, 
        {"Spin Attack Recoil", SpinAttackRecoilSpeed},
        {"Throw", ThrowSpeed}, 
        {"Throw Antic", ThrowAnticSpeed}, 
        {"Air Throw", AirThrowSpeed},
        {"Air Rethrow", AirRethrowSpeed},
        {"Rethrow Antic 1", RethrowAntic1Speed},
        {"Rethrow Antic 2", RethrowAntic2Speed},
        {"Launch Antic", LaunchAnticSpeed}, 
        {"Launch", LaunchSpeed},
        {"Jump Antic", JumpAnticSpeed},
        {"Jump", JumpSpeed}, 
        {"JumpSpin Antic", JumpSpinAnticSpeed}, 
        {"JumpSpin", JumpSpinSpeed}, 
        {"Jump Attack Land", JumpAttackLandSpeed},   
        {"Wall Land", WallLandSpeed},  
        {"Wall Dive", WallDiveSpeed},  
        {"Dash Grind", DashGrindSpeed},  
        {"Dash Grind Spin", DashGrindSpinSpeed},  
    };
    
    public static bool IsBlackWhiteHighlight => SceneManager.GetActiveScene().name == KarmelitaSceneName 
                                                && KarmelitaPrimeMain.Instance.wrapper &&
                                                KarmelitaPrimeMain.Instance.wrapper.IsInHighlightMode;
}