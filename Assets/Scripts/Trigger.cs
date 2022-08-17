using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{                                     //Trigger for boss
    [SerializeField] GameObject help;

 
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("GroundCharacter"))
        {
            Boss state;

            state = GameObject.FindGameObjectWithTag("boss").GetComponent<BossStateManager>().fight;
            GameObject.FindGameObjectWithTag("boss").GetComponent<BossStateManager>().SwitchState(state);
            GameObject.FindGameObjectWithTag("boss").GetComponent<BossStateManager>().tap=true;
            if(help!=null)
                help.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
