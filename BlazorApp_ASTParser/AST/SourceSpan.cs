// BlazorApp_ASTParser -- SourceSpan.cs
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

public readonly struct SourceSpan(SourcePosition start, SourcePosition end) : IEquatable<SourceSpan>
{
    public SourcePosition End => end;

    public int Length => end.Index - start.Index;

    public SourcePosition Start => start;

    public static bool operator !=(SourceSpan left, SourceSpan right)
    {
        return !left.Equals(right);
    }

    public static bool operator ==(SourceSpan left, SourceSpan right)
    {
        return left.Equals(right);
    }

    public override bool Equals(object? obj)
    {
        if (obj is SourceSpan span)
        {
            return Equals(span);
        }

        return base.Equals(obj);
    }

    public bool Equals(SourceSpan other)
    {
        return other.Start == Start && other.End == End;
    }

    public override int GetHashCode()
    {
        return 0x509CE ^ Start.GetHashCode() ^ End.GetHashCode();
    }

    public override string ToString()
    {
        return $"Line: {start.Line} | Col: {start.Column} | Len: {Length}";
    }
}