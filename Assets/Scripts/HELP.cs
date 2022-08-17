using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HELP : MonoBehaviour
{
    [SerializeField]
    float speed;
    float originalSize;
    Vector3 size;
    float time;
    [SerializeField] bool stop;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        originalSize = transform.localScale.x;
        size = transform.localScale;
        if (!stop)
            StartCoroutine(Stop());
    }

    // Update is called once per frame
    void Update()
    {
        time += speed * Time.deltaTime;
        var value = Mathf.Sin(time);
        var modifier = value * .08f;
        //transform.localScale = Vector3.one * (originalSize-modifier);
        transform.localScale = new Vector3(size.x+modifier, size.y+modifier, size.z+modifier);
    }
    IEnumerator Stop()
    {
        yield return new WaitForSeconds(15f);
       // gameObject.SetActive(false);
    }
}
