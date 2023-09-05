using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeRunner : MonoBehaviour
{
    BehaviorTree tree;

    // Start is called before the first frame update
    void Start()
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
    void Update()
    {
        tree.Update();
    }
}
