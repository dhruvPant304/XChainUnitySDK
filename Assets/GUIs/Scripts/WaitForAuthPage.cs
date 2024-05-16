using Features.Communication.Enums;
using Features.Communication.Singletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitForAuthPage : MonoBehaviour
{
    private SceneReferencesController _sceneReferencesController;


    void Awake()
    {
        if (_sceneReferencesController == null)
            _sceneReferencesController = FindObjectOfType<SceneReferencesController>();
    }

    private void OnEnable()
    {
        Button backButton = _sceneReferencesController.NavigationPage.GetComponent<NavigationPage>()?.BackButton;
        backButton.onClick.AddListener(OnBackButtonClicked);
        _sceneReferencesController.NavigationPage.SetActive(true);
    }

    private void OnBackButtonClicked()
    {
        XChain.CancelOperation();
        _sceneReferencesController.SignupPage.SetActive(true);
        _sceneReferencesController.WaitForAuthPage.SetActive(false);
    }


    void Update()
    {
        XChain.OnEvent(XChainEvents.LoginSuccess, (context) =>
        {
            Debug.Log("Logged In successfully");
            _sceneReferencesController.WaitForAuthPage.SetActive(false);
            _sceneReferencesController.NavigationPage.SetActive(false);
        });
    }
}
