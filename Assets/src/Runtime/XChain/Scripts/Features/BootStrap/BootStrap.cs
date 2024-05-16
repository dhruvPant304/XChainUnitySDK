using UnityEngine;
using UnityEngine.SceneManagement;

namespace XChainSDK.XChain.Scripts.Features.BootStrap
{
    public class BootStrap : MonoBehaviour
    {
        void Start()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
