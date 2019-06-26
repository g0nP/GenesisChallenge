namespace GenesisChallenge.Domain
{
    public class CustomError
    {
        public string Message { get; }

        public CustomError(string message)
        {
            Message = message;
        }
    }
}
