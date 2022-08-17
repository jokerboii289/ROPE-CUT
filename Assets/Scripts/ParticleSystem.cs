using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyParticle());
    }

    IEnumerator DestroyParticle()
    {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
}
