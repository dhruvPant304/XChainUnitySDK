using Features.Communication.Enums;
using Features.Communication.Singletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuPage : MonoBehaviour
{
    [SerializeField] private Button startGame;
    [SerializeField] private Button completeGame;
    private SceneReferencesController _sceneReferencesController;


    void Start()
    {
        if (_sceneReferencesController == null)
            _sceneReferencesController = FindObjectOfType<SceneReferencesController>();
        startGame.onClick.AddListener(OnGameStartClicked);
        completeGame.onClick.AddListener(OnCompleteGameClicked);
        _sceneReferencesController.SignupPage.SetActive(false);
    }

    private void OnGameStartClicked()
    {
        XChain.StartGame();
        XChain.SubscribeToEvent(XChainEvents.StartGameSuccess, (context) =>
        {
            Debug.Log($"Game Started Successfully {context.GameContext.SessionID}");
            StartCoroutine(TurnOnCompleteButton());
        });
        XChain.SubscribeToEvent(XChainEvents.StartGameFailed, (context) =>
        {
            Debug.Log("Start Game Failed!");
        });
    }

    IEnumerator TurnOnCompleteButton()
    {
        yield return new WaitForSeconds(5f);
        completeGame.gameObject.SetActive(true);
        startGame.gameObject.SetActive(false);
    }

    private void OnCompleteGameClicked()
    {
        XChain.CompleteGame();
        XChain.SubscribeToEvent(XChainEvents.CompleteGameSuccess, (context) =>
        {
            Debug.Log("Complete Game Success");
        });
        XChain.SubscribeToEvent(XChainEvents.CompleteGameFailed, (context) =>
        {
            Debug.Log("Complete Game Failed");
        });
    }
}
