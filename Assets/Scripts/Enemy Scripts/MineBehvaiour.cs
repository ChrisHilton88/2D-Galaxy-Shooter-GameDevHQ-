using UnityEngine;

public class MineBehvaiour : MonoBehaviour
{
    private float _mineSpeed = 3f;

    private bool _blownUp;

    Vector3 _playClipAtPoint = new Vector3(0, 0, 0);
    Vector3 _playerPos;

    [SerializeField] AudioClip _explosionClip;

    Animator _anim;

    Player _player;

    SpriteRenderer _spriteRend;


    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _spriteRend = GetComponent<SpriteRenderer>();

        if(_player == null)
        {
            Debug.LogError("Player is NULL in MineBehaviour");
        }

        if(_anim == null)
        {
            Debug.LogError("Animator is NULL in MineBehaviour");
        }

        if(_spriteRend == null)
        {
            Debug.LogError("SpriteRenderer is NULL in MineBehaviour");
        }
    }

    void Update()
    {
        if (_blownUp)
        {
            _mineSpeed = 0f;
        }
        else
        {
            _playerPos = _player.transform.position;
            Vector3 direction = transform.position - _playerPos;
            direction = -direction.normalized;
            transform.position += direction * _mineSpeed * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _anim.SetTrigger("OnMineCollision");
            AudioSource.PlayClipAtPoint(_explosionClip, _playClipAtPoint, 1f);
            _blownUp = true;
            _player.Damage();
            Destroy(gameObject, 2.633f);
            Destroy(GetComponent<Collider2D>());
        }

        if (other.tag == "Shield")
        {
            _anim.SetTrigger("OnMineCollision");
            AudioSource.PlayClipAtPoint(_explosionClip, _playClipAtPoint, 1f);
            _blownUp = true;
            Destroy(gameObject, 2.633f);
            Destroy(GetComponent<Collider2D>());
        }

        if (other.tag == "Laser")
        {
            _anim.SetTrigger("OnMineCollision");
            AudioSource.PlayClipAtPoint(_explosionClip, _playClipAtPoint, 1f);
            _blownUp = true;
            Destroy(gameObject, 2.633f);
            Destroy(GetComponent<Collider2D>());
            Destroy(other.gameObject);
        }

        if (other.tag == "Missile")
        {
            _anim.SetTrigger("OnMineCollision");
            AudioSource.PlayClipAtPoint(_explosionClip, _playClipAtPoint, 1f);
            _blownUp = true;
            Destroy(gameObject, 2.633f);
            Destroy(GetComponent<Collider2D>());
            Destroy(other.gameObject);
        }

        if(other.tag == "MegaLaser")
        {
            _anim.SetTrigger("OnMineCollision");
            AudioSource.PlayClipAtPoint(_explosionClip, _playClipAtPoint, 1f);
            _blownUp = true;
            Destroy(gameObject, 2.633f);
            Destroy(GetComponent<Collider2D>());
        }
    }   
}
