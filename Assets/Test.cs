using Features.Communication.Enums;
using Features.Communication.Singletons;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        XChain.StartGame();
        
        XChain.OnEvent(XChainEvents.LoginSuccess, (context) =>
        {
            Debug.Log("Logged In successfully");
            
            XChain.StartGame();
            XChain.SubscribeToEvent(XChainEvents.StartGameSuccess, (context) =>
            {
                Debug.Log($"Game Started Successfully {context.GameContext.SessionID}");
            });
            XChain.SubscribeToEvent(XChainEvents.StartGameFailed, (context) =>
            {
                Debug.Log("Start Game Failed!");
            });
            
            
            XChain.CompleteGame();
            XChain.SubscribeToEvent(XChainEvents.CompleteGameSuccess, (context) =>
            {
                Debug.Log("Complete Game Success");
            });
            XChain.SubscribeToEvent(XChainEvents.CompleteGameFailed, (context) =>
            {
                Debug.Log("Complete Game Failed");
            });
        });
    }
}
