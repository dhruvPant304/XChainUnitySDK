using System;
using System.IO;
using UnityEngine;

namespace Core.App
{
    public class SavedConfigData<T> where T : new()
    {
        private static readonly string SavedName = typeof(T).ToString();
        private static string DirectoryPath => Application.dataPath + "/Resources/XChain";
        private static string FilePath => $"{DirectoryPath}/{SavedName}.json";

        public void SaveData()
        {
            string json = JsonUtility.ToJson(this);
            if (!Directory.Exists(DirectoryPath)) Directory.CreateDirectory(DirectoryPath);
            File.WriteAllText(FilePath, json);
        }

        public static T LoadData()
        {
            try
            {
                string fileName = $"XChain/{SavedName}";
                TextAsset jsonFile = Resources.Load<TextAsset>(fileName);
                if (jsonFile == null)
                {
                    throw new FileNotFoundException("Resource file not found.");
                }
                string json = jsonFile.text;
                Debug.Log($"Loaded config data: {json}");
                return JsonUtility.FromJson<T>(json);
            }
            catch (Exception e)
            {
                Debug.Log("Error while loading saved config data: " + e.Message);
                return new T();
            }
        }
    }
}

