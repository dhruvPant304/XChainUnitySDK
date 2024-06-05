using XNode;
using XNodeStateMachine;
using Data;
using Features.Communication.Singletons;
using UnityEngine;
using Newtonsoft.Json;

namespace Features.XChainAuthentication.States {
    public class SaveLoginCredentials : State {
        [Output] public NodePort exit;
        protected override void Enter() {
            var loginCredentials = new LoginCredentialData();
            loginCredentials.accessToken = XChain.Instance.GetContext().SessionContext.AccessToken;
            loginCredentials.privateKey = XChain.Instance.GetContext().Web3Context.AccessKey;
            loginCredentials.userDetails = XChain.Instance.GetContext().Web3Context.UserData;
            var loginCredJson = JsonConvert.SerializeObject(loginCredentials);
            Debug.Log("cred: " + loginCredJson);
            PlayerPrefs.SetString("loginCredentials", loginCredJson);
            PlayerPrefs.Save();
            ExitThroughNodePort("exit");
        }

        protected override void Exit() {
        }

        protected override void UpdateState() {
        }
    }
}
