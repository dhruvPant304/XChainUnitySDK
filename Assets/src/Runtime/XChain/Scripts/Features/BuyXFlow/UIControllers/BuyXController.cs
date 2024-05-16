using Core.API.APIParams;
using Core.API.APIResponse;
using Core.UI;
using Cysharp.Threading.Tasks;
using Features.Communication.Enums;
using Features.Communication.Singletons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Types;
using UnityEngine;
using UnityEngine.UI;

namespace Features.BuyXFlow.UIControllers
{
    public class BuyXController : View
    {
        [SerializeField] TMP_Dropdown networkDropdown;
        [SerializeField] TMP_Dropdown currencyDropdown;
        [SerializeField] TextMeshProUGUI currencyText;
        [SerializeField] TextMeshProUGUI balanceText;
        [SerializeField] TextMeshProUGUI conversionRateText;
        [SerializeField] TMP_InputField xTokenInputField;
        [SerializeField] TMP_InputField currencyInputField;

        private List<ExchangeNetwork> exchangeNetworks = new List<ExchangeNetwork>();
        private List<Currencies> currencies = new List<Currencies>();
        private Dictionary<string, ExchangePriceResponse> currencyRates = new Dictionary<string, ExchangePriceResponse>();

        private int _chainId;
        [SerializeField] string _accessToken;
        private string _accessKey = "283c238b82d0cfba454f9b01a7c205bd";
        private string _symbol;
        private string _tokenUUID;
        private float _xTokenAmount;
        private float _xTokenConversionRate;
        private string _exchangeRateInEth;
        private string _exchangeRateInUSDT = "200000000";
        

        private void Start()
        {
            XChain.OnEvent(XChainEvents.StartBuyXSuccess, async (context) => {
                xTokenInputField.text = "0";
                Init(context.BuyXContext.exchangeNetworks);
                await UpdateCurrency(0);
                await FetchXTokenRateAsync(_chainId);
                await FetchExchangeRateAsync(_chainId, _tokenUUID);
                CalculateCurrency(float.Parse(xTokenInputField.text));
                SetEventListeners();
                Show();
            });
        }

        private void SetEventListeners()
        {
            networkDropdown.onValueChanged.AddListener(async (i) => { await UpdateCurrency(i); });
            currencyDropdown.onValueChanged.AddListener(OnCurrencyDropdownChange);
            xTokenInputField.onValueChanged.AddListener(OnXTokenValueChanged);
            currencyInputField.onValueChanged.AddListener(OnCurrencyValueChanged);
        }

        private void OnXTokenValueChanged(string newValue)
        {
            if (!string.IsNullOrEmpty(newValue))
            {
                float xTokenValue = 0;
                if (float.TryParse(newValue, out xTokenValue))
                {
                    CalculateCurrency(xTokenValue);
                }
            }
        }

        private void OnCurrencyValueChanged(string newValue)
        {
            if (!string.IsNullOrEmpty(newValue))
            {
                float currencyValue = 0;
                if (float.TryParse(newValue, out currencyValue))
                {
                    CalculateXToken(currencyValue);
                }
            }
        }

        private void Init(List<ExchangeNetwork> networks)
        {
            exchangeNetworks.Clear();
            exchangeNetworks = networks;
            List<string> networkNames = networks.Select(network => network.name).ToList();
            networkDropdown.ClearOptions();
            networkNames.RemoveAt(networkNames.Count-1); //removing xToken from the list
            networkDropdown.AddOptions(networkNames);
        }

        private async UniTask UpdateCurrency(int selectedIndex) {
            ExchangeNetwork selectedNetwork = exchangeNetworks[selectedIndex];
            Debug.Log(selectedNetwork + " " + selectedIndex);
            _chainId = selectedNetwork.chainId;
            _tokenUUID = selectedNetwork.currencies[0].id;
            InitializeCurrencyDropDown(selectedNetwork.currencies.ToList());
            UpdateBalanceHeaderText(0);
            try {
                await FetchCurrencyRatesAsync(_chainId);
                await FetchExchangeRateAsync(_chainId, _tokenUUID);
                OnXTokenValueChanged(xTokenInputField.text);
            }
            catch {
                Hide();
            }
        }

        private void InitializeCurrencyDropDown(List<Currencies> networkCurrencies)
        {
            currencies.Clear();
            List<string> currencyNames = networkCurrencies.Select(currency => currency.name).ToList();
            currencies.AddRange(networkCurrencies);
            currencyDropdown.ClearOptions();
            currencyDropdown.AddOptions(currencyNames);
        }

        private async UniTask FetchCurrencyRatesAsync(int chainId)
        {
            var response = await XChain.Instance.APIService.GetPrice(chainId);
            if (response.IsSuccess)
            {
                currencyRates = response.SuccessResponse;
                Debug.Log(response.SuccessResponse);
            }
            else
            {
                throw new System.Exception("Failed to fetch currency rate");
            }
        }

        private async UniTask FetchExchangeRateAsync(int chainId, string tokenUUID) {
            var response = await XChain.Instance.APIService.GetExchangeRate(chainId, tokenUUID);
            if (response.IsSuccess) {
                _exchangeRateInEth = response.SuccessResponse.exchangeRate;
                Debug.Log(response.SuccessResponse);
            }
            else {
                throw new System.Exception("Failed to fetch currency rate");
            }
        }

        private async UniTask FetchXTokenRateAsync(int chainId) {
            var response = await XChain.Instance.APIService.GetXTokenPriceInUSDT(chainId);
            if (response.IsSuccess) {
                _xTokenConversionRate = response.SuccessResponse;
            }
            else {
                throw new System.Exception("Failed to fetch token rate");
            }
        }

        private void UpdateBalanceHeaderText(int selectedIndex)
        {
            Currencies selectedCurrency = currencies[selectedIndex];
            _tokenUUID = selectedCurrency.id;
            _symbol = currencies[selectedIndex].symbol;
            currencyText.text = $"Amount in <color=black><b>{currencies[selectedIndex].name}</b></color> you pay";
            UpdateBalance();
        }

        private void OnCurrencyDropdownChange(int selectedIndex)
        {
            UpdateBalanceHeaderText(selectedIndex);
            CalculateCurrency(float.Parse(xTokenInputField.text));
            CalculateXToken(float.Parse(currencyInputField.text));
        }

        private async void UpdateBalance()
        {
            var response = await XChain.Instance.APIService.GetBalance(_chainId, _tokenUUID, _accessToken);
            if (response.IsSuccess)
            {
                balanceText.text = $"Balance: <color=black><b>{response.SuccessResponse.balance} {_symbol}</b></color>";
                Debug.Log($"Balance: {response.SuccessResponse.balance}");
            }
            else
            {
                throw new System.Exception("Failed to fetch balance");
            }
        }

        private void CalculateCurrency(float xTokenAmount)
        {
            //input*xtokeninusd/currencyinUSD
            double currencyToUSDRate = currencyRates[_symbol].usd;
            decimal currencyAmount = (decimal)xTokenAmount * (decimal)_xTokenConversionRate / (decimal)currencyToUSDRate;
            decimal exchangeRateCurrencyAmount = (1 * (decimal)_xTokenConversionRate) / (decimal)currencyToUSDRate;
            currencyInputField.text = currencyAmount.ToString();
            conversionRateText.text = $"Exchange rate: 1 XToken = {exchangeRateCurrencyAmount.ToString("0.000000")} {_symbol}";
        }

        private void CalculateXToken(float currencyAmount)
        {
            //input*cuurencyinUSD/ xtokeninusd
            float currencyToUSDRate = currencyRates[_symbol].usd;
            float xTokenAmount = (currencyAmount * currencyToUSDRate) / _xTokenConversionRate;
            xTokenInputField.text = xTokenAmount.ToString();
        }

        public void BuyXToken()
        {
            SendBuyXTokenRequestAsync();
        }

        private async UniTask SendBuyXTokenRequestAsync()
        {
            var buyParams = new BuyRequestParams()
            {
                xTokenAmount = double.Parse(xTokenInputField.text),
                chainId = _chainId,
                exchangeRate = _exchangeRateInEth,
                currencyId = _tokenUUID
            };

            var response = await XChain.Instance.APIService.BuyToken(buyParams, _accessToken, _accessKey);
            if (response.IsSuccess)
            {
                Debug.Log(response.SuccessResponse);
            }
            else
            {
                Debug.Log($"BuyXToken Failed: {response.FailureResponse.message}");
            }
        }

        public void GetWalletAddress() {
            GetPlayerToken();
        }

        private async UniTask GetPlayerToken() {
            var response = await XChain.Instance.APIService.GetUserDetails(_accessToken);
            if (response.IsSuccess) {
                XChain.Instance.Context.Web3Context.PrivateKey = response.SuccessResponse.walletAddress;
                Debug.Log(response.SuccessResponse.walletAddress);
            }
            else {
                Debug.Log($"Get User Details Failed: {response.FailureResponse.message}");
            }
        }

        public void GetUserNFTDetails() {
            GetNFTDetails();
        }

        private async UniTask GetNFTDetails() {
            var response = await XChain.Instance.APIService.GetOwnedNFTDetails(_accessToken);
            if (response.IsSuccess) {
                Debug.Log(response.SuccessResponse);
            }
            else {
                Debug.Log($"Get User Details Failed: {response.FailureResponse.message}");
            }
        }

        public override void OnHide()
        {

        }

        public override void OnShow()
        {

        }
    }
}
