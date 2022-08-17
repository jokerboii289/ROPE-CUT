using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCharacter : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            GetComponent<Animator>().SetBool("stand", true);
        }
    }

}
