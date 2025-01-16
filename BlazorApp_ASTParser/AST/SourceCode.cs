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

using System.Collections;

namespace BlazorApp_ASTParser.AST;

public class SourceCode
{
    private readonly Lazy<string[]> _lines;


    public SourceCode(string sourceCode)
    {
        Contents = sourceCode;
        _lines = new Lazy<string[]>(() => Contents.Split(new[] { Environment.NewLine }, StringSplitOptions.None));
    }

    public string Contents { get; }

    public string[] Lines => _lines.Value;

    public char this[int index] => Contents.CharAt(index);

    public string GetLine(int line)
    {
        if (line < 1)
        {
            throw new IndexOutOfRangeException($"{nameof(line)} must not be less than 1!");
        }

        if (line > Lines.Length)
        {
            throw new IndexOutOfRangeException($"No line {line}!");
        }

        // Lines start at 1; array indexes start at 0.
        return Lines[line - 1];
    }

    public string[] GetLines(int start, int end)
    {
        if (end < start)
        {
            throw new IndexOutOfRangeException("Cannot retrieve negative range!");
        }

        if (start < 1)
        {
            throw new IndexOutOfRangeException($"{nameof(start)} must not be less than 1!");
        }

        if (end > Lines.Length)
        {
            throw new IndexOutOfRangeException("Cannot retrieve more lines than exist in file!");
        }

        // Line indexes are offset by +1 compared to array indexes.
        return new Subset<string>(Lines, start - 1, end - 1).ToArray();
    }

    public string GetSpan(SourceSpan span)
    {
        var start = span.Start.Index;
        var length = span.Length;
        return Contents.Substring(start, length);
    }

    public string GetSpan(SyntaxNode node)
    {
        return GetSpan(node.SourceSpan);
    }

    private class Subset<T>(IEnumerable<T> collection, int start, int end) : IEnumerable<T>
    {
        private readonly int _end = end;
        private readonly IEnumerable<T> _set = collection;

        private readonly int _start = start;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new SubsetEnumerator(this);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new SubsetEnumerator(this);
        }

        private struct SubsetEnumerator(Subset<T> subset) : IEnumerator<T>
        {
            private bool _disposed = false;
            private int _index = subset._start - 1; // MoveNext() appears to be called before get_Current.

            public T Current => subset._set.ElementAt(_index);

            object IEnumerator.Current => subset._set.ElementAt(_index) ?? throw new IndexOutOfRangeException();

            public void Dispose()
            {
                _disposed = true;
            }

            public bool MoveNext()
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException("SubsetEnumerator");
                }

                if (_index == subset._end)
                {
                    return false;
                }

                _index++;
                return true;
            }

            public void Reset()
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException("SubsetEnumerator");
                }

                _index = subset._start;
            }
        }
    }
}