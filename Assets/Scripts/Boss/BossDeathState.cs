
using UnityEngine;
using UnityEngine.EventSystems;

public class BossDeathState : Boss
{
    public delegate void GameOver();
    public static event GameOver gameOver;
    
    public override void EnterState(BossStateManager state, Animator animator)
    {
        state.GetComponent<Rigidbody>().isKinematic = true;
        state.GetComponent<Collider>().enabled = false;
        if(state.GetComponent<Animator>().runtimeAnimatorController!=null)
            animator.SetBool("death", true);
       
        if(gameOver!=null)
        {
            gameOver();
        }
    }
    public override void UpdateState(BossStateManager state)
    {        
       
    }
    public override void OnDeathState(BossStateManager state)
    {
       
    }
    
}
