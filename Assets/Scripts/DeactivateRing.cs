using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateRing : MonoBehaviour
{
    //bool expand;
    //float temp;
 

    [SerializeField]
    private GameObject player;

    private void OnEnable()
    {
        GeneralVariable.currentPlayerInfo += CurrentPlayerInfo;
    }
    //private void Start()
    //{
    //    temp = transform.localScale.x;
    //    expand = false;
    //}

    // Update is called once per frame
    void Update()
    {
        if (player!=null)
        {
            var distance = transform.position.y - player.transform.position.y;
            //print(distance);
            if (distance > 0 && distance > 10)
            {
                gameObject.SetActive(false);
            }
        }   
        

        //if(expand)
        //{
        //    temp += (5f* Time.deltaTime);
        //    transform.localScale = Vector3.one * temp;
        //}
    }

    void CurrentPlayerInfo(GameObject player)
    {
        this.player = player;
    }

    //public void DeactivateGameObject() //disable disableGameobject
    //{
    //    expand = true;
    //    Invoke("Delay", .5f);
    //}

    //void Delay()
    //{
    //    this.gameObject.SetActive(false);
    //}
 
    private void OnDisable()
    {
        GeneralVariable.currentPlayerInfo -= CurrentPlayerInfo;
    }
}
