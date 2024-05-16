using System;
using Types;

namespace Core.API.APIResponse {
    [Serializable]
    public class OwnedNFTSuccessResponse {
        public string id;
        public string createdBy;
        public string createdById;
        public string updatedBy;
        public string updatedById;
        public string name;
        public string description;
        public float royaltyPercentage;
        public int totalCopies;
        public string creatorId;
        public string txHash;
        public string txStatus;
        public int score;
        public string[] tags;
        public int price;
        public string gameId;
        public string createdAt;
        public string updatedAt;
        public Creator creator;
        public Creator game;
        public int uniqueOwners;
        public int totalSales;
        public int tokenId;
        public string tokenURI;
        public int amount;
        public string currency;
        public AssetURLs[] assetUrls;
        public bool isOwned;
        public bool isLiked;
    }
}
