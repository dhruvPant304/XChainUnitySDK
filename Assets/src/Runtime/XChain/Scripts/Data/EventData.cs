using System;

namespace Data {
    [Serializable]
    public class EventData {
        public string eventName;
        public EventDataData eventData;
    }

    [Serializable]
    public class EventDataData {
        public string accessToken;
        public string accessKey;
    }
}
