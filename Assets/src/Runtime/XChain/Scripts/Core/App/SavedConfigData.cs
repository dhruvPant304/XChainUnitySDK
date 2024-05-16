using System;
using System.IO;
using System.Net;
using UnityEngine;
//using Directory = UnityEngine.Windows.Directory;

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
            if (!File.Exists(FilePath))
            {
                if (!Directory.Exists(DirectoryPath)) Directory.CreateDirectory(DirectoryPath);
                File.Create(FilePath);
            }
            File.WriteAllText(FilePath,json);
        }

        public static T LoadData()
        {
            try
            {
                var json = File.ReadAllText(FilePath);
                return JsonUtility.FromJson<T>(json);
            }
            catch
            {
                return new T();
            }
        }
    }
}