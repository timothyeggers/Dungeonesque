using System.Collections.Generic;
using UnityEngine;

namespace Dungeonesque.BehaviorTree
{
    public class BehaviorTreeRunner : MonoBehaviour
    {
        private BehaviorTree tree;

        // Start is called before the first frame update
        private void Start()
        {
            tree = ScriptableObject.CreateInstance<BehaviorTree>();

            var log = ScriptableObject.CreateInstance<DebugLogNode>();
            log.message = "Hello tree.";

            var timeOut = ScriptableObject.CreateInstance<WaitNode>();
            timeOut.duration = 2f;

            var at = ScriptableObject.CreateInstance<SequencerNode>();
            at.children = new List<Node> { log, timeOut };

            var loop = ScriptableObject.CreateInstance<RepeatNode>();
            loop.child = at;

            tree.rootNode = loop;
        }

        // Update is called once per frame
        private void Update()
        {
            tree.Update();
        }
    }
}