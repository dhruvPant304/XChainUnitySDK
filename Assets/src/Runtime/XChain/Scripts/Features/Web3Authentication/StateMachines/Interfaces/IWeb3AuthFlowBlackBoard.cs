namespace Features.Web3Authentication.StateMachines.Interfaces
{
    public interface IWeb3AuthFlowBlackBoard
    {
        public string WalletAddress { get; set; }
        //public Provider LoginProvider { get; set; }
        //public Web3AuthResponse Web3AuthResponse { get; set; }
        public string AppPublicKey { get; set; }
    }
}