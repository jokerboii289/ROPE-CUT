using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private Transform pointToMove;
    [SerializeField]float speed;
    NavMeshAgent agent;
    Transform enemy=null;
    Animator animator;
    bool stop;

    // Start is called before the first frame update
    void Start()
    {
        stop = false;
        if (GameObject.FindGameObjectWithTag("boss") != null)
        {
            enemy = GameObject.FindGameObjectWithTag("boss").transform;
            pointToMove = GeneralVariable.instance.points[Random.Range(0, GeneralVariable.instance.points.Length)];
            BossDeathState.gameOver += DanceAnimation;
        }
        else
        {
            pointToMove = GameObject.FindGameObjectWithTag("restPoint").transform;
        }        
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy != null)
        {
            if ((enemy.position - transform.position).magnitude > 2 && !stop)
            {
                agent.SetDestination(pointToMove.position);
            }
            else
            {
                animator.SetBool("kick", true);
                var direction = (enemy.transform.position - transform.position).normalized;
                transform.forward = direction;
            }
        }
        else
        {
            if((pointToMove.position-transform.position).magnitude>2)
            {
                agent.SetDestination(pointToMove.position);
            }
            else if((pointToMove.position - transform.position).magnitude<2)
            {
                //change expression               
                print("stop");
                animator.SetBool("dance", true);
            }
        }
    }

    void DanceAnimation()
    {
        animator.SetBool("dance", true);
    }
    
    private void OnDestroy()
    {   
        if (enemy != null)
        {
            stop = true;
           // BossDeathState.gameOver -= DanceAnimation;
        }
        BossDeathState.gameOver -= DanceAnimation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("restPoint"))
        {
            PauseMenu.instance.GameOver();
            other.GetComponent<Collider>().enabled = false;
        }
    }

    public void KickSound()
    {        
        var rand = Random.Range(0, 7);
        if(rand==0)
        {
            AudioManager.instance.Play("kick");
        }
        if (rand == 1)
        {
            AudioManager.instance.Play("kick2");
        }
        if (rand == 2)
        {
            AudioManager.instance.Play("kick3");
        }
        if (rand == 3)
        {
            AudioManager.instance.Play("kick4");
        }
        if (rand == 4)
        {
            AudioManager.instance.Play("kick5");
        }
    }
}
