namespace CarrotScript.Exception
{
    public class ParserNotSupportedException : ExceptionBase
    {
        public ParserNotSupportedException()
        : base("ParserNotSupportedException")
        {
        }

        public ParserNotSupportedException(TokenPosition pos)
        : base("ParserNotSupportedException", pos)
        {
        }
    }
}
