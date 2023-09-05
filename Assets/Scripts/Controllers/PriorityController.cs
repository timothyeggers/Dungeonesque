using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityController : MonoBehaviour
{
    Dictionary<IPriority, float> priorityTargets = new Dictionary<IPriority, float>();

    public void SetPriority(IPriority sender)
    {
        Debug.Log($"Called from {sender}.");
        if (priorityTargets.ContainsKey(sender))
        {
            priorityTargets[sender] += sender.weight;
        }
        else
        {
            priorityTargets.Add(sender, sender.weight);
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            SetPriority(new DemoSoundTarget(3));
            SetPriority(new DemoSoundTarget(4));
        }

        var targets = new Dictionary<IPriority, float>(priorityTargets);
        foreach (var entry in targets)
        {
            priorityTargets[entry.Key] -= Time.deltaTime;
            if (priorityTargets[entry.Key] < 0 )
            {
                priorityTargets.Remove(entry.Key);
            }
            Debug.Log($"{entry.Key} : {entry.Value}");
        }
    }

}