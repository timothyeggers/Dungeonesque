using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LookAtPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var player = FindObjectOfType<PlayerController>();
        transform.forward = player.transform.position - transform.position;
        transform.forward = transform.forward.normalized;
    }
}
