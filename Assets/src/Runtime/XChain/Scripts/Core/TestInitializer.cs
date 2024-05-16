using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.API;
using UnityEngine;
using UnityEngine.Networking;

public class TestInitializer : MonoBehaviour
{
    [SerializeField] string token;
    private string _sessionID;
    private int _score = 0;

    public class Root
    {
        public bool status { get; set; }
    }

    public async void StartGame()
    {
        var url = "https://dev-api.greymango.techalchemy.dev/api/v1/1/start";
        var request = CreateRequest(url, "Post", token);

        var operation = request.SendWebRequest();
        while (!operation.isDone)
        {
            await Task.Yield();
        }
        if (request.result == UnityWebRequest.Result.Success)
        {
            StartGameRequestResponse sessionID = JsonConvert.DeserializeObject<StartGameRequestResponse>(request.downloadHandler.text);
            _sessionID = sessionID.sessionId;
            Debug.Log("sessionID: " + sessionID.sessionId);
        }
        else
        {
            Debug.Log($"Failed: {request.error} {request.downloadHandler.text}");
        }
    }

    public async void CompleteGame()
    {
        var url = $"https://dev-api.greymango.techalchemy.dev/api/v1/{_sessionID}/complete";
        var request = CreateRequest(url, "PATCH", token);
        var operation = request.SendWebRequest();
        while (!operation.isDone)
        {
            await Task.Yield();
        }
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(request.downloadHandler.text);
            //RequestResponse completeGame = JsonConvert.DeserializeObject<RequestResponse>(request.downloadHandler.text);
            //Debug.Log("completed: " + completeGame.status);
        }
        else
        {
            Debug.Log($"Failed: {request.error} {request.downloadHandler.text}");
        }
    }

    public async void UpdateCurrentScore()
    {
        var url = $"https://dev-api.greymango.techalchemy.dev/api/v1/{_sessionID}/score";
        _score = _score + 1;
        GetScoreRequestResponse getScoreRequest = new GetScoreRequestResponse
        {
            score = _score
        };
        string jsonData = JsonUtility.ToJson(getScoreRequest);
        var request = CreateRequest(url, "POST", token, getScoreRequest);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.SetRequestHeader("Content-Type", "application/json");
        var operation = request.SendWebRequest();
        while (!operation.isDone)
        {
            await Task.Yield();
        }
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(request.downloadHandler.text);
            Root root = JsonConvert.DeserializeObject<Root>(request.downloadHandler.text);
        }
        else
        {
            Debug.Log($"Failed: {request.error} {request.downloadHandler.text}");
        }
    }

    public async void GetCurrentScore()
    {
        var url = $"https://dev-api.greymango.techalchemy.dev/api/v1/{_sessionID}/score";
        var request = CreateRequest(url, "GET", token);
        var operation = request.SendWebRequest();
        while (!operation.isDone)
        {
            await Task.Yield();
        }
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(request.downloadHandler.text);
            GetScoreRequestResponse score = JsonConvert.DeserializeObject<GetScoreRequestResponse>(request.downloadHandler.text);
        }
        else
        {
            Debug.Log($"Failed: {request.error} {request.downloadHandler.text}");
        }
    }

    private UnityWebRequest CreateRequest(string uri, string method, string token = "", object data = null)
    {
        var request = new UnityWebRequest(uri);
        request.timeout = 20;
        request.method = method;
        Debug.Log(uri);
        request.SetRequestHeader("accept", "application/json");
        request.downloadHandler = new DownloadHandlerBuffer();

        request.disposeUploadHandlerOnDispose = true;
        request.disposeDownloadHandlerOnDispose = true;

        if (!string.IsNullOrEmpty(token)) request.SetRequestHeader("Authorization", $"Bearer {token}");
        return request;
    }
}
