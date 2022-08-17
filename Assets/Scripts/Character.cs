using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Character : MonoBehaviour
{
    bool ropeCut;

    //change expression
    public delegate void ExpressionChange();
    public static event ExpressionChange expressionChange;

    [SerializeField]
    private string playerTag;
    private string tempTag;
    
    public Transform holdPoint,anchorPoint;

    public LineRenderer lineRenderer;
 

    public Transform[] points = new Transform[2]; 

    bool enableLineRender;

    bool canSpawn;

    Transform knife;
  

    Transform spawnPoint;
    bool secondCamActive;
    Camera cam;
    Vector3 position;
    

    private void Start() 
    {
        ropeCut = false;
        playerTag = transform.GetChild(transform.childCount - 1).tag;
        cam = GameObject.FindGameObjectWithTag("rootcam").GetComponent<Camera>();
        secondCamActive = false;
        canSpawn = false;
        enableLineRender = true;
        points[0] = holdPoint;

        anchorPoint = GameObject.FindGameObjectWithTag("anchor").transform;
        var characterHinge = GetComponent<HingeJoint>();
        characterHinge.connectedBody = anchorPoint.GetComponent<Rigidbody>();
        characterHinge.anchor = transform.InverseTransformPoint(anchorPoint.position);

        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>().cameraPivot = anchorPoint;

        StartCoroutine(Delay());

        knife = GameObject.FindGameObjectWithTag("knife").transform;
       
        position = Vector3.zero;
    }

    private void Update()
    {
        if (playerTag != transform.GetChild(transform.childCount - 1).tag) //recheck if the player character script holds the right tag
            playerTag = transform.GetChild(transform.childCount - 1).tag;
        
        //print(playerTag);

        ///_____________________________
        LineRenderer();

        if(Input.touchCount>0)
        {
            Touch touch = Input.GetTouch(0);

            var offset =   transform.position.z- cam.transform.position.z;
            if (touch.phase == TouchPhase.Began)
            {
                knife = GameObject.FindGameObjectWithTag("knife").transform;
                knife.GetComponent<Collider>().enabled = true;
                
                position = cam.ScreenToWorldPoint(new Vector3(touch.position.x,touch.position.y,offset));
                position = new Vector3(position.x,position.y, transform.position.z) ;
                knife.position = position;

                knife.GetComponent<TrailRenderer>().enabled = true;

            }
            if(touch.phase == TouchPhase.Moved)
            {
                position = cam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, offset));
                position = new Vector3(position.x, position.y, transform.position.z);
                knife.position = position;
            }
            if(touch.phase==TouchPhase.Ended)
            {
                knife.GetComponent<Collider>().enabled = false;
                knife.GetComponent<TrailRenderer>().enabled = false;

                DeactivateLineRender(); //rope deactivate
                ropeCut = true;
            }
        }

        if (ropeCut)
            GeneralVariable.instance.AddGravity();
    }
 
    public void DeactivateLineRender()//rope
    {
        //test
        var point = GameObject.FindGameObjectWithTag("anchor").transform.GetChild(1).GetChild(0).position;
        print(GameObject.FindGameObjectWithTag("anchor").transform.GetChild(1).name);
        Instantiate(GeneralVariable.instance.ropeBreak,point, Quaternion.identity);
        anchorPoint.GetComponent<AnchorHangingRope>().cutPoint = point;
        Vibration.Vibrate(30);  //vibrate

        //pass the the knife position to the anchor
        //anchorPoint.GetComponent<AnchorHangingRope>().cutPoint = knife.position;
        //Effect();
        
        AudioManager.instance.Play("swipe");

        //change expression change
        if (expressionChange != null)
            expressionChange();

        GetComponent<HingeJoint>().breakForce = 1;
        enableLineRender = false;
        points[1] = points[0]; //holds the same hold point value
    
        anchorPoint.gameObject.GetComponent<AnchorHangingRope>().enabled=true;

        if (GameObject.FindGameObjectsWithTag("Ring").Length == 0)
        {
            StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>().RingDisappeared());
            GeneralVariable.instance.RemoveCharcterJoint();
        }
    }


    void LineRenderer()
    {
        if (enableLineRender)
        {
            points[0] = holdPoint;
            points[1] = anchorPoint;
        }
        lineRenderer.positionCount = points.Length; 
        for (int i = 0; i < points.Length; i++)
        {
            lineRenderer.SetPosition(i, points[i].position);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            anchorPoint.gameObject.SetActive(false);
            //instantiate
            GeneralVariable.instance.InstantiateCharacterOnGround();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("destructible"))
        {
            Instantiate(GeneralVariable.instance.destructible, other.transform.position, Quaternion.identity);
            other.transform.root.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("spawnAnchor") && canSpawn)
        {
            anchorPoint.gameObject.SetActive(false); //deactivate anchor Point

            var parentObj = other.transform.root.gameObject;//ring transform

            var characterPoint = parentObj.transform.GetChild(2).transform.position;

            var anchor = parentObj.transform.GetChild(0).gameObject;

            anchor.SetActive(true);

            enableLineRender = true;
            anchor.transform.SetParent(null);

            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>().cameraPivot = anchor.transform;

            var rbody = anchor.GetComponent<Rigidbody>();//rbody of new anchor

            Instantiate(GeneralVariable.instance.ringDisapper, parentObj.transform.position, Quaternion.Euler(-90, 0, 0));  //ring disappear effect

            //DEAACTIVATE loose COLLIDER in adjacent rings
            var obj = parentObj.transform.GetChild(0).GetComponent<Anchor>().looseCollider;
            if (obj != null)
                obj.SetActive(false);
            //Debug.Log(parentObj.transform.GetChild(0));

            parentObj.SetActive(false);//ring deactivate
            //parentObj.GetComponent<DeactivateRing>().DeactivateGameObject();

            GeneralVariable.instance.SpawnCharacter(rbody, anchor.transform, characterPoint);

            var temp = other.GetComponent<Anchor>();

            if (parentObj.transform.GetChild(parentObj.transform.childCount - 1).tag == "randomcolor")
                tempTag = parentObj.transform.GetChild(parentObj.transform.childCount - 1).GetComponent<RandomColorTag>().ringCurrentColorTag;
            else
                tempTag = playerTag;

            GeneralVariable.instance.SpawnChainOfCharacter(temp.noOfCharacter, temp.multiply, tempTag);//spawning player function call


            gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("collectables"))
        {
            GeneralVariable.instance.AddCharacter(other.transform.position);
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("loose")) //loose condition
        {
            PauseMenu.instance.Loose();
        }

        if (other.gameObject.CompareTag("ringEnter"))
        {
            canSpawn = true;
            spawnPoint = other.transform;

            //color check of ring and comparisson
            var root = other.transform.root;
            var temp = root.GetChild(root.childCount - 1);
            //print(temp.tag);
            if (temp.CompareTag(playerTag) || temp.CompareTag("randomcolor"))
            {
                print("detect");
            }
            else
            {
                PauseMenu.instance.Loose();
                GeneralVariable.instance.DestroyTheCharacters();
            }
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0);
        var rBody = GetComponent<Rigidbody>();
        rBody.AddForceAtPosition(GeneralVariable.instance.playerHangingSpeed*transform.right,rBody.centerOfMass,ForceMode.Impulse);//default 1
    }

    void Effect()
    {
        Instantiate(GeneralVariable.instance.ropeBreak, knife.position, Quaternion.identity);
    }
}
