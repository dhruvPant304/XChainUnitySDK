using System;
using Data;
using Features.Communication.Singletons;
using UnityEngine;
using XNode;
using XNodeStateMachine;

namespace Features.XChainAuthentication.States {
    public class CheckSaveLoginCredentials : State {
        [Output] public NodePort yes;
        [Output] public NodePort no;

        protected override void Enter() {
            if (!PlayerPrefs.HasKey("loginCredentials") || string.IsNullOrWhiteSpace(PlayerPrefs.GetString("loginCredentials"))) {
                Debug.Log("exitting through no");
                ExitThroughNodePort("no");
                return;
            }

            var loginCredJson = PlayerPrefs.GetString("loginCredentials");
            try {
                var loginCred = JsonUtility.FromJson<EventData>(loginCredJson);
                XChain.Instance.Context.SessionContext.AccessToken = loginCred.eventData.accessToken;
                XChain.Instance.Context.Web3Context.AccessKey = loginCred.eventData.accessKey;
                ExitThroughNodePort("yes");
                return;
            }
            catch (Exception e) {
                Debug.LogError($"unable to deserialize login Credentials {e}");
                ExitThroughNodePort("no");
                return;
            }
        }

        protected override void Exit() {
        }

        protected override void UpdateState() {
        }
    }
}
