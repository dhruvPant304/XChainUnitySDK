namespace Core.API
{
    public class RequestResponse<TSuccess, TFailure>
    {
        public bool IsSuccess;
        public TSuccess SuccessResponse;
        public TFailure FailureResponse;
    }
}
