using Dungeonesque.BehaviorTree;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BehaviorTreeEditor : EditorWindow
{
    [SerializeField] private VisualTreeAsset m_VisualTreeAsset;

    private InspectorView inspectorView;
    private BehaviorTreeView treeView;

    public void CreateGUI()

    {
        // Each editor window contains a root VisualElement object
        var root = rootVisualElement;

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);

        treeView = root.Q<BehaviorTreeView>();
        inspectorView = root.Q<InspectorView>();
    }

    private void OnSelectionChange()
    {
        var tree = Selection.activeObject as BehaviorTree;
        if (tree)
        {
            /* treeView.PopulateView(BehaviorTree tree);*/
        }
    }

    [MenuItem("Window/UI Toolkit/BehaviorTreeEditor")]
    public static void OpenWith()
    {
        var wnd = GetWindow<BehaviorTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviorTreeEditor");
    }
}