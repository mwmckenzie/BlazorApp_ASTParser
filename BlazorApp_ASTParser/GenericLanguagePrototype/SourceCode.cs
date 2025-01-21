// BlazorApp_ASTParser -- SourceCode.cs
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

using BlazorApp_ASTParser.AST;

namespace BlazorApp_ASTParser.GenericLanguagePrototype;

public record SourceCode(string Code)
{
    public char this[int index] => Code.CharAt(index);
    public int Length => Code.Length;
    
    public string GetSpanText(SourceCodeSpan span) => Code.Substring(span.start, span.length);
}