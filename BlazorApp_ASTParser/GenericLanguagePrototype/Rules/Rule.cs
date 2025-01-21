// BlazorApp_ASTParser -- Rule.cs
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

using BlazorApp_ASTParser.GenericLanguagePrototype.Predicates;

namespace BlazorApp_ASTParser.GenericLanguagePrototype.Rules;

public class Rule
{
    private Predicate _predicate;
    private Token _successToken;
    
    private IRulePredicate _rulePredicate;
    
    public Rule(Predicate predicate, Token successToken)
    {
        _predicate = predicate;
        _successToken = successToken;
    }

    public bool RulePredicateTest()
    {
        var parent = new Token("Parent", null);
        var tokenA = new Token("A", parent);
        var tokenB = new Token("B", parent);
        
        var tokenAWeWant = new Token("AWeWant", parent);
        var tokenBWeWant = new Token("BWeWant", parent);
        
        var testA = new IsA(tokenA, tokenAWeWant);
        var testB = new IsA(tokenB, tokenBWeWant);
        var testAB = new And(testA, testB);
        var testABOrA = new Or(testAB, testA);
        
        _rulePredicate = testABOrA;
        return _rulePredicate.Evaluate();
    }

    public bool Test(List<Token> tokens)
    {
        return _predicate.Test(tokens);
    }

    
}