using System;
using Core.App;
using UnityEditor;
using UnityEngine;

namespace XChainSDK.XChain.Scripts.Editor
{
    public class XChainSettingsWindow : EditorWindow
    {
        [MenuItem("XChain/Configs")]
        public static void ShowWindow()
        {
            _window = GetWindow<XChainSettingsWindow>("Configs");
            _window.position = new Rect(100, 100, 400, 600);
        }

        private AppConfig _appConfig;
        private string _configFileName;
        private bool _saved;
        private static XChainSettingsWindow _window;

        private void OnEnable()
        {
            _appConfig = AppConfig.LoadData();
        }

        private void OnGUI()
        {
            if (_appConfig == null) _appConfig = new AppConfig();
            DrawHeader1("XChain Config");
            DrawHeader2("Network Config");
            _appConfig.networkSettings.serverUrl = EditorGUILayout.TextField("Server URL", _appConfig.networkSettings.serverUrl);
            _appConfig.networkSettings.xChainAuthUrl = EditorGUILayout.TextField("X Chain URL", _appConfig.networkSettings.xChainAuthUrl);
            DrawHeader2("Game Config");
            _appConfig.gameSettings.gameID = EditorGUILayout.TextField("Game ID", _appConfig.gameSettings.gameID);
            DrawHeader2("Web3Auth Config");
            DrawHeader2("Debug Options");
            _appConfig.networkSettings.debugSettings.logParams = EditorGUILayout.Toggle("Log request params", _appConfig.networkSettings.debugSettings.logParams);
            _appConfig.networkSettings.debugSettings.logResponses = EditorGUILayout.Toggle("Log request response", _appConfig.networkSettings.debugSettings.logResponses);

            if (GetButton("Save"))
            {
                try
                {
                    _appConfig.SaveData();
                    _saved = true;
                }
                catch (Exception e)
                {
                    Debug.LogError($"Unable to save Application config: {e.Message}");
                    throw;
                }
            }
            if (_saved)
            {
                DrawHeader2("Saved!");
                Debug.Log("App config data saved successfully!");
                _saved = false;
            }
        }

        private void DrawHeader1(string headerText)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontStyle = FontStyle.Bold;
            style.alignment = TextAnchor.MiddleLeft;
            style.fontSize = 20;

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            XChainGUILayout.DrawLogo();
            EditorGUILayout.LabelField(headerText, style, GUILayout.Height(50));
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);
        }
        private void DrawHeader2(string headerText)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontStyle = FontStyle.Bold;
            style.alignment = TextAnchor.MiddleLeft;

            GUILayout.Space(10);
            EditorGUILayout.LabelField(headerText, style, GUILayout.Height(25));
        }

        private bool GetButton(string label)
        {
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.fixedWidth = 100;
            
            GUILayout.Space(20);
            return GUILayout.Button(label, style);
        }
    }
}
