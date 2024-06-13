namespace CarrotScript.Exception
{
    public class InvalidSyntaxException : ExceptionBase
    {
        public InvalidSyntaxException(TokenSpan? pos)
        : base("InvalidSyntaxException", pos)
        {
        }

        public InvalidSyntaxException(string msg, TokenSpan? pos)
        : base(msg, pos)
        {
        }
    }
}
