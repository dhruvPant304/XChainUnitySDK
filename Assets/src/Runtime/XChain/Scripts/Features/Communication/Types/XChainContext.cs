using Assets.Scripts.Features.Communication.Enums;

namespace Features.Communication.Types
{
    public struct XChainContext
    {
        public XChainGameContext GameContext;
        public XChainSessionContext SessionContext;
        public Web3Context Web3Context;
        public BuyXContext BuyXContext;
    }
}