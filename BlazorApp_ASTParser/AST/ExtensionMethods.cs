// BlazorApp_ASTParser -- ExtensionMethods.cs
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

public static class ExtensionMethods
{
    /// <summary>
    /// Gets that <see cref="char"/> at a given <paramref name="index"/>.
    /// </summary>
    /// <param name="str">The <see cref="string"/> to search.</param>
    /// <param name="index">The index of <paramref name="str"/> to return.</param>
    /// <returns>
    /// The <see cref="char"/> at the given <paramref cref="index"/>.  If <paramref name="index"/> is less than 0
    /// or greater than the length of the string, returns an ASCII NULL (\0).
    /// </returns>
    public static char CharAt(this string str, int index)
    {
        if (index > str.Length - 1 || index < 0)
        {
            return '\0';
        }

        return str[index];
    }
}