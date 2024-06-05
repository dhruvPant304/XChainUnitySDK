using System;
using Data;
using Features.Communication.Singletons;
using Newtonsoft.Json;
using UnityEngine;
using XNode;
using XNodeStateMachine;

namespace Features.XChainAuthentication.States {
    public class CheckSavedLoginCredentials : State {
        [Output] public NodePort yes;
        [Output] public NodePort no;

        protected override void Enter(){
            if(!PlayerPrefs.HasKey("loginCredentials") || string.IsNullOrWhiteSpace(PlayerPrefs.GetString("loginCredentials"))){
                ExitThroughNodePort("no");
                return;
            }

            Debug.Log($"Checking saved Login credentials");
            var loginCredJson = PlayerPrefs.GetString("loginCredentials");
            try{
                var loginCred = JsonConvert.DeserializeObject<LoginCredentialData>(loginCredJson);
                Debug.Log($"Fetched saved credentials as: {loginCred}");
                XChain.Instance.Context.SessionContext.AccessToken = loginCred.accessToken;
                XChain.Instance.Context.Web3Context.AccessKey = loginCred.privateKey;
                XChain.Instance.Context.Web3Context.UserData = loginCred.userDetails;
                ExitThroughNodePort("yes");
                return;
            }
            catch(Exception e){
                Debug.LogError($"unable to deserialize login Credentials {e}");
                ExitThroughNodePort("no");
                return;
            }
        }

        protected override void Exit(){
        }

        protected override void UpdateState(){
        }
    }
}
