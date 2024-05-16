namespace Features.Communication.Enums
{
    public enum XChainEvents
    {
        StartLogin = 1,
        WaitingForAuthentication = 2,
        LoginCancelled = 3,
        LoginSuccess = 4,
        LoginFailed = 5,
        
        StartGame = 6,
        StartGameFailed = 7,
        StartGameSuccess = 8,
        
        CompleteGame = 9,
        CompleteGameFailed = 10,
        CompleteGameSuccess = 11,

        UpdateScore = 12,
        UpdateScoreFailed = 13,
        UpdateScoreSuccess = 14,
        
        DirectLogin = 15,

        StartAndWaitLoginCompletion = 16,

        StartBuyXFlow = 17,
        StartBuyXSuccess = 18,
        StartBuyXFailed = 19,

        CancelOperation = 100
    }
}