using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Anchor : MonoBehaviour
{
    public bool multiply;  
    public int noOfCharacter;

    [SerializeField]
    TextMeshPro ringValue;

    [Header("adjacent Collider")]
    public GameObject looseCollider=null;

    private void Start()
    {
        if (ringValue != null)
        {
            if (multiply)
            {
                ringValue.text = "x" + noOfCharacter.ToString();
            }
            else if(!multiply && noOfCharacter<0)
            {
                ringValue.text =  noOfCharacter.ToString();
            }
            else if(!multiply && noOfCharacter>0)
                ringValue.text = "+" + noOfCharacter.ToString();

            if(noOfCharacter==0)
                ringValue.text =  " ".ToString();
        }
    }
}
