using Core.API.APIParams;
using Core.API.APIResponse;
using Core.UI;
using Cysharp.Threading.Tasks;
using Features.Communication.Enums;
using Features.Communication.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Types;
using UnityEngine;

namespace Features.BuyXFlow.UIControllers
{
    public class BuyXController : View
    {
        [SerializeField] TMP_Dropdown networkDropdown;
        [SerializeField] TMP_Dropdown currencyDropdown;
        [SerializeField] TextMeshProUGUI currencyText;
        [SerializeField] TextMeshProUGUI balanceText;
        [SerializeField] TextMeshProUGUI xTokenBalanceText;
        [SerializeField] TextMeshProUGUI conversionRateText;
        [SerializeField] TMP_InputField xTokenInputField;
        [SerializeField] TMP_InputField currencyInputField;

        private List<ExchangeNetwork> exchangeNetworks = new List<ExchangeNetwork>();
        private List<Currency> currencies = new List<Currency>();
        private Dictionary<string, ExchangePriceResponse> currencyRates = new Dictionary<string, ExchangePriceResponse>();


        [SerializeField] string accessToken;
        [SerializeField] string accessKey = "283c238b82d0cfba454f9b01a7c205bd";

        private int _chainId;
        private string _symbol;
        private string _tokenUUID;
        private float _xTokenAmount;
        private float _xTokenConversionRate;
        private string _exchangeRateInEth;
        private string _exchangeRateInUSDT = "200000000";

        private string _accessToken {
            get {
                #if UNITY_EDITOR
                return accessToken;
                #endif
                return XChain.Instance.Context.SessionContext.AccessToken;
            }
        }

        private string _accessKey {
            get {
                #if UNITY_EDITOR
                return accessKey;
                #endif
                return XChain.Instance.Context.Web3Context.AccessKey;
            }
        }

        public async void OnEnable() {
            await Initialize();
         }

        private bool _initialized;
        private async UniTask Initialize(){
            if(_initialized) return;
            xTokenInputField.text = "0";
            InitNetworks(XChain.Instance.Context.BuyXContext.exchangeNetworks);
            await UpdateCurrency(0);
            await FetchXTokenRateAsync(_chainId);
            await FetchExchangeRateAsync(_chainId, _tokenUUID);
            CalculateCurrency(float.Parse(xTokenInputField.text));
            SetEventListeners();

            //fetching XToken Balance on initialization to update balance text
            await XChain.FetchXTokenBalance();
            _initialized = true;
        }

        private void SetEventListeners()
        {
            networkDropdown.onValueChanged.AddListener(async (i) => { await UpdateCurrency(i); });
            currencyDropdown.onValueChanged.AddListener(OnCurrencyDropdownChange);
            xTokenInputField.onValueChanged.AddListener(OnXTokenValueChanged);
            currencyInputField.onValueChanged.AddListener(OnCurrencyValueChanged);
            XChain.Instance.OnXTokenBalanceChanged += OnXTokenBalanceUpdate;
        }

        private void OnXTokenValueChanged(string newValue) {
            if (string.IsNullOrEmpty(newValue)) return;    
            if (!float.TryParse(newValue, out float xTokenValue)) return;
            CalculateCurrency(xTokenValue);
        }

        private void OnCurrencyValueChanged(string newValue) {
            if (string.IsNullOrEmpty(newValue)) return;
            if (!float.TryParse(newValue, out float currencyValue)) return;
            CalculateXToken(currencyValue);
        }

        private void InitNetworks(List<ExchangeNetwork> networks)
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
            catch(Exception e) {
                Debug.Log("Unable to update currency: " + e.Message);
                Hide();
            }
        }

        private void InitializeCurrencyDropDown(List<Currency> networkCurrencies)
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
            if (response.IsSuccess){
                currencyRates = response.SuccessResponse;
                Debug.Log(response.SuccessResponse);
                return;
            }
            throw new Exception($"Failed to fetch currency rate: {response.FailureResponse.message}");
        }

        private async UniTask FetchExchangeRateAsync(int chainId, string tokenUUID) {
            var response = await XChain.Instance.APIService.GetExchangeRate(chainId, tokenUUID);
            if (response.IsSuccess) {
                _exchangeRateInEth = response.SuccessResponse.exchangeRate;
                Debug.Log(response.SuccessResponse);
                return;
            }
            throw new Exception($"Failed to fetch currency rate: {response.FailureResponse.message}");
        }

        private async UniTask FetchXTokenRateAsync(int chainId) {
            var response = await XChain.Instance.APIService.GetXTokenPriceInUSDT(chainId);
            if (response.IsSuccess) {
                _xTokenConversionRate = response.SuccessResponse;
                return;
            }
            throw new Exception($"Failed to fetch token rate {response.FailureResponse.message}");
        }

        private void UpdateBalanceHeaderText(int selectedIndex)
        {
            Currency selectedCurrency = currencies[selectedIndex];
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
            if (response.IsSuccess) {
                balanceText.text = $"Balance: <color=black><b>{response.SuccessResponse.balance} {_symbol}</b></color>";
                Debug.Log($"Balance: {response.SuccessResponse.balance}");
                return;
            }
            throw new Exception($"Failed to fetch balance: {response.FailureResponse.message}");
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

        public async void BuyXToken(){
            await SendBuyXTokenRequestAsync();
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
                await XChain.FetchXTokenBalance();
            }
            else
            {
                Debug.Log($"BuyXToken Failed: {response.FailureResponse.message}");
            }
        }

        private void OnXTokenBalanceUpdate(float balance){
            var balanceRound = Mathf.FloorToInt(balance);
            xTokenBalanceText.text = $"Balance: {balanceRound} XTK";
        }

        public async void GetWalletAddress() {
            await GetPlayerToken();
        }

        private async UniTask GetPlayerToken() {
            var response = await XChain.Instance.APIService.GetUserDetails(_accessToken);
            if (response.IsSuccess) {
                XChain.Instance.Context.Web3Context.AccessKey = response.SuccessResponse.walletAddress;
                Debug.Log(response.SuccessResponse.walletAddress);
            }
            else {
                Debug.Log($"Get User Details Failed: {response.FailureResponse.message}");
            }
        }

        public void ExitBuyXFlow() {
            XChain.EndBuyXFlow();
        }

        public override void OnHide(){

        }

        public override void OnShow(){

        }
    }
}
