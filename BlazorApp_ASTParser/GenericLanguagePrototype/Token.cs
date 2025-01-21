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

namespace BlazorApp_ASTParser.GenericLanguagePrototype;

public class Token : IEquatable<Token>
{
    protected bool isNone = false;
    
    public readonly Token? parent;
    public readonly string tokenType;
    
    public static readonly Token None = new(true);
    private Token(bool isNone = false)
    {
        parent = null;
        this.isNone = isNone;
        tokenType = "none";
    }
    
    public Token(string type, Token? parent)
    {
        tokenType = type;
        this.parent = parent ?? None;
        isNone = false;
    }
    
    
    public bool IsA(Token expectedType)
    {
        if (expectedType == this) return true;
        
        var ancestor = parent;
        while (ancestor != None && ancestor != null)
        {
            if (ancestor == expectedType) return true;
            ancestor = ancestor?.parent;
        }
        return false;
    }

    public bool Equals(Token? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return tokenType == other.tokenType;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Token)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(tokenType);
    }

    public static bool operator ==(Token? left, Token? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Token? left, Token? right)
    {
        return !Equals(left, right);
    }
}