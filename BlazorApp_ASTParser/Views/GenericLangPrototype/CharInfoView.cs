// BlazorApp_ASTParser -- CharInfoView.cs
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

using BlazorApp_ASTParser.GenericLanguagePrototype;

namespace BlazorApp_ASTParser.Views.GenericLangPrototype;

public class CharInfoView
{
    public List<CharInfo> CharInfoList { get; set; } = new();
    
    public void AddCharInfo(CharInfo charInfo)
    {
        CharInfoList.Add(charInfo);
    }

    public TextElementViewModel CreateTextElementViewModel(CharInfo charInfo)
    {
        var viewModel = new TextElementViewModel();

        viewModel.text = GetElementText(charInfo.group, charInfo.value.ToString());
        viewModel.styleAttribText = GetStyleText(charInfo.group);
        viewModel.classText = GetElementClass(charInfo.group);
        
        return viewModel;
    }

    private static string GetElementClass(CharGroup group)
    {
        switch (group)
        {
            case CharGroup.Letter:
            case CharGroup.Number:
                return "ms-1";
                
            case CharGroup.Mark:
            case CharGroup.Separator:
            case CharGroup.Special:
                return "ms-1";
            case CharGroup.Punctuation:
            case CharGroup.Symbol:
                return "ms-1";
            case CharGroup.NotAssigned:
                return "ms-5";
            default:
                throw new ArgumentOutOfRangeException(nameof(group), group, null);
        }
    }

    private static string GetElementText(CharGroup group, string existingText)
    {
        switch (group)
        {
            case CharGroup.Letter:
                return "LTR";
            
            case CharGroup.Number:
                return "NUM";
            
            case CharGroup.Punctuation:
                return "PUN";
            
            case CharGroup.Symbol:
                return "SYM";
            
            case CharGroup.Mark:
                return "MRK";

            case CharGroup.Separator:
                return "SEP";
            
            case CharGroup.Special:
                return "SPC";


            case CharGroup.NotAssigned:
                return "NA";
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }


    private string GetStyleText(CharGroup group)
    {
        switch (group)
        {
            case CharGroup.Letter:
                return TextElementViewModel.GetStyleText(
                    TextElementViewModel.StyleColors.LimeGreen,
                    TextElementViewModel.StyleColors.Navy);
            
            case CharGroup.Mark:
                return TextElementViewModel.GetStyleText(
                    TextElementViewModel.StyleColors.Orange,
                    TextElementViewModel.StyleColors.CornflowerBlue);
            
            case CharGroup.Number:
                return TextElementViewModel.GetStyleText(
                    TextElementViewModel.StyleColors.Aquamarine,
                    TextElementViewModel.StyleColors.LightGray);
            
            case CharGroup.Separator:
                return TextElementViewModel.GetStyleText(
                    TextElementViewModel.StyleColors.Yellow,
                    TextElementViewModel.StyleColors.CornflowerBlue);
            
            case CharGroup.Special:
                return TextElementViewModel.GetStyleText(
                    TextElementViewModel.StyleColors.PaleGreen,
                    TextElementViewModel.StyleColors.DarkGray);
            
            case CharGroup.Punctuation:
                return TextElementViewModel.GetStyleText(
                    TextElementViewModel.StyleColors.OrangeRed,
                    TextElementViewModel.StyleColors.LightGray);
            
            case CharGroup.Symbol:
                return TextElementViewModel.GetStyleText(
                    TextElementViewModel.StyleColors.Aquamarine,
                    TextElementViewModel.StyleColors.DarkCyan);
            
            case CharGroup.NotAssigned:
                return TextElementViewModel.GetStyleText(
                    TextElementViewModel.StyleColors.LightGray,
                    TextElementViewModel.StyleColors.DarkRed);
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    
}