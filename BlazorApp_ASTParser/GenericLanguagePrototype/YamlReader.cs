// BlazorApp_ASTParser -- YamlReader.cs
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

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace BlazorApp_ASTParser.GenericLanguagePrototype;

public class YamlReader
{
    // Dictionary to hold the parsed YAML data
    private static Dictionary<string, string> charDictionary;

    public static async Task<Dictionary<string, string>> ReadYaml(string yamlContent)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        charDictionary = deserializer.Deserialize<Dictionary<string, string>>(yamlContent);

        foreach (var entry in charDictionary)
        {
            Console.WriteLine($"{entry.Key}: {entry.Value}");
        }

        return charDictionary;
    }

    /// <summary>
    /// Get the key corresponding to a provided character.
    /// </summary>
    /// <param name="character">The character to look up.</param>
    /// <returns>The key if found, otherwise null.</returns>
    public static string GetKeyFromChar(char character)
    {
        foreach (var entry in charDictionary)
        {
            if (entry.Value == character.ToString())
            {
                return entry.Key;
            }
        }
        return null;
    }
}