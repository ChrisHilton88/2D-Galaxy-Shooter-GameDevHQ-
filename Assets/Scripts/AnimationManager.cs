using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GameObject.Find("Enemy").GetComponent<Animator>();   

        if(anim == null)
        {
            Debug.LogError("Enemy not found");
        }
    }

    public void PlayEnemyExplosionAnim()
    {
        anim.SetTrigger("OnEnemyDeath");
    }
}
