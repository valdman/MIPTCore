using System;
using System.Collections.Generic;

namespace PagesManagment
{
    public class PageTreeNode
    {
        public IDictionary<string, PageTreeNode> Nodes { get; private set; }

        public string Path { get; private set; }

        public PageTreeNode()
        {
            Nodes =
            new Dictionary<string, PageTreeNode>();
        }
        
        public void AddPath(string path)
        {
            var charSeparators = new char[] {'/'};

            // Parse into a sequence of parts.
            var parts = path.Split(charSeparators, 
                StringSplitOptions.RemoveEmptyEntries);

            // The current node.  Start with this.
            var current = this;

            // Iterate through the parts.
            foreach (var part in parts)
            {
                // The child node.

                // Does the part exist in the current node?  If
                // not, then add.
                if (!current.Nodes.TryGetValue(part, out var child))
                {
                    //todo: Throw if trying to create sequence of leaf
                    
                    // Add the child.
                    child = new PageTreeNode {
                        Path = part
                    };
                  
                    // Add to the dictionary.
                    current.Nodes[part] = child;
                }

                // Set the current to the child.
                current = child;
            }
        }

    }
}