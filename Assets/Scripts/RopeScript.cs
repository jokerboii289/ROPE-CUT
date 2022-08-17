using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour
{
    [SerializeField]
    private GameObject player=null;

    private void OnEnable()
    {
        GeneralVariable.currentPlayerInfo += CurrentPlayer;
    }
   
    // Update is called once per frame
    void Update()
    {
        if(player!=null)
            transform.right = player.transform.right;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("knife"))
        {         
            GameObject.FindGameObjectWithTag("Player").GetComponent<Character>().DeactivateLineRender();
        }
    }

    void CurrentPlayer(GameObject player)
    {
        this.player = player;
    }

    private void OnDisable()
    {
        GeneralVariable.currentPlayerInfo -= CurrentPlayer;
    }
}
