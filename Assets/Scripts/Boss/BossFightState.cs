using UnityEngine;
using UnityEngine.EventSystems;

public class BossFightState : Boss
{
    int count;
    float healthpiece;
    float originalHealth;
    float hpPoint;
    int noOFEnemyRequired=10; 
    public override void EnterState(BossStateManager state, Animator animator)
    {
        hpPoint = (int)(state.HealthBar.fillAmount*100); //set hp point
        state.healthNo.text = hpPoint.ToString();
        animator.SetBool("fight", true);
        count = 0;
        originalHealth = state.HealthBar.fillAmount;
        healthpiece = state.HealthBar.fillAmount /6;
    }
    public override void UpdateState(BossStateManager state)
    {
        if (Input.touchCount > 0 && state.tap)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                count++;
                if (count > 2)
                {
                    originalHealth -= healthpiece;
                }
            }
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                if (count > 2)
                    count = 0;               
            }
        }      

        //smoothing
        state.HealthBar.fillAmount = Mathf.Lerp(state.HealthBar.fillAmount,originalHealth,1*Time.deltaTime);
        hpPoint = (int)(state.HealthBar.fillAmount * 100); //set hp point
        state.healthNo.text = hpPoint.ToString();

        if ( hpPoint <= 0)
            state.SwitchState(state.death);
    }
    public override void OnDeathState(BossStateManager state)
    {
        
    }
}
