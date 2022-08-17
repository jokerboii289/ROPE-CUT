using UnityEngine;

public class BossIdleState : Boss
{
    public override void EnterState(BossStateManager state, Animator animator)
    {
      // Debug.Log("hello");
    }
    public override void UpdateState(BossStateManager state)
    {
       // Debug.Log("print");
    }
    public override void OnDeathState(BossStateManager state)
    {

    }
}
