using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PagesManagment;

namespace MIPTCore.Models.ComplexMappers
{
    public static class PageTreeMapper
    {
        public static PageTreeModel PageTreeToModel(PageTreeNode tree)
        {
            return tree == null 
                ? null 
                : TraverseNode(tree);
        }

        private static PageTreeModel TraverseNode(PageTreeNode tree)
        {
            var modelOfThisLevel = new PageTreeModel
            {
                Id = tree.Id,
                PageName = tree.Path ?? "",
                Nodes = new List<PageTreeModel>()
            };
            foreach (var node in tree.Nodes)
            {
                modelOfThisLevel.Nodes.Add(TraverseNode(node.Value));
            }
            return modelOfThisLevel;
        }
    }
}