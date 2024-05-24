using Features.Communication.Singletons;
using UnityEngine;

namespace Features.BuyXFlow.UIControllers
{
    public class XChainCanvas : Singleton<XChainCanvas>
    {
        [SerializeField] BuyXController buyXController;
        [SerializeField] BuyNFTController buyNFTController;

        public void OpenBuyXView(){
            if (buyXController == null) return;
            buyXController.Show();
        }

        public void HideBuyXView(){
            if (buyXController == null) return;
            buyXController.Hide();
        }

        public void OpenBuyNFTView(){
            if (buyNFTController == null) return;
            buyNFTController.Show();
        }

        public void HideBuyNFTView(){
            if (buyNFTController == null) return;
            buyNFTController.Hide();
        }

        protected override void Init(){
            buyXController.Init();
            buyXController.Hide();
        }


    }
}
