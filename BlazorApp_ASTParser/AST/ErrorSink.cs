// BlazorApp_ASTParser -- ErrorSink.cs
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

using System.Collections;
using MudBlazor;

namespace BlazorApp_ASTParser.AST;

public class ErrorSink : IEnumerable<ErrorDetails>
{
    private readonly List<ErrorDetails> _errors = new();

    public IEnumerable<ErrorDetails> Errors => _errors;

    public bool HasErrors => _errors.Count > 0;

    public IEnumerator<ErrorDetails> GetEnumerator()
    {
        return _errors.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _errors.GetEnumerator();
    }

    public void AddError(string message, SourceCode sourceCode, ErrorSeverity severity, SourceSpan span)
    {
        _errors.Add(new ErrorDetails(message, sourceCode.GetLines(span.Start.Line, span.End.Line), severity, span));
    }

    public void Clear()
    {
        _errors.Clear();
    }
}