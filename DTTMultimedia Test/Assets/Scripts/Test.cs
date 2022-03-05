using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject);
        
        if(other.isTrigger == false)
        {
            print("A Collider touched me ;-;");
        }
        else
        {
            print("A Trigger touched me ;-;");
        }
    }
}
