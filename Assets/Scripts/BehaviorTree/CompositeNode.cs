using System.Collections.Generic;

namespace Dungeonesque.BehaviorTree
{
    public abstract class CompositeNode : Node
    {
        public List<Node> children = new();
    }
}