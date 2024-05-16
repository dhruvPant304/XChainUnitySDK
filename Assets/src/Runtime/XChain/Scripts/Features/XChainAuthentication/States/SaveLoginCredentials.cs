using XNode;
using XNodeStateMachine;
using Data;
using Features.Communication.Singletons;
using UnityEngine;

namespace Features.XChainAuthentication.States {
    public class SaveLoginCredentials : State {
        [Output] public NodePort exit;
        protected override void Enter() {
            var loginCredentials = new EventData();
            loginCredentials.eventData = new EventDataData();
            loginCredentials.eventData.accessToken = XChain.Instance.GetContext().SessionContext.AccessToken;
            loginCredentials.eventData.accessKey = XChain.Instance.GetContext().Web3Context.PrivateKey;
            var loginCredJson = JsonUtility.ToJson(loginCredentials);
            Debug.Log("cred: " + loginCredJson);
            PlayerPrefs.SetString("loginCredentials", loginCredJson.ToString());
            PlayerPrefs.Save();
            ExitThroughNodePort("exit");
        }

        protected override void Exit() {
        }

        protected override void UpdateState() {
        }
    }
}
