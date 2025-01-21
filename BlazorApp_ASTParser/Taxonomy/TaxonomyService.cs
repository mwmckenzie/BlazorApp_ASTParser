// BlazorApp_ASTParser -- TaxonomyService.cs
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

namespace BlazorApp_ASTParser.Taxonomy;

public class TaxonomyService
{
    private TaxonomyNode root;

    // Constructor to initialize the taxonomy from a dictionary
    public TaxonomyService(Dictionary<string, object> taxonomy)
    {
        root = new TaxonomyNode("Root");
        BuildTree(taxonomy, root);
    }

    // Build the taxonomy tree recursively from a dictionary
    private void BuildTree(Dictionary<string, object> taxonomy, TaxonomyNode parent)
    {
        foreach (var entry in taxonomy)
        {
            var node = new TaxonomyNode(entry.Key) { Parent = parent };
            parent.Children.Add(node);

            if (entry.Value is Dictionary<object, object> childTaxonomy)
            {
                // Convert child taxonomy to a strongly typed dictionary
                var childDict = childTaxonomy.ToDictionary(
                    kvp => kvp.Key.ToString(),
                    kvp => kvp.Value
                );
                BuildTree(childDict, node);
            }
            else if (entry.Value == null)
            {
                // Treat null values as leaf nodes with no children
                continue;
            }
        }
    }

    // Get ancestors of a given node name
    public List<string> GetAncestors(string nodeName)
    {
        var node = FindNode(root, nodeName);
        if (node == null) return null;

        var ancestors = new List<string>();
        while (node.Parent != null)
        {
            ancestors.Add(node.Parent.Name);
            node = node.Parent;
        }

        ancestors.Reverse(); // Return ancestors in root-to-leaf order
        return ancestors;
    }

    // Find a node by name using depth-first search
    private TaxonomyNode FindNode(TaxonomyNode current, string nodeName)
    {
        if (current.Name == nodeName) return current;

        foreach (var child in current.Children)
        {
            var result = FindNode(child, nodeName);
            if (result != null) return result;
        }

        return null;
    }

    // Walk the tree and apply a callback function to each node
    public void WalkTree(Action<TaxonomyNode> callback)
    {
        WalkTreeRecursive(root, callback);
    }

    private void WalkTreeRecursive(TaxonomyNode current, Action<TaxonomyNode> callback)
    {
        callback(current);
        foreach (var child in current.Children)
        {
            WalkTreeRecursive(child, callback);
        }
    }
    
    // Helper to load taxonomy from YAML
    public static TaxonomyService LoadFromYaml(string yamlContent)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var taxonomy = deserializer.Deserialize<Dictionary<string, object>>(yamlContent);
        return new TaxonomyService(taxonomy);
    }
}