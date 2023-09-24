using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dungeonesque.BehaviorTree
{
    [CreateAssetMenu]
    public class BehaviorTree : ScriptableObject
    {
        public Node rootNode;
        public Node.State treeState = Node.State.Running;

        public List<Node> nodes = new();

        public Node.State Update()
        {
            if (rootNode.state == Node.State.Running) treeState = rootNode.Update();
            return treeState;
        }

        public Node CreateNode(Type type)
        {
            var node = CreateInstance(type) as Node;
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();
            nodes.Add(node);

            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();
            return node;
        }

        public void DeleteNode(Node node)
        {
            nodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }
    }
}