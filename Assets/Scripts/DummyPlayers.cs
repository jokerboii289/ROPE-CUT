using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlayers : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("collectables"))
        {
            GeneralVariable.instance.AddCharacter(other.transform.position);
            other.gameObject.SetActive(false);
        }
    }
}
