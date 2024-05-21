using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T> {
    private static T _instance;
    public static T Instance { 
        get{
            if(_instance == null) {
                GameObject obj = new GameObject(typeof(T).Name);
                _instance = obj.AddComponent<T>();
            }
            return _instance;
        } 
    }

    private void Awake() {
            if (_instance != null && _instance != this) {
                Debug.LogWarning($"{typeof(T).Name} Instance already Exist in scene");
                Destroy(gameObject);
                return;
            }
            _instance = (T)this;
            DontDestroyOnLoad(gameObject);
            Init();
    }

    protected abstract void Init();
}
