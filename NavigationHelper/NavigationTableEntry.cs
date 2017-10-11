using System;
using Common;

namespace NavigationHelper
{
    public class NavigationTableEntry : AbstractIdentifyable
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public int Position { get; set; }
    }
}