using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class XChainGUILayout
{
    public static Texture2D LoadLogo()
    {
        return Resources.Load<Texture2D>("XChain/GUI/Logo");
    }

    public static void DrawLogo()
    {
        GUILayout.Label(LoadLogo(), GUILayout.Height(50), GUILayout.Width(50));
    }
}
