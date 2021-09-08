using UnityEngine;

public class ThrusterEngine : MonoBehaviour
{
    Animator _anim;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void PlaySmokeAnimation()
    {
        _anim.Play("Thruster_Overload_Anim");
    }
}
