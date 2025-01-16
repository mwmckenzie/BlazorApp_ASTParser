// BlazorApp_ASTParser -- TokenType.cs
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

public enum TokenType
{
    #region Meta

    EndOfFile,
    Error,

    #endregion Meta

    #region WhiteSpace

    WhiteSpace,
    NewLine,

    #endregion WhiteSpace

    #region Comments

    LineComment,
    BlockComment,

    #endregion Comments

    #region Constants

    IntegerLiteral,
    StringLiteral,
    FloatLiteral,

    #endregion Constants

    #region Identifiers

    Identifier,
    Keyword,

    #endregion Identifiers

    #region Groupings

    LeftBracket, // {
    RightBracket, // }
    RightBrace, // ]
    LeftBrace, // [
    LeftParenthesis, // (
    RightParenthesis, // )

    #endregion Groupings

    #region Operators

    GreaterThanOrEqual, // >=
    GreaterThan, // >

    LessThan, // <
    LessThanOrEqual, // <=

    PlusEqual, // +=
    PlusPlus, // ++
    Plus, // +

    MinusEqual, // -=
    MinusMinus, // --
    Minus, // -

    Assignment, // =

    Not, // !
    NotEqual, // !=

    Mul, // *
    MulEqual, // *=

    Div, // /
    DivEqual, // /=

    BooleanAnd, // &&
    BooleanOr, // ||

    BitwiseAnd, // &
    BitwiseOr, // |

    BitwiseAndEqual, // &=
    BitwiseOrEqual, // |=

    ModEqual, // %=
    Mod, // %

    BitwiseXorEqual, // ^=
    BitwiseXor, // ^

    DoubleQuestion, // ??
    Question, // ?

    Equal, // ==

    BitShiftLeft, // <<
    BitShiftRight, // >>

    #endregion Operators

    #region Punctuation

    Dot,
    Comma,
    Semicolon,
    Colon,
    Arrow, // ->
    FatArrow, // =>

    #endregion Punctuation
}