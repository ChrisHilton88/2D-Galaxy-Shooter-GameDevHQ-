using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    Animator anim;

    AudioSource source;

    void Start()
    {
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        StartExplosion();
    }

    void StartExplosion()
    {
        anim.SetTrigger("OnAsteroidExplosion");
        Destroy(this.gameObject, 2.633f);
    }
}
