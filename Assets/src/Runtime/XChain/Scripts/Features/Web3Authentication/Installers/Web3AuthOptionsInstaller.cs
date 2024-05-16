using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

/*[Serializable]
[CreateAssetMenu(fileName = "web3AuthOptionsInstaller", menuName = "Web3Auth/Installers/Web3AuthOptionsInstaller")]
public class Web3AuthOptionsInstaller : ScriptableObjectInstaller<Web3AuthOptionsInstaller>
{
    [SerializeField] private string redirectUrl;
    [SerializeField] private string sdkUrl = "https://sdk.openlogin.com";
    [ShowInInspector] public Dictionary<string, LoginConfigItem> loginConfig;

    [Header("White Label Data")]

    [SerializeField] private string labelName;
    [SerializeField] private string logoLight;
    [SerializeField] private string logoDark;
    [SerializeField] private Web3Auth.Language defaultLanguage = Web3Auth.Language.en;
    [SerializeField] private bool dark = false;
    [ShowInInspector] public Dictionary<string, string> theme;

    [Inject] Web3EnvSettings web3EnvSettings;

    public override void InstallBindings()
    {
        WhiteLabelData whiteLabelData = new WhiteLabelData()
        {
            //name = labelName,
            logoLight = logoLight,
            logoDark = logoDark,
            defaultLanguage = defaultLanguage,
            //dark = dark,
            theme = theme
        };
        Web3AuthOptions options = new Web3AuthOptions()
        {
            clientId = web3EnvSettings.clientID,
            network = (Web3Auth.Network)web3EnvSettings.network,
            redirectUrl = new Uri(redirectUrl),
            sdkUrl = sdkUrl,
            whiteLabel = whiteLabelData,
            loginConfig = loginConfig
        };


        Container.Bind<Web3AuthOptions>().FromInstance(options)
            .AsSingle()
            .NonLazy();

        Container.Bind<Web3Auth>().FromComponentInHierarchy()
            .AsSingle()
            .NonLazy();
    }
}*/