using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    WaitForSeconds _cameraTimer = new WaitForSeconds(0.1f);

    [SerializeField] Transform _mainCamera;

    Vector3 _resetMainCamera = new Vector3(0, 0, -10);
    Vector3 _leftCameraPos = new Vector3(-0.05f, 0, -10);
    Vector3 _rightCameraPos = new Vector3(0.05f, 0, -10);

    private bool _coroutinePlaying = false;


    public void CallCameraShake()
    {
        if (_coroutinePlaying == false)
        {
            StartCoroutine(CameraShakeRoutine(1f, 0.05f));
            _coroutinePlaying = true;
        }
        else
        {
            return;
        }
    }

    IEnumerator CameraShakeRoutine(float duration, float durationReduction)
    {
        while (duration > 0f)
        {
            _mainCamera.position = _leftCameraPos;
            yield return _cameraTimer;
            _mainCamera.position = _rightCameraPos;
            yield return _cameraTimer;
            duration -= Time.time * durationReduction;
        }

        _mainCamera.position = _resetMainCamera;
        _coroutinePlaying = false;
    }
}



