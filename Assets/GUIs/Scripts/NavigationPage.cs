using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationPage : MonoBehaviour
{
    [SerializeField] private Button backButton;
    public Button BackButton => backButton;

    private void OnDisable()
    {
        BackButton.onClick.RemoveAllListeners();
    }
}
