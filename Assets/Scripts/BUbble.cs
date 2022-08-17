using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BUbble : MonoBehaviour
{
    [SerializeField]bool horizontal;

    [SerializeField] float width ,speed;
    float time;
    float yPos;
    float xpos;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        yPos = transform.position.y;
        xpos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        time += speed * Time.deltaTime;
        var value = Mathf.Sin(time);
        //var temp = .2f * value;
        var temp = width * value;
  
        if(!horizontal)
            transform.position = new Vector3(transform.position.x, yPos + temp, transform.position.z);
        else
            transform.position = new Vector3(xpos+temp, transform.position.y, transform.position.z);
    }
}
