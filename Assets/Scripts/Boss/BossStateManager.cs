using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class BossStateManager : MonoBehaviour
{
    static Animator animator;
    public Image HealthBar;
    public TextMeshProUGUI healthNo;
    public bool tap;
    public Transform pointDirection;//to add force during the player death
    public delegate void PlayerDead();
    public static event PlayerDead playerDead;

    Boss currentState;
    public BossIdleState idle = new BossIdleState();
    public BossFightState fight = new BossFightState();
    public BossDeathState death = new BossDeathState();

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentState = idle;
        currentState.EnterState(this, animator);
        tap = false;
    }

    private void Update()
    {
        if (animator == null)
            print("animator null"); 
        currentState.UpdateState(this);
    }

    public void SwitchState(Boss state)
    {
        //change current state here
        currentState=state;
        currentState.EnterState(this,animator);
    }
    
    public void PlayerCharactersDie()//make the character die during the fight with splashEffect
    {
        GameObject[] characters = GameObject.FindGameObjectsWithTag("GroundCharacter");
        print(characters.Length);
        for (int i = 0; i < Random.Range(1, 4); i++) // random no of player to get destroyed between 1-3
        {
            if (characters.Length > 0)
            {
                var obj = characters[Random.Range(0, characters.Length)];
                if ((obj.transform.position - transform.position).magnitude < 2.5f && obj != null)
                {
                    Instantiate(GeneralVariable.instance.playerSplat, obj.transform.position, Quaternion.identity);//PLAYER DEAD EFFECT AFTER FIGHTING BOSS
                    AudioManager.instance.Play("playerDeathSplash");
                    obj.SetActive(false);
                }
            }
        }

        if (GameObject.FindGameObjectsWithTag("GroundCharacter").Length == 0)
        {
            if(playerDead!=null)
                playerDead();
        }
    }

}
