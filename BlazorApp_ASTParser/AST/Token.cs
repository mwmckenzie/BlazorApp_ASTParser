// BlazorApp_ASTParser -- Token.cs
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

namespace BlazorApp_ASTParser.AST;

public class Token
{
    private readonly Lazy<TokenFunction> _tokenFunction;

    public TokenFunction tokenFunction => _tokenFunction.Value;

    public TokenType Kind { get; }

    public SourceSpan Span { get; }

    public string Value { get; }

    public Token(TokenType kind, string contents, SourcePosition start, SourcePosition end)
    {
        Kind = kind;
        Value = contents;
        Span = new SourceSpan(start, end);

        _tokenFunction = new Lazy<TokenFunction>(GetTokenFunction);
    }

    public static bool operator !=(Token left, string right)
    {
        return left?.Value != right;
    }

    public static bool operator !=(string left, Token right)
    {
        return right?.Value != left;
    }

    public static bool operator !=(Token left, TokenType right)
    {
        return left?.Kind != right;
    }

    public static bool operator !=(TokenType left, Token right)
    {
        return right?.Kind != left;
    }

    public static bool operator ==(Token left, string right)
    {
        return left?.Value == right;
    }

    public static bool operator ==(string left, Token right)
    {
        return right?.Value == left;
    }

    public static bool operator ==(Token left, TokenType right)
    {
        return left?.Kind == right;
    }

    public static bool operator ==(TokenType left, Token right)
    {
        return right?.Kind == left;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Token token)
        {
            return Equals(token);
        }

        return base.Equals(obj);
    }

    public bool Equals(Token other)
    {
        return other.Value == Value &&
               other.Span == Span &&
               other.Kind == Kind;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode() ^ Span.GetHashCode() ^ Kind.GetHashCode();
    }

    public bool IsTrivia()
    {
        return tokenFunction is TokenFunction.WhiteSpace or TokenFunction.Comment;
    }

    private TokenFunction GetTokenFunction()
    {
        switch (Kind)
        {
            case TokenType.Arrow:
            case TokenType.FatArrow:
            case TokenType.Colon:
            case TokenType.Semicolon:
            case TokenType.Comma:
            case TokenType.Dot:
                return TokenFunction.Punctuation;

            case TokenType.Equal:
            case TokenType.NotEqual:
            case TokenType.Not:
            case TokenType.LessThan:
            case TokenType.LessThanOrEqual:
            case TokenType.GreaterThan:
            case TokenType.GreaterThanOrEqual:
            case TokenType.BooleanOr:
            case TokenType.BooleanAnd:
                return TokenFunction.OperatorBool;
                
                
            case TokenType.Plus:
            case TokenType.Minus:
            case TokenType.Mul:
            case TokenType.Div:
            case TokenType.Mod:
            case TokenType.BitwiseXor:
            case TokenType.BitwiseOr:
            case TokenType.BitwiseAnd:
            case TokenType.BitShiftLeft:
            case TokenType.BitShiftRight:
                return TokenFunction.OperatorCalculation;
            
            case TokenType.MinusEqual:
            case TokenType.ModEqual:
            case TokenType.MulEqual:
            case TokenType.PlusEqual:
            case TokenType.DivEqual:
            case TokenType.BitwiseXorEqual:
            case TokenType.BitwiseOrEqual:
            case TokenType.BitwiseAndEqual:
                return TokenFunction.OperatorInPlace;
            
            case TokenType.PlusPlus:
            case TokenType.MinusMinus:
                return TokenFunction.OperatorIterator;
            
            case TokenType.Question:
            case TokenType.DoubleQuestion:
            case TokenType.Assignment:

            case TokenType.BlockComment:
            case TokenType.LineComment:
                return TokenFunction.Comment;

            case TokenType.NewLine:
            case TokenType.WhiteSpace:
                return TokenFunction.WhiteSpace;

            case TokenType.LeftBrace:
            case TokenType.LeftBracket:
            case TokenType.LeftParenthesis:
            case TokenType.RightBrace:
            case TokenType.RightBracket:
            case TokenType.RightParenthesis:
                return TokenFunction.Grouping;

            case TokenType.Identifier:
            case TokenType.Keyword:
                return TokenFunction.Identifier;

            case TokenType.StringLiteral:
            case TokenType.IntegerLiteral:
            case TokenType.FloatLiteral:
                return TokenFunction.Constant;

            case TokenType.Error:
                return TokenFunction.Invalid;

            default: return TokenFunction.Unknown;
        }
    }
}