using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColorTag : MonoBehaviour
{
    float time;
    public string ringCurrentColorTag;
    // Start is called before the first frame update
    void Start()
    {
        time = 3;
    }

    // Update is called once per frame
    void Update()
    {
        time += 1 * Time.deltaTime;
        if(time>3)
        {
            var colorIndex=Random.Range(0,GeneralVariable.instance.materialOfPlayer.Count);// int value
            ringCurrentColorTag = GeneralVariable.instance.materialOfPlayer[colorIndex].colorTagName; //color name
            time = 0;
        }
       // print(ringCurrentColorTag);
    }
}
