using UnityEngine;

public class Explosion : MonoBehaviour
{
    Animator _anim;

    AudioSource _source;


    void Start()
    {
        _anim = GetComponent<Animator>();
        _source = GetComponent<AudioSource>();

        if(_anim == null)
        {
            Debug.LogError("Animator is NULL in Explosion");
        }

        if(_source == null)
        {
            Debug.LogError("AudioSource is NULL in Explosion");
        }

        StartExplosion();
    }

    void StartExplosion()
    {
        _anim.SetTrigger("OnAsteroidExplosion");
        Destroy(this.gameObject, 2.633f);
    }
}
