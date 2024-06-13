namespace CarrotScript.Exception
{
    public class LexarNotSupportedException : ExceptionBase
    {
        public LexarNotSupportedException() : base("")
        {

        }

        public LexarNotSupportedException(TokenSpan? pos)
        : base("InvalidSyntaxException", pos)
        {
        }

        public LexarNotSupportedException(string msg, TokenSpan? pos)
        : base(msg, pos)
        {
        }
    }
}
