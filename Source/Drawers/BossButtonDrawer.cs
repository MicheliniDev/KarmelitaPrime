using System;
using BepInEx.Configuration;
using UnityEngine;

namespace KarmelitaPrime.Drawers;

public static class BossButtonDrawer
{
    public static event Action OnButtonPressed;
    private static GUIStyle buttonStyle;
    public static void DrawButton(ConfigEntryBase entry)
    {
        GUILayout.Space(10f);
        GUILayout.BeginHorizontal();
        
        buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 40;
        buttonStyle.font = KarmelitaPrimeMain.Instance.MenuFont;
        buttonStyle.padding = new RectOffset(12, 10, 30, 10);
        
        if (GUILayout.Button("Fight Karmelita Prime", buttonStyle, GUILayout.Height(80f)))
        {
            OnButtonPressed?.Invoke();
        }

        GUILayout.EndHorizontal();
        GUILayout.Space(20f);
    }
}