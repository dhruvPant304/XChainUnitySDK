using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LandingPage : MonoBehaviour
{
    [SerializeField] private Button loginButton;
    [SerializeField] private Button signupButton;
    private SceneReferencesController _sceneReferencesController;

    private void Awake()
    {
        if(_sceneReferencesController == null)
            _sceneReferencesController = FindObjectOfType<SceneReferencesController>();
        loginButton.onClick.AddListener(OnLoginClicked);
        signupButton.onClick.AddListener(OnLoginClicked);
    }

    private void OnEnable()
    {
        _sceneReferencesController.NavigationPage.SetActive(false);
    }

    private void OnLoginClicked()
    {
        _sceneReferencesController.SignupPage.SetActive(true);
        gameObject.SetActive(false);
    }
}
