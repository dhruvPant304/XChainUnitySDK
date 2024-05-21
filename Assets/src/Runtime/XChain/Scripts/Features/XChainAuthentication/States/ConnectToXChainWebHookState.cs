using System;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Core.API.APIResponse;
using Features.Communication.Singletons;
using Newtonsoft.Json;
using UnityEngine;
using XNode;
using XNodeStateMachine;

namespace Features.XChainAuthentication.States
{
    public class ConnectToXChainWebHookState : State
    {
        [DllImport("user32.dll")] static extern uint GetActiveWindow();
        [DllImport("user32.dll")] static extern bool SetForegroundWindow(IntPtr hWnd);
        
        private HttpListener _listener;
        private readonly string _redirectUrl = "http://localhost:8080/";
        private string _gateWayUrl;
        [Output] public NodePort success;
        [Output] public NodePort failed;
        protected override void Enter()
        {
            _gateWayUrl = $"http://localhost:5173/?redirect={_redirectUrl}";
            Application.OpenURL(_gateWayUrl);
            OpenWebHook();
        }

        // Initializes and starts an HttpListener on "http://localhost:8080/" and begins asynchronous processing of incoming requests
        private void OpenWebHook()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://localhost:8080/");
            _listener.Start();
            _listener.BeginGetContext(OnRequest, null);
        }
        
        void OnRequest(IAsyncResult result)
        {
            try
            {
                var context = _listener.EndGetContext (result);

                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                context.Response.Headers.Add("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
                context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Accept");

                if (context.Request.HttpMethod == "OPTIONS")
                {
                    context.Response.StatusCode = 200;
                    context.Response.OutputStream.Close();
                    return;
                }

                Debug.Log ("Method: " + context.Request.HttpMethod);
                Debug.Log ("LocalUrl: " + context.Request.Url.LocalPath);
                if (context.Request.QueryString.AllKeys.Length > 0)
                    foreach (var key in context.Request.QueryString.AllKeys) {
                        Debug.Log ("Key: " + key + ", Value: " + context.Request.QueryString.GetValues (key) [0]);
                    }

                if (context.Request.HttpMethod == "POST") {	
                    Thread.Sleep (1000);
                    var data_text = new StreamReader (context.Request.InputStream, 
                        context.Request.ContentEncoding).ReadToEnd ();
                    Debug.Log (data_text);
                    var loginResponse = JsonConvert.DeserializeObject<LoginSuccessResponse>(data_text);
                    XChain.Instance.Context.SessionContext.AccessToken = loginResponse.accessToken;
                    XChain.Instance.Context.Web3Context.WalletAddress = loginResponse.user.walletAddress;
                    XChain.Instance.Context.Web3Context.AccessKey = loginResponse.privateKey;
                    
                    context.Response.StatusCode = 200;
                    string httpResponse = "{}";
                    byte[] buffer = Encoding.ASCII.GetBytes(httpResponse);
                    context.Response.OutputStream.Write(buffer);
                    ExitThroughNodePort("success");
                }
                context.Response.Close();
            }
            catch (Exception e)
            {
                Debug.Log(e);
                var gameWindow = GetActiveWindow();
                SetForegroundWindow((IntPtr)gameWindow);
                ExitThroughNodePort("failed");
            }
        }

        protected override void Exit()
        {
            _listener.Close();
        }

        protected override void UpdateState()
        {
            
        }
    }
}