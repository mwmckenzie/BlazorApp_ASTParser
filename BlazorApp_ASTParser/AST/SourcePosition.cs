// BlazorApp_ASTParser -- SourcePosition.cs
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

public readonly struct SourcePosition(int index, int line, int column) : IEquatable<SourcePosition>
{
    public int Column => column;

    public int Index => index;

    public int Line => line;

    public static bool operator !=(SourcePosition left, SourcePosition right)
    {
        return !left.Equals(right);
    }

    public static bool operator ==(SourcePosition left, SourcePosition right)
    {
        return left.Equals(right);
    }

    public override bool Equals(object? obj)
    {
        if (obj is SourcePosition position)
        {
            return Equals(position);
        }

        return base.Equals(obj);
    }

    public bool Equals(SourcePosition other)
    {
        return other.GetHashCode() == GetHashCode();
    }

    public override int GetHashCode()
    {
        return 0xB1679EE ^ Index ^ Line ^ Column;
    }
}