using System;
using Cysharp.Threading.Tasks;
using Data;
using Features.Communication.Singletons;
using UnityEngine;
using UnityEngine.Networking;
using XNode;
using XNodeStateMachine;

namespace Features.XChainAuthentication.States {
    public class StartLoginAndWaitCompletion : State {

        WebViewObject webView;
        string keyName = "openlogin_store";

        string url = "https://dev-api.greymango.techalchemy.dev/";
        string userAgent = "Mozilla/5.0 (Linux; Android 10; Redmi Note 8 Pro) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.88 Mobile Safari/537.36";

        [Output] public NodePort failed;
        [Output] public NodePort success;
        [Output] public NodePort cancelled;


        protected override void Enter() {
            CheckForCancellation().Forget();
            StartLogin().Forget();
        }

        public async UniTask StartLogin() {
            webView = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
            webView.Init(ua: userAgent, cb: OnMessageRecieved);
            webView.SetVisibility(true);
            await LoadWebContent(url);
        }

        string receivedAuthToken;
        string receivedAccessKey;

        public async void OnMessageRecieved(string msg){
                var jsonObject = JsonUtility.FromJson<EventData>(msg);
                Debug.Log($"{jsonObject.eventData.accessToken} {jsonObject.eventData.accessKey}");
                receivedAuthToken= jsonObject.eventData.accessToken;
                receivedAccessKey = jsonObject.eventData.accessKey;
                webView.EvaluateJS($"localStorage.removeItem('{keyName}')");
                await FetchUserDetails();
        }

        public async UniTask CheckForCancellation(){
            await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Menu));
            Debug.Log("User cancelled login");
            ExitThroughNodePort("cancelled");
        }

        public async UniTask FetchUserDetails(){
            var response = await XChain.Instance.APIService.GetUserDetails(XChain.Instance.Context.SessionContext.AccessToken);
            if(response.IsSuccess){
                var userData = response.SuccessResponse;

                var result = await XChain.Instance.Login(accessToken: receivedAuthToken,
                    accessKey: receivedAccessKey,
                    userData: userData);

                if(result){
                    ExitThroughNodePort("success");
                    return;
                }
            }
            ExitThroughNodePort("failed");
        }

        protected override void Exit() {
            Debug.Log("Exitting webview");
            webView.SetVisibility(false);
            Destroy(webView.gameObject);
        }

        async UniTask LoadWebContent(string url) {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url)) {
                await webRequest.SendWebRequest();
                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    if(!TryLoadURL(url)) ExitThroughNodePort("failed");
                }
                else {
                    Debug.LogError("Error loading webUrl: " + webRequest.error);
                    ExitThroughNodePort("failed");
                }
            }
        }

        private bool TryLoadURL(string url) {
            try{
                webView.LoadURL(url);
                return true;
            }
            catch (Exception ex) {
                Debug.Log(ex);
                return false;
            }
        }

        protected override void UpdateState() {
        }
    }
}


