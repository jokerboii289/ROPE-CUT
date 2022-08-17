using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GeneralVariable : MonoBehaviour
{
    public float playerHangingSpeed;
    public float forceOnchainOfPlayers;

    public GameObject destructible;
    //event to send current player info
    public delegate void CurrentPlayerInfo(GameObject playerinfo);
    public static event CurrentPlayerInfo currentPlayerInfo;
    private string playerCurrentTag;

    //map the color with material;
    [Header("materials of player")]
    public List<MaterialOfPlayer> materialOfPlayer = new List<MaterialOfPlayer>();
    private Material updateMaterial;


    /// ______________________________________________
    /// </summary>
    [Header("color tags")]
   // public string[] colorTags;

    [Header("bubble")]
    public float bubbleMoveSpeed;

    public static GeneralVariable instance;
    [Header("with script")]
    public GameObject character;
    [Header("without script")]
    public GameObject Character_leftHang, Character_RightHang;


    /// <summary>
    /// spawning chain of character
    /// </summary>
    /// 
   
    [SerializeField]
    private List<GameObject> ChainOFCharacter = new List<GameObject>();
    //current player
    [SerializeField]
    [HideInInspector] private GameObject currentParentCharacter;
    private int updatedNoOfCharacter;

    [Header("characterToSpawnOnGround")]
    public GameObject groundCharacter;
    public GameObject bossCam;/* cam2,cam3*/
    public Transform[] points; 


    [Header("ParticleEffect")]
    public GameObject ropeBreak;
    public GameObject bubblePop;
    //public GameObject destroyPlayer; // when enteres the wrong ring

    [Header("BrokenRope")]
    public GameObject ropePoints;
    public GameObject ringDisapper;
    public GameObject playerSplat;

   
    private void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        
        updatedNoOfCharacter = 1;
        bossCam.SetActive(false);
        //StartCoroutine(DelayCam());

        instance = this;
        currentParentCharacter = GameObject.FindGameObjectWithTag("Player"); //current player character hanging oon the rope
        print(currentParentCharacter);
        if (currentPlayerInfo != null)   // sending player info
            currentPlayerInfo(currentParentCharacter);

        playerCurrentTag = currentParentCharacter.transform.GetChild(currentParentCharacter.transform.childCount - 1).tag;// storing current player tag

        Debug.Log(playerCurrentTag);

        ChainOFCharacter.Add(currentParentCharacter);
    }

    //IEnumerator DelayCam()
    //{
    //    yield return new WaitForSeconds(2.5f);
    //    bossCam.SetActive(false);
    //    yield return new WaitForSeconds(1.5f);
    //    //cam2.SetActive(false);

    //    yield return new WaitForSeconds(.5f);
    //    //cam3.SetActive(false);
    //}

    public void SpawnCharacter(Rigidbody rbody, Transform tempPos,Vector3 pos) //spawning character after entering the ring
    {
        var temp=Instantiate(character, pos, Quaternion.Euler(0,180,0));
    }

    public void SpawnChainOfCharacter(int noOfCharacter,bool multiply,string playerColorTag)
    {
        StartCoroutine(Delay(noOfCharacter,multiply,playerColorTag));
    }
    IEnumerator Delay(int noOfCharacter,bool multiply,string playerColorTag)
    {
        if(noOfCharacter>=0)
            AudioManager.instance.Play("characterAdd");
        else
            AudioManager.instance.Play("characterDeduct");

        yield return new WaitForSeconds(0);
        //for this function add the character on spawnPoint of each last character of stack
   
        GameObject tempCharacter=null;
        int count = 3;
       

        foreach (GameObject x in ChainOFCharacter)
        {
            x.SetActive(false);
        }

        ChainOFCharacter.Clear();
        currentParentCharacter = GameObject.FindGameObjectWithTag("Player"); //current player character hanging oon the rope
        ChainOFCharacter.Add(currentParentCharacter);
     
        print(currentParentCharacter.transform.childCount);

        currentParentCharacter.transform.GetChild(currentParentCharacter.transform.childCount-1).tag = playerColorTag;
        //color change for player
        foreach (MaterialOfPlayer x in materialOfPlayer)
        {
            if (x.colorTagName == playerColorTag)
            {
                updateMaterial = x.material;
                print(updateMaterial.name);
                currentParentCharacter.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material = updateMaterial;
            }
        }


        if (currentPlayerInfo != null)   // sending player info
            currentPlayerInfo(currentParentCharacter);


        //updating characterNo
        if (!multiply)
            updatedNoOfCharacter += noOfCharacter;   //addition
        else
            updatedNoOfCharacter = updatedNoOfCharacter * noOfCharacter;//multiplication

 

        for (int i = 0; i < updatedNoOfCharacter-1; i++)
        {
            if (count % 2 == 0)
                tempCharacter = Character_leftHang;
            else
                tempCharacter = Character_RightHang;

            var rbody = ChainOFCharacter[ChainOFCharacter.Count - 1].GetComponent<Rigidbody>();
            var tempObjHolder = Instantiate(tempCharacter, ChainOFCharacter[ChainOFCharacter.Count - 1].transform.GetChild(1).position, Quaternion.Euler(0,180,0));

            tempObjHolder.GetComponent<HingeJoint>().connectedBody = rbody;

            ChainOFCharacter.Add(tempObjHolder);
            //color change during special ring
            Debug.Log(playerColorTag);
            foreach (MaterialOfPlayer x in materialOfPlayer)
            {
                if (x.colorTagName == playerColorTag)
                {
                    updateMaterial = x.material;
                    print(updateMaterial.name);
                    tempObjHolder.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material = updateMaterial;
                }
            }
           // tempObjHolder.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material = updateMaterial;

            count++;
        }

        foreach (GameObject x in ChainOFCharacter)
        {
            var rBody = x.GetComponent<Rigidbody>();
            rBody.AddForceAtPosition( forceOnchainOfPlayers* transform.right, rBody.centerOfMass, ForceMode.Impulse);// default 3
        }

        //send no of character to camera
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>().GetSize(ChainOFCharacter.Count);
    }

    public void RemoveCharcterJoint()
    {
        foreach (GameObject x in ChainOFCharacter)
        {
            StartCoroutine(Delay(x));
            Destroy( x.GetComponent<HingeJoint>());
        }
    }
    IEnumerator Delay(GameObject x)
    {
        yield return new WaitForSeconds(4);
        x.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void InstantiateCharacterOnGround()///GROUND CHARCTER SPAWN
    {

        foreach (GameObject x in ChainOFCharacter)
        {
            var temp = Instantiate(groundCharacter, x.transform.position, Quaternion.identity);

            //checking color of player
            var parentobjColorTag = ChainOFCharacter[0].transform.GetChild(ChainOFCharacter[0].transform.childCount - 1).tag;
            print(parentobjColorTag);
            foreach (MaterialOfPlayer obj in materialOfPlayer)
            {
                if (obj.colorTagName == parentobjColorTag)
                {
                    updateMaterial = obj.material;
                    temp.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material = updateMaterial;

                    //PLAYER SPLAT PARTICLE UPDADTE
                    playerSplat = obj.playerSplat; // for material change of ground character
                }
            }
            x.SetActive(false);
        }
        bossCam.SetActive(true);
        //  StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        foreach (GameObject x in ChainOFCharacter)
        {
            var temp = Instantiate(groundCharacter, x.transform.position, Quaternion.identity);

            //checking color of player
            var parentobjColorTag = ChainOFCharacter[0].transform.GetChild(ChainOFCharacter[0].transform.childCount - 1).tag;
            print(parentobjColorTag);
            foreach (MaterialOfPlayer obj in materialOfPlayer)
            {
                if (obj.colorTagName == parentobjColorTag)
                {
                    updateMaterial = obj.material;
                    temp.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material = updateMaterial;

                    //PLAYER SPLAT PARTICLE UPDADTE
                    playerSplat = obj.playerSplat; // for material change of ground character
                }
            }
            x.SetActive(false);
        }
        bossCam.SetActive(true);
    }

    public void AddCharacter(Vector3 effectPos)// bubble character add
    {
        
        Instantiate(GeneralVariable.instance.bubblePop, effectPos, Quaternion.identity);
        var tempObjHolder = Instantiate(Character_leftHang, ChainOFCharacter[ChainOFCharacter.Count - 1].transform.GetChild(1).position, ChainOFCharacter[ChainOFCharacter.Count - 1].transform.rotation);
        var rbody = ChainOFCharacter[ChainOFCharacter.Count - 1].transform.GetChild(1).GetComponent<Rigidbody>();
        var hinge = tempObjHolder.GetComponent<HingeJoint>();
        hinge.connectedBody = rbody;

        tempObjHolder.GetComponent<Rigidbody>().velocity = ChainOFCharacter[ChainOFCharacter.Count - 1].GetComponent<Rigidbody>().velocity;//passing the velocity to the new chain member(dummy character)
        //change color of flying character when caught by player
        var parentobjColorTag = ChainOFCharacter[0].transform.GetChild(ChainOFCharacter[0].transform.childCount - 1).tag;
        print(parentobjColorTag);
        foreach (MaterialOfPlayer x in materialOfPlayer)
        {
            if (x.colorTagName == parentobjColorTag)
            {
                updateMaterial = x.material;
                print(updateMaterial.name);
                tempObjHolder.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material = updateMaterial;
            }
        }

        ChainOFCharacter.Add(tempObjHolder);
        updatedNoOfCharacter += 1;
        //expression
        var expression = tempObjHolder.GetComponent<Expression>();

        StartCoroutine(Delay(expression));

        //send no of character to camera
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>().GetSize(ChainOFCharacter.Count);
    }

    IEnumerator Delay(Expression expression)
    {
        yield return new WaitForSeconds(0);
        expression.happyExpression.SetActive(false);
        expression.afraidExpression.SetActive(true);
    }

    public void DestroyTheCharacters()//when enters the wrong ring
    {
        GameObject particleType=null;
        //TYPE OF PARTICLE EFFECT
        var parentobjColorTag = ChainOFCharacter[0].transform.GetChild(ChainOFCharacter[0].transform.childCount - 1).tag;
        print(parentobjColorTag);
        foreach (MaterialOfPlayer x in materialOfPlayer)
        {
            if (x.colorTagName == parentobjColorTag)
            {
                particleType = x.destroyPlayerParicleEffect;
                //print(updateMaterial.name);
            }
        }

        if (particleType != null)
        {
            foreach (GameObject x in ChainOFCharacter)
            {
                x.SetActive(false);
                //Instantiate(GeneralVariable.instance.destroyPlayer, x.transform.position, Quaternion.identity);
                Instantiate(particleType, x.transform.position, Quaternion.identity);
            }
        }
    }


    public void AddGravity()
    {
        foreach(GameObject x in ChainOFCharacter)
        {
            //physcocs gravity change
           // Physics.gravity = new Vector3(0,-16,0);
        }
    }
}
