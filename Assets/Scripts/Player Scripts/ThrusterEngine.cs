using UnityEngine;

public class ThrusterEngine : MonoBehaviour
{
    Animator _anim;


    void Start()
    {
        _anim = GetComponent<Animator>();

        if(_anim == null)
        {
            Debug.LogError("Animator is NULL in ThrusterEngine");
        }
    }

    public void PlaySmokeAnimation()
    {
        _anim.Play("Thruster_Overload_Anim");
    }
}
