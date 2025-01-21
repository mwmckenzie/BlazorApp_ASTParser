// BlazorApp_ASTParser -- CharParser.cs
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

using System.Globalization;

namespace BlazorApp_ASTParser.GenericLanguagePrototype;

public class CharParser
{
    public static readonly char NullChar = '\0';
    
    public static bool IsNullChar(char c) => c == NullChar;


    public List<CharInfo> Parse(string code)
    {
        var infoList = new List<CharInfo>();
        
        foreach (var codeChar in code)
        {
            var category = Char.GetUnicodeCategory(codeChar);
            CharGroup group;
            
            switch (category)
            {
                case UnicodeCategory.UppercaseLetter:
                case UnicodeCategory.LowercaseLetter:
                case UnicodeCategory.TitlecaseLetter:
                case UnicodeCategory.ModifierLetter:
                case UnicodeCategory.OtherLetter:
                    group = CharGroup.Letter;
                    break;
                
                case UnicodeCategory.NonSpacingMark:
                case UnicodeCategory.SpacingCombiningMark:
                case UnicodeCategory.EnclosingMark:
                    group = CharGroup.Mark;
                    break;
                
                case UnicodeCategory.DecimalDigitNumber:
                case UnicodeCategory.LetterNumber:
                case UnicodeCategory.OtherNumber:
                    group = CharGroup.Number;
                    break;
                
                case UnicodeCategory.SpaceSeparator:
                case UnicodeCategory.LineSeparator:
                case UnicodeCategory.ParagraphSeparator:
                    group = CharGroup.Separator;
                    break;
                
                case UnicodeCategory.Control:
                case UnicodeCategory.Format:
                case UnicodeCategory.Surrogate:
                case UnicodeCategory.PrivateUse:
                    group = CharGroup.Special;
                    break;
                
                case UnicodeCategory.ConnectorPunctuation:
                case UnicodeCategory.DashPunctuation:
                case UnicodeCategory.OpenPunctuation:
                case UnicodeCategory.ClosePunctuation:
                case UnicodeCategory.InitialQuotePunctuation:
                case UnicodeCategory.FinalQuotePunctuation:
                case UnicodeCategory.OtherPunctuation:
                    group = CharGroup.Punctuation;
                    break;
                
                case UnicodeCategory.MathSymbol:
                case UnicodeCategory.CurrencySymbol:
                case UnicodeCategory.ModifierSymbol:
                case UnicodeCategory.OtherSymbol:
                    group = CharGroup.Symbol;
                    break;
                
                case UnicodeCategory.OtherNotAssigned:
                    group = CharGroup.NotAssigned;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
            infoList.Add(new CharInfo(codeChar, group, category));
        }
        
        return infoList;
    }
    
}