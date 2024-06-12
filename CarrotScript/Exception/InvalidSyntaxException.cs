namespace CarrotScript.Exception
{
    public class InvalidSyntaxException : ExceptionBase
    {
        public InvalidSyntaxException(TokenPosition? pos)
        : base("InvalidSyntaxException", pos)
        {
        }

        public InvalidSyntaxException(string msg, TokenPosition? pos)
        : base(msg, pos)
        {
        }
    }
}
