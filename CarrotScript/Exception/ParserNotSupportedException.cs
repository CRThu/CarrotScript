namespace CarrotScript.Exception
{
    public class ParserNotSupportedException : ExceptionBase
    {
        public ParserNotSupportedException(TokenSpan? pos)
        : base("InvalidSyntaxException", pos)
        {
        }

        public ParserNotSupportedException(string msg, TokenSpan? pos)
        : base(msg, pos)
        {
        }
    }
}
