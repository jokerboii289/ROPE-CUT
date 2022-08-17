using UnityEngine;

public abstract class Boss  //base State
{
    public abstract void EnterState(BossStateManager state , Animator animator);

    public abstract void UpdateState(BossStateManager state);

    public abstract void OnDeathState(BossStateManager state);
    
}
