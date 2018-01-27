using System;
using System.Collections.Generic;
using System.Linq;

namespace PagesManagment
{
    public class PageTreeNode
    {
        public IDictionary<string, PageTreeNode> Nodes { get; private set; }

        public string Path { get; private set; }

        public int Id { get; set; }

        public PageTreeNode()
        {
            Nodes =
            new Dictionary<string, PageTreeNode>();
        }
        
        public void AddPath(int id, string path)
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
                    //todo: Throw if trying To create sequence of leaf
                    
                    // Add the child.
                    child = new PageTreeNode {
                        Path = part
                    };
                  
                    // Add To the dictionary.
                    current.Nodes[part] = child;
                }

                // Set the current To the child.
                current = child;
            }
            
            SetIdForPath(id, path);
        }

        private void SetIdForPath(int id, string path)
        {
            var charSeparators = new char[] {'/'};
            
            var parts = path.Split(charSeparators, 
                StringSplitOptions.RemoveEmptyEntries);

            var nodeToChange = this;

            nodeToChange = parts.Aggregate(nodeToChange, (current, part) => current.Nodes[part]);

            nodeToChange.Id = id;
        }

    }
}