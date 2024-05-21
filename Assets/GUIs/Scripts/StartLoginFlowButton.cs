using Core.API;
using Data;
using Features.Communication.Enums;
using Features.Communication.Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartLoginFlowButton : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI privateKeyText;
    [SerializeField] private Button loginBtn;
    [SerializeField] private Button logoutBtn;
    [SerializeField] private Button buyXTokenBtn;
    [SerializeField] private Button getUserWalletAddress;
    [SerializeField] private Button getUserNFTs;
    [SerializeField] private GameObject loginPanel;


    public void Start() {
        string loginCredentials = PlayerPrefs.GetString("loginCredentials");
        Debug.Log(JsonUtility.FromJson<EventData>(loginCredentials));
        if (!string.IsNullOrEmpty(loginCredentials)) {
            Debug.Log("User is logged in.");
            var credentials = JsonUtility.FromJson<EventData>(loginCredentials);
            privateKeyText.text = ("Access Key: " + credentials.eventData.accessKey);
            loginBtn.gameObject.SetActive(false);
            logoutBtn.gameObject.SetActive(true);
            buyXTokenBtn.gameObject.SetActive(true);
        }
        else {
            Debug.Log("User is not logged in.");
        }
    }

    public void StartLogin() {
        XChain.StartAndWaitLoginCompletion();
        XChain.OnEvent(XChainEvents.LoginSuccess, (context) => {
            privateKeyText.text = ("Access Key: " + context.Web3Context.AccessKey);
            loginBtn.gameObject.SetActive(false);
            logoutBtn.gameObject.SetActive(true);
            buyXTokenBtn.gameObject.SetActive(true);
            getUserNFTs.gameObject.SetActive(true);
            getUserWalletAddress.gameObject.SetActive(true);
        });
    }

    public void Logout() {
        PlayerPrefs.DeleteKey("loginCredentials");
        PlayerPrefs.Save();
        Debug.Log("Deleting login credentials: " + PlayerPrefs.GetString("loginCredentials"));
        privateKeyText.text = "Please login!";
        loginBtn.gameObject.SetActive(true);
        logoutBtn.gameObject.SetActive(false);
        buyXTokenBtn.gameObject.SetActive(false);
        getUserNFTs.gameObject.SetActive(false);
        getUserWalletAddress.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BuyXToken() {
        XChain.StartBuyXFlow();
        XChain.OnEvent(XChainEvents.StartBuyXSuccess, (context) => {
            loginPanel.SetActive(false);
        });
    }
}
