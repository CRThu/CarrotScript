using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarrotScript.Lang.Def;

namespace CarrotScript.Reader
{
    public class TokenPositionContext
    {
        public string File { get; set; }

        public int FixedOffset { get; set; }

        public int Offset { get; set; }

        public int Line { get; set; }

        public int Column { get; set; }

        public CodePosition Position => new CodePosition(File, FixedOffset + Offset, Line, Column);

        public void Reset(int startPos)
        {
            Offset = 0;
            FixedOffset = startPos;
        }

        public void Update(char? c)
        {
            if (c == null)
                return;
            else if (c == '\n')
            {
                Line += 1;
                Column = 1;
            }
            else
            {
                Column += 1;
            }
            Offset += 1;
        }
    }

    public interface ITokensReader
    {
        public bool IsAtEnd { get; }
        public Token? CurrentToken { get; }

        public char? CurrentChar { get; }
        public char? NextChar { get; }
        public Symbol? CurrentSymbol { get; }
        public Symbol? NextSymbol { get; }
        public CodePosition Position { get; }
        public void AdvanceToken();
        public char? Advance();

    }

    public class TokensReader : ITokensReader
    {
        private readonly List<Token> _tokens;   // token列表
        private int _currentTokenIndex;         // token列表索引
        private int _currentCharIndex;          // token内部字符索引

        private TokenPositionContext _positionContext;

        public bool IsAtEnd => _currentTokenIndex >= _tokens.Count;
        public Token? CurrentToken => IsAtEnd ? null : _tokens[_currentTokenIndex];
        //public bool IsAtTokenEnd => CurrentToken == null || _currentCharIndex >= CurrentToken.Value.Length;

        public char? CurrentChar
        {
            get
            {
                if (CurrentToken == null
                    || _currentCharIndex >= CurrentToken.Value.Length)
                    return null;
                else
                    return CurrentToken!.Value[_currentCharIndex];
            }
        }

        public char? NextChar
        {
            get
            {
                if (CurrentToken == null
                    || _currentCharIndex + 1 >= CurrentToken.Value.Length)
                    return null;
                else
                    return CurrentToken!.Value[_currentCharIndex + 1];
            }
        }

        // TODO support multichar symbol
        public Symbol? CurrentSymbol => CurrentChar.ToSymbol();
        public Symbol? NextSymbol => NextChar.ToSymbol();

        public CodePosition Position => _positionContext.Position;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code"></param>
        public TokensReader(IEnumerable<Token> tokens)
        {
            _tokens = tokens != null ? new List<Token>(tokens) : throw new ArgumentNullException();
            _currentTokenIndex = 0;
            _currentCharIndex = 0;
            _positionContext = new TokenPositionContext();
        }

        public void AdvanceToken()
        {
            _currentTokenIndex++;
            _currentCharIndex = 0;
            _positionContext.Reset(CurrentToken == null ? 0 : CurrentToken.Span.Start.Offset);
        }

        public char? Advance()
        {
            if (IsAtEnd)
            {
                return null;
            }
            _positionContext.Update(CurrentChar);
            _currentCharIndex += 1;
            return CurrentChar;
        }
    }

}
