using Features.Communication.Singletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetBuyXNetwork : MonoBehaviour
{
    public async void StartBuyXFlow()
    {
        XChain.StartBuyXFlow();
        gameObject.SetActive(false);

    }
}
