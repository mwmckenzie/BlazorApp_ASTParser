// BlazorApp_ASTParser -- Lexer.cs
// 
// Copyright (C) 2025 Matthew W. McKenzie and Kenz LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Text;

namespace BlazorApp_ASTParser.AST;

public class Lexer
{
    private static readonly string[] _keywords =
    {
        "class", "func", "prop", "cotr", "new", "if", "else", "switch", "case", "default",
        "break", "return", "do", "while", "for", "var", "null", "true", "false"
    };

    private readonly StringBuilder _builder;
    private int _column;
    private int _index;
    private int _line;
    private SourceCode _sourceCode;
    private SourcePosition _tokenStart;

    public Lexer() : this(new ErrorSink()) { }

    public Lexer(ErrorSink errorSink)
    {
        ErrorSink = errorSink;
        _builder = new StringBuilder();
        _sourceCode = null;
    }

    public ErrorSink ErrorSink { get; }

    private char _ch => _sourceCode[_index];

    private char _last => Peek(-1);

    private char _next => Peek(1);

    public IEnumerable<Token> LexFile(string sourceCode)
    {
        return LexFile(new SourceCode(sourceCode));
    }

    public IEnumerable<Token> LexFile(SourceCode sourceCode)
    {
        _sourceCode = sourceCode;
        _builder.Clear();
        _line = 1;
        _index = 0;
        _column = 0;
        CreateToken(TokenType.EndOfFile);

        return LexContents();
    }

    private void AddError(string message, ErrorSeverity severity)
    {
        var span = new SourceSpan(_tokenStart, new SourcePosition(_index, _line, _column));
        ErrorSink.AddError(message, _sourceCode, severity, span);
    }

    private void Advance()
    {
        _index++;
        _column++;
    }

    private void Consume()
    {
        _builder.Append(_ch);
        Advance();
    }

    private Token CreateToken(TokenType kind)
    {
        var contents = _builder.ToString();
        var end = new SourcePosition(_index, _line, _column);
        var start = _tokenStart;

        _tokenStart = end;
        _builder.Clear();

        return new Token(kind, contents, start, end);
    }

    private void DoNewLine()
    {
        _line++;
        _column = 0;
    }

    private bool IsDigit()
    {
        return char.IsDigit(_ch);
    }

    private bool IsEOF()
    {
        return _ch == '\0';
    }

    private bool IsIdentifier()
    {
        return IsLetterOrDigit() || _ch == '_';
    }

    private bool IsKeyword()
    {
        return _keywords.Contains(_builder.ToString());
    }

    private bool IsLetter()
    {
        return char.IsLetter(_ch);
    }

    private bool IsLetterOrDigit()
    {
        return char.IsLetterOrDigit(_ch);
    }

    private bool IsNewLine()
    {
        return _ch == '\n';
    }

    private bool IsPunctuation()
    {
        return "<>{}()[]!%^&*+-=/.,?;:|".Contains(_ch);
    }

    private bool IsWhiteSpace()
    {
        return (char.IsWhiteSpace(_ch) || IsEOF()) && !IsNewLine();
    }

    private IEnumerable<Token> LexContents()
    {
        while (!IsEOF()) yield return LexToken();

        yield return CreateToken(TokenType.EndOfFile);
    }

    private Token LexToken()
    {
        if (IsEOF())
        {
            return CreateToken(TokenType.EndOfFile);
        }

        if (IsNewLine())
        {
            return ScanNewLine();
        }

        if (IsWhiteSpace())
        {
            return ScanWhiteSpace();
        }

        if (IsDigit())
        {
            return ScanInteger();
        }

        if (_ch == '/' && (_next == '/' || _next == '*'))
        {
            return ScanComment();
        }

        if (IsLetter() || _ch == '_')
        {
            return ScanIdentifier();
        }

        if (_ch == '"')
        {
            return ScanStringLiteral();
        }

        if (_ch == '.' && char.IsDigit(_next))
        {
            return ScanFloat();
        }

        if (IsPunctuation())
        {
            return ScanPunctuation();
        }

        return ScanWord();
    }

    private char Peek(int ahead)
    {
        return _sourceCode[_index + ahead];
    }

    private Token ScanBlockComment()
    {
        var isEndOfComment = () => _ch == '*' && _next == '/';
        while (!isEndOfComment())
        {
            if (IsEOF())
            {
                return CreateToken(TokenType.Error);
            }

            if (IsNewLine())
            {
                DoNewLine();
            }

            Consume();
        }

        Consume();
        Consume();

        return CreateToken(TokenType.BlockComment);
    }

    private Token ScanComment()
    {
        Consume();
        if (_ch == '*')
        {
            return ScanBlockComment();
        }

        Consume();

        while (!IsNewLine() && !IsEOF()) Consume();

        return CreateToken(TokenType.LineComment);
    }

    private Token ScanFloat()
    {
        if (_ch == 'f')
        {
            Advance();
            if ((!IsWhiteSpace() && !IsPunctuation() && !IsEOF()) || _ch == '.')
            {
                return ScanWord(message: "Remove 'f' in floating point number.");
            }

            return CreateToken(TokenType.FloatLiteral);
        }

        var preDotLength = _index - _tokenStart.Index;

        if (_ch == '.')
        {
            Consume();
        }

        while (IsDigit()) Consume();

        if (_last == '.')
        {
            // .e10 is invalid.
            return ScanWord(message: "Must contain digits after '.'");
        }

        if (_ch == 'e')
        {
            Consume();
            if (preDotLength > 1)
            {
                return ScanWord(message: "Coefficient must be less than 10.");
            }

            if (_ch == '+' || _ch == '-')
            {
                Consume();
            }

            while (IsDigit()) Consume();
        }

        if (_ch == 'f')
        {
            Consume();
        }

        if (!IsWhiteSpace() && !IsPunctuation() && !IsEOF())
        {
            if (IsLetter())
            {
                return ScanWord(message: "'{0}' is an invalid float value");
            }

            return ScanWord();
        }

        return CreateToken(TokenType.FloatLiteral);
    }

    private Token ScanIdentifier()
    {
        while (IsIdentifier()) Consume();

        if (!IsWhiteSpace() && !IsPunctuation() && !IsEOF())
        {
            return ScanWord();
        }

        if (IsKeyword())
        {
            return CreateToken(TokenType.Keyword);
        }

        return CreateToken(TokenType.Identifier);
    }

    private Token ScanInteger()
    {
        while (IsDigit()) Consume();

        if (_ch == 'f' || _ch == '.' || _ch == 'e')
        {
            return ScanFloat();
        }

        if (!IsWhiteSpace() && !IsPunctuation() && !IsEOF())
        {
            return ScanWord();
        }

        return CreateToken(TokenType.IntegerLiteral);
    }

    private Token ScanNewLine()
    {
        Consume();

        DoNewLine();

        return CreateToken(TokenType.NewLine);
    }

    private Token ScanPunctuation()
    {
        switch (_ch)
        {
            case ';':
                Consume();
                return CreateToken(TokenType.Semicolon);

            case ':':
                Consume();
                return CreateToken(TokenType.Colon);

            case '{':
                Consume();
                return CreateToken(TokenType.LeftBracket);

            case '}':
                Consume();
                return CreateToken(TokenType.RightBracket);

            case '[':
                Consume();
                return CreateToken(TokenType.LeftBrace);

            case ']':
                Consume();
                return CreateToken(TokenType.RightBrace);

            case '(':
                Consume();
                return CreateToken(TokenType.LeftParenthesis);

            case ')':
                Consume();
                return CreateToken(TokenType.RightParenthesis);

            case '>':
                Consume();
                if (_ch == '=')
                {
                    Consume();
                    return CreateToken(TokenType.GreaterThanOrEqual);
                }

                if (_ch == '>')
                {
                    Consume();
                    return CreateToken(TokenType.BitShiftRight);
                }

                return CreateToken(TokenType.GreaterThan);

            case '<':
                Consume();
                if (_ch == '=')
                {
                    Consume();
                    return CreateToken(TokenType.LessThanOrEqual);
                }

                if (_ch == '<')
                {
                    Consume();
                    return CreateToken(TokenType.BitShiftLeft);
                }

                return CreateToken(TokenType.LessThan);

            case '+':
                Consume();
                if (_ch == '=')
                {
                    Consume();
                    return CreateToken(TokenType.PlusEqual);
                }

                if (_ch == '+')
                {
                    Consume();
                    return CreateToken(TokenType.PlusPlus);
                }

                return CreateToken(TokenType.Plus);

            case '-':
                Consume();
                if (_ch == '=')
                {
                    Consume();
                    return CreateToken(TokenType.MinusEqual);
                }

                if (_ch == '>')
                {
                    Consume();
                    return CreateToken(TokenType.Arrow);
                }

                if (_ch == '-')
                {
                    Consume();
                    return CreateToken(TokenType.MinusMinus);
                }

                return CreateToken(TokenType.Minus);

            case '=':
                Consume();
                if (_ch == '=')
                {
                    Consume();
                    return CreateToken(TokenType.Equal);
                }

                if (_ch == '>')
                {
                    Consume();
                    return CreateToken(TokenType.FatArrow);
                }

                return CreateToken(TokenType.Assignment);

            case '!':
                Consume();
                if (_ch == '=')
                {
                    Consume();
                    return CreateToken(TokenType.NotEqual);
                }

                return CreateToken(TokenType.Not);

            case '*':
                Consume();
                if (_ch == '=')
                {
                    Consume();
                    return CreateToken(TokenType.MulEqual);
                }

                return CreateToken(TokenType.Mul);

            case '/':
                Consume();
                if (_ch == '=')
                {
                    Consume();
                    return CreateToken(TokenType.DivEqual);
                }

                return CreateToken(TokenType.Div);

            case '.':
                Consume();
                return CreateToken(TokenType.Dot);

            case ',':
                Consume();
                return CreateToken(TokenType.Comma);

            case '&':
                Consume();
                if (_ch == '&')
                {
                    Consume();
                    return CreateToken(TokenType.BooleanAnd);
                }

                if (_ch == '=')
                {
                    Consume();
                    return CreateToken(TokenType.BitwiseAndEqual);
                }

                return CreateToken(TokenType.BitwiseAnd);

            case '|':
                Consume();
                if (_ch == '|')
                {
                    Consume();
                    return CreateToken(TokenType.BooleanOr);
                }

                if (_ch == '=')
                {
                    Consume();
                    return CreateToken(TokenType.BitwiseOrEqual);
                }

                return CreateToken(TokenType.BitwiseOr);

            case '%':
                Consume();
                if (_ch == '=')
                {
                    Consume();
                    return CreateToken(TokenType.ModEqual);
                }

                return CreateToken(TokenType.Mod);

            case '^':
                Consume();
                if (_ch == '=')
                {
                    Consume();
                    return CreateToken(TokenType.BitwiseXorEqual);
                }

                return CreateToken(TokenType.BitwiseXor);

            case '?':
                Consume();
                if (_ch == '?')
                {
                    Consume();
                    return CreateToken(TokenType.DoubleQuestion);
                }

                return CreateToken(TokenType.Question);

            default: return ScanWord();
        }
    }

    private Token ScanStringLiteral()
    {
        Advance();

        while (_ch != '"')
        {
            if (IsEOF())
            {
                AddError("Unexpected End Of File", ErrorSeverity.Fatal);
                return CreateToken(TokenType.Error);
            }

            Consume();
        }

        Advance();

        return CreateToken(TokenType.StringLiteral);
    }

    private Token ScanWhiteSpace()
    {
        while (IsWhiteSpace()) Consume();
        return CreateToken(TokenType.WhiteSpace);
    }

    private Token ScanWord(ErrorSeverity severity = ErrorSeverity.Error, string message = "Unexpected Token '{0}'")
    {
        while (!IsWhiteSpace() && !IsEOF() && !IsPunctuation()) Consume();
        AddError(string.Format(message, _builder), severity);
        return CreateToken(TokenType.Error);
    }
}