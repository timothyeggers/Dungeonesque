using Dungeonesque.BehaviorTree;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using Node = Dungeonesque.BehaviorTree.Node;

public class BehaviorTreeView : GraphView
{
    private BehaviorTree tree;

    public BehaviorTreeView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviorTreeEditor.uss");
        styleSheets.Add(styleSheet);
    }

    internal void PopulateView(BehaviorTree behaviorTree, BehaviorTree tree)
    {
        this.tree = tree;

        DeleteElements(graphElements);

        tree.nodes.ForEach(node => CreateNodeView(node));
    }

    private void CreateNodeView(Node node)
    {
    }

    public new class UxmlFactory : UxmlFactory<BehaviorTreeView, UxmlTraits>
    {
    }
}