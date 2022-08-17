using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorHangingRope : MonoBehaviour
{
    public Vector3 cutPoint;
    [SerializeField]
    private List<GameObject> ropeJOints = new List<GameObject>();
    LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        ropeJOints.Add(gameObject);
        lineRenderer = transform.GetChild(2).GetComponent<LineRenderer>();
        BrokenRope();
    }

    void BrokenRope()
    {
        var distance = transform.position.y - cutPoint.y;
        var yOffset = distance / 4;


        for(int i=0;i<5;i++)
        {
            var prefab = GeneralVariable.instance.ropePoints;
            if (prefab != null)
            {
                var temp= Instantiate(prefab, ropeJOints[i].transform.position - new Vector3(0, yOffset, 0),Quaternion.identity);
                var rb = ropeJOints[i].GetComponent<Rigidbody>();
                temp.GetComponent<HingeJoint>().anchor = temp.transform.InverseTransformPoint(ropeJOints[i].transform.position);

                temp.GetComponent<HingeJoint>().connectedBody = rb;
                temp.transform.SetParent(this.transform);
                ropeJOints.Add(temp);               
            }
        }

        //linerendere for rope
        lineRenderer.positionCount = ropeJOints.Count;
        for (int i = 0; i < ropeJOints.Count; i++)
        {
            lineRenderer.SetPosition(i, ropeJOints[i].transform.position);
        }

        //CHECK in which side the characater are in;
        var player = GameObject.FindGameObjectWithTag("Player").transform.position;
        var sign = Mathf.Sign( player.x- transform.position.x);
     
        foreach(GameObject x in ropeJOints)
        {           
            x.GetComponent<Rigidbody>().AddForce(transform.right * 3 *sign, ForceMode.Impulse);
        }
    }

    private void Update()
    {
        lineRenderer.positionCount = ropeJOints.Count;
        for (int i = 0; i < ropeJOints.Count; i++)
        {
            lineRenderer.SetPosition(i, ropeJOints[i].transform.position);
        }
    }

   

}
