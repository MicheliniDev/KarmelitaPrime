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
    public static int KarmelitaMaxHp => 2000;
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
    
    public static bool IsBlackWhiteHighlight => SceneManager.GetActiveScene().name == KarmelitaSceneName 
                                                && KarmelitaPrimeMain.Instance.wrapper &&
                                                KarmelitaPrimeMain.Instance.wrapper.IsInHighlightMode;
}