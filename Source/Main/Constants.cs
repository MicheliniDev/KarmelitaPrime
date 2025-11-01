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
    public static string KarmelitaNoteKey => "NOTE_HUNTER_QUEEN";
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
                    case LanguageCode.DE:
                        value = "Whatsapp Karmelita";
                        break;
                    case LanguageCode.ES:
                        value = "Karmelita Whatsapp";
                        break;
                    case LanguageCode.FR:
                        value = "Karmelita Whatsapp";
                        break;
                    case LanguageCode.IT:
                        value = "Karmelita Whatsapp";
                        break;
                    case LanguageCode.JA:
                        value = "Whatsapp カルメリタ";
                        break;
                    case LanguageCode.KO:
                        value = "Whatsapp 카르멜리타";
                        break;
                    case LanguageCode.RU:
                        value = "Whatsapp Кармелита";
                        break;
                    case LanguageCode.ZH:
                        value = "Whatsapp 卡梅莉塔";
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
                    case LanguageCode.DE:
                        value = "Karmelita in Höchstform";
                        break;
                    case LanguageCode.ES:
                        value = "Karmelita en su Apogeo";
                        break;
                    case LanguageCode.FR:
                        value = "Karmelita à son Apogée";
                        break;
                    case LanguageCode.IT:
                        value = "Karmelita all'Apice";
                        break;
                    case LanguageCode.JA:
                        value = "カルメリタ・プライム";
                        break;
                    case LanguageCode.KO:
                        value = "카르멜리타 프라임";
                        break;
                    case LanguageCode.RU:
                        value = "Кармелита Прайм";
                        break;
                    case LanguageCode.ZH:
                        value = "卡梅莉塔 巅峰";
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
                    value = @"The Hunter Queen at her vibrant zenith.";
                    break;
                case LanguageCode.PT:
                    value = @"A Rainha Caçadora em seu zênite vibrante.";
                    break;
                case LanguageCode.DE:
                    value = @"Die Jägerkönigin auf ihrem pulsierenden Zenit.";
                    break;
                case LanguageCode.ES:
                    value = @"La Reina Cazadora en su vibrante cénit.";
                    break;
                case LanguageCode.FR:
                    value = @"La Reine-chasseresse à son zénith éclatant.";
                    break;
                case LanguageCode.IT:
                    value = @"La Regina Cacciatrice al suo vibrante zenith.";
                    break;
                case LanguageCode.JA:
                    value = @"狩人の女王、その輝かしい絶頂にて。";
                    break;
                case LanguageCode.KO:
                    value = @"사냥꾼 여왕이 찬란한 정점에 섰습니다.";
                    break;
                case LanguageCode.RU:
                    value = @"Королева-Охотница в своем блистательном зените.";
                    break;
                case LanguageCode.ZH:
                    value = @"猎手女王正值其辉煌的巅峰。";
                    break;
            }
            return value;
        }
    }

    public static string KarmelitaNote
    {
        get
        {
            string value = "";
            switch (Language.CurrentLanguage())
            {
                case LanguageCode.EN:
                    value = @"She is different from what I once envisioned. Her form, a vibrant pink unknown to her kin, marks the apex of her power. Her delicate determination honed to a point of pure, warlike aggression.";
                    break;
                case LanguageCode.PT:
                    value = @"Ela é diferente do que eu imaginei outrora. Sua forma, um rosa vibrante desconhecido para sua espécie, marca o ápice de seu poder. Sua delicada determinação foi afiada a um ponto de pura agressão bélica.";
                    break;
                case LanguageCode.DE:
                    value = @"Sie ist anders, als ich es mir einst vorgestellt hatte. Ihre Gestalt, ein leuchtendes Pink, das ihrem Volk unbekannt ist, markiert den Gipfel ihrer Macht. Ihre zarte Entschlossenheit, geschärft zu einem Punkt purer, kriegerischer Aggression und Entschlossenheit.";
                    break;
                case LanguageCode.ES:
                    value = @"Es diferente de lo que una vez imaginé. Su forma, de un rosa vibrante desconocido para su estirpe, marca el ápice de su poder. Su delicada determinación, pulida hasta un punto de pura agresividad bélica y determinación.";
                    break;
                case LanguageCode.FR:
                    value = @"Elle est différente de ce que j'avais imaginé. Sa forme, d'un rose vif inconnu des siens, marque l'apogée de sa puissance. Sa détermination délicate, affûtée jusqu'à devenir pure agression guerrière et détermination.";
                    break;
                case LanguageCode.IT:
                    value = @"È diversa da come l'avevo immaginata. Le sue fattezze, di un rosa vibrante sconosciuto alla sua stirpe, segnano l'apice del suo potere. La sua delicata determinazione, affinata fino a un punto di pura aggressione bellica e determinazione.";
                    break;
                case LanguageCode.JA:
                    value = @"彼女は、私がかつて思い描いた姿とは異なる。その姿は、同族には見られぬ鮮やかな桃色。力の頂点にあることの証。その繊細な決意は、純粋な戦意、そして決意の域まで研ぎ澄まされている。";
                    break;
                case LanguageCode.KO:
                    value = @"제가 한때 상상했던 모습과는 다릅니다. 그녀의 모습은 동족에게는 없는 선명한 분홍빛을 띠며, 힘의 정점을 상징합니다. 그녀의 섬세한 결의는 순수한 호전적 공격성, 그리고 결의로 날카롭게 벼려져 있습니다.";
                    break;
                case LanguageCode.RU:
                    value = @"Она отличается от той, что я некогда представляла. Ее облик, ярко-розовый, неведомый ее сородичам, знаменует собой вершину ее силы. Ее хрупкая решимость отточена до состояния чистой, воинственной агрессии и решимости.";
                    break;
                case LanguageCode.ZH:
                    value = @"她与我曾经想象的样子截然不同。她的形态，一种族人所没有的鲜艳粉色，标志着她力量的顶点。她那微妙的决心被磨砺到了极致，化作纯粹的、好战的侵略性与决心。";
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
                    case LanguageCode.DE:
                        value = "Die Botin";
                        break;
                    case LanguageCode.ES:
                        value = "La Mensajera";
                        break;
                    case LanguageCode.FR:
                        value = "La Messagère";
                        break;
                    case LanguageCode.IT:
                        value = "La Messaggera";
                        break;
                    case LanguageCode.JA:
                        value = "使者";
                        break;
                    case LanguageCode.KO:
                        value = "전령";
                        break;
                    case LanguageCode.RU:
                        value = "Посланница";
                        break;
                    case LanguageCode.ZH:
                        value = "信使";
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
                    case LanguageCode.DE:
                        value = "Die Jägerkönigin";
                        break;
                    case LanguageCode.ES:
                        value = "La Reina Cazadora";
                        break;
                    case LanguageCode.FR:
                        value = "La Reine Chasseuse";
                        break;
                    case LanguageCode.IT:
                        value = "La Regina Cacciatrice";
                        break;
                    case LanguageCode.JA:
                        value = "狩人の女王";
                        break;
                    case LanguageCode.KO:
                        value = "사냥꾼의 여왕";
                        break;
                    case LanguageCode.RU:
                        value = "Королева-охотница";
                        break;
                    case LanguageCode.ZH:
                        value = "猎人女王";
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
                case LanguageCode.DE:
                    value = "Von MicheliniDev";
                    break;
                case LanguageCode.ES:
                    value = "Por MicheliniDev";
                    break;
                case LanguageCode.FR:
                    value = "Par MicheliniDev";
                    break;
                case LanguageCode.IT:
                    value = "Di MicheliniDev";
                    break;
                case LanguageCode.JA:
                    value = "MicheliniDev作";
                    break;
                case LanguageCode.KO:
                    value = "제작: MicheliniDev";
                    break;
                case LanguageCode.RU:
                    value = "От MicheliniDev";
                    break;
                case LanguageCode.ZH:
                    value = "作者：MicheliniDev";
                    break;
            }
            return value;
        }
    }
    #endregion

    #region Health

    public static int KarmelitaMaxHp => KarmelitaPrimeMain.Instance.isWhatsapp.Value ? 2501 : 2500;
    public static float KarmelitaPhase2HpThreshold => KarmelitaMaxHp * 0.80f;
    public static float KarmelitaPhase2_5HpThreshold => KarmelitaMaxHp * 0.60f;
    public static float KarmelitaPhase3HpThreshold => KarmelitaMaxHp * 0.40f;
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
        "Launch Antic",
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
        "Phase 3 Knocked",
        "Phase 3 Recovering State",
        "P2 Roar Antic",
        "P2 Roar",
        "Fake Phase 3",
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