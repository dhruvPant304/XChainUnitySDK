using System;
using Core.API.APIResponse;

namespace Data{
	[Serializable]
	public class LoginCredentialData {
		public string accessToken;
		public UserDetails userDetails;
		public string privateKey;
	}
}
