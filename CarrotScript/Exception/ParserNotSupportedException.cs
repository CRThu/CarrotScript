namespace CarrotScript.Exception
{
    public class ParserNotSupportedException : ExceptionBase
    {
        public ParserNotSupportedException(TokenPosition? pos)
        : base("InvalidSyntaxException", pos)
        {
        }

        public ParserNotSupportedException(string msg, TokenPosition? pos)
        : base(msg, pos)
        {
        }
    }
}
