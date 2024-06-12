namespace CarrotScript.Exception
{
    public class InvalidSyntaxException : ExceptionBase
    {
        public InvalidSyntaxException()
        : base("InvalidSyntaxException")
        {
        }

        public InvalidSyntaxException(TokenPosition pos)
        : base("InvalidSyntaxException", pos)
        {
        }
    }
}
