using System;

namespace Core.API.APIResponse {
    [Serializable]
    public class UserDetailsSucessResponse
    {
        public string id;
        public string createdBy;
        public string createdById;
        public string updatedBy;
        public string updatedById;
        public string name;
        public bool active;
        public string username;
        public string email;
        public string orgId;
        public string walletAddress;
        public string lastActiveDate;
        public string lastActiveTime;
        public string type;
        public string createdAt;
        public string updatedAt;
        public string deletedAt;
    }
}