using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOVE : MonoBehaviour
{
    [SerializeField]
    float speed;

    float time;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += speed * Time.deltaTime;
        var value = Mathf.Sin(time);
        //var temp = .2f * value;
       // var temp = width * value;
    }
}
