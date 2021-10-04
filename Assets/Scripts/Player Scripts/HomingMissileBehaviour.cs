using UnityEngine;

public class HomingMissileBehaviour : MonoBehaviour
{
    private float _missileSpeed = 5f;

    private bool _enemyTargetFound;

    Vector3 _playClipAtPoint = new Vector3(0, 0, 0);

    [SerializeField] AudioClip _explosionClip;

    Animator _anim;

    GameObject _closestEnemy;

    SpriteRenderer _spriteRend;


    void Start()
    {
        _anim = GetComponent<Animator>();
        _spriteRend = GetComponent<SpriteRenderer>();

        if(_anim == null)
        {
            Debug.LogError("Animator is NULL in HomingMissileBehaviour");
        }

        if(_spriteRend == null)
        {
            Debug.LogError("SpriteRenderer is NULL in HomingMissileBehaviour");
        }
    }

    void Update()
    {
        if (_enemyTargetFound == true)
        {
            HomingMissileMovement();
        }
        else
        {
            transform.position += Vector3.up * _missileSpeed * Time.deltaTime;

            if(transform.position.y > 6f)
            {
                Destroy(gameObject);
            }
        }
    }

    void FindClosestEnemy(string _enemyTagName)
    {
        float _closestDistanceToEnemy = Mathf.Infinity;
        GameObject[] _enemyAndBossList = GameObject.FindGameObjectsWithTag(_enemyTagName);

        if (_enemyAndBossList != null)
        {
            _enemyTargetFound = true;

            foreach (GameObject enemy in _enemyAndBossList)
            {
                float _distanceToEnemy = (enemy.transform.position - transform.position).sqrMagnitude;

                if (_distanceToEnemy < _closestDistanceToEnemy)
                {
                    _closestDistanceToEnemy = _distanceToEnemy;
                    _closestEnemy = enemy;
                }
            }
        }

        if (_enemyAndBossList.Length == 0)
        {
            _enemyTargetFound = false;
            Debug.Log("No enemies found!");
        }
    }

    void HomingMissileMovement()
    {
        Vector3 direction = transform.position - _closestEnemy.transform.position;
        direction = -direction.normalized;
        transform.rotation = Quaternion.LookRotation(transform.forward, direction);
        transform.position += direction * _missileSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            _anim.SetBool("OnLaserCollision", true); 
            AudioSource.PlayClipAtPoint(_explosionClip, _playClipAtPoint, 1f);
            Destroy(gameObject, 2.633f);
            Destroy(other.gameObject);
            Destroy(GetComponent<SpriteRenderer>());
            Destroy(GetComponent<Collider2D>());
            Destroy(GetComponent<HomingMissileBehaviour>());
        }

        if(other.tag == "Boss")
        {
            _anim.SetTrigger("OnBossCollision");
            AudioSource.PlayClipAtPoint(_explosionClip, _playClipAtPoint, 1f);
            _missileSpeed = 0f;
            Destroy(gameObject, 2.633f);
            Destroy(_spriteRend);
        }

        if (other.tag == "BossShield")
        {
            _anim.SetTrigger("OnBossCollision");
            AudioSource.PlayClipAtPoint(_explosionClip, _playClipAtPoint, 1f);
            _missileSpeed = 0f;
            Destroy(gameObject, 2.633f);
            Destroy(GetComponent<Collider2D>());
            Destroy(_spriteRend);
        }
    }

    public void FindClosestEnemy(int _currentWave)
    {
        if (_currentWave < 6)
        {
            FindClosestEnemy("Enemy");
        }
        else if (_currentWave == 6)
        {
            FindClosestEnemy("Boss");
        }
    }
}

