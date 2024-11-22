
using CarrotScript.Reader;
using System.Text;

namespace CarrotScript.Lexar
{
    public class CarrotScriptLexar : ILexar
    {
        private TokenReader? Reader { get; set; }
        private List<Token> ResultTokens { get; set; }
        private StringBuilder Buffer { get; set; }
        private CodePosition Start { get; set; }
        private CodePosition End { get; set; }

        //private Stack<XmlLexarState> ContextStates { get; set; }
        //private XmlLexarState State => ContextStates.Peek();

        public IEnumerable<Token> Tokenize(IEnumerable<Token> inputTokens)
        {
            foreach (Token inputToken in inputTokens)
            {
                Reader = new TokenReader(inputToken);
                RootLexar();
            }
            return ResultTokens;
        }

        public void RootLexar()
        {
            // check reader has read to end or not
            while (Reader != null && Reader.CurrentChar != null)
            {

            }
        }
    }
}