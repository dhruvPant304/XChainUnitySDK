using System;

namespace Core.API.APIResponse
{
    [Serializable]
    public class GetScoreRequestResponse
    {
        public int id;
        public object createdBy;
        public object createdById;
        public object updatedBy;
        public object updatedById;
        public int gameSessionId;
        public int gameId;
        public int userId;
        public int score;
        public string date;
        public string time;
        public DateTime createdAt;
        public DateTime updatedAt;
    }
}
