namespace CarrotScript.Exception
{
    public class LexarNotSupportedException : ExceptionBase
    {
        public LexarNotSupportedException()
        : base("LexarNotSupportedException")
        {
        }

        public LexarNotSupportedException(TokenPosition pos)
        : base("LexarNotSupportedException", pos)
        {
        }
    }
}
