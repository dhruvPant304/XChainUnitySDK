using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneReferencesController : MonoBehaviour
{
    [SerializeField] private GameObject landingPage;
    [SerializeField] private GameObject signupPage;
    [SerializeField] private GameObject navigationPage;
    [SerializeField] private GameObject waitForAuthPage;

    public GameObject LandingPage => landingPage;
    public GameObject SignupPage => signupPage;
    public GameObject NavigationPage => navigationPage;
    public GameObject WaitForAuthPage => waitForAuthPage;
    
}
