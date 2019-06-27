namespace GenesisChallenge.Domain
{
    /// <summary>
    /// Creates a custom error
    /// </summary>
    public class CustomError
    {
        /// <value>The error message</value>
        public string Message { get; }

        public CustomError(string message)
        {
            Message = message;
        }
    }
}
