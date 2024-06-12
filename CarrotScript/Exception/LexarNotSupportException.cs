namespace CarrotScript.Exception
{
    public class LexarNotSupportedException : ExceptionBase
    {
        public LexarNotSupportedException() : base("")
        {

        }

        public LexarNotSupportedException(TokenPosition? pos)
        : base("InvalidSyntaxException", pos)
        {
        }

        public LexarNotSupportedException(string msg, TokenPosition? pos)
        : base(msg, pos)
        {
        }
    }
}
