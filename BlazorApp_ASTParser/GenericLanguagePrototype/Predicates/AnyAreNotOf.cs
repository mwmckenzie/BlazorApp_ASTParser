﻿// BlazorApp_ASTParser -- AnyAreNotOf.cs
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

namespace BlazorApp_ASTParser.GenericLanguagePrototype.Predicates;

public class AnyAreNotOf(Token expectedType) : Predicate(expectedType)
{
    public override bool Test(List<Token> tokens)
    {
        predicate = token => token.IsA(_expectedType);
        return !tokens.All(predicate);
    }
}