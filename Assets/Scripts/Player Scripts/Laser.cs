using UnityEngine;

public class Laser : MonoBehaviour
{
    private int _laserSpeed = 8;

    private bool _isEnemy;

    Vector3 _offset = new Vector3(0f, -0.5f, 0f);

    [SerializeField] GameObject _explosionPrefab;

    Player _player;


    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if(_player == null)
        {
            Debug.LogError("Player is NULL in Laser");
        }
    }

    void Update()
    {
        if (_isEnemy == true)
        {
            MoveDown();
        }
        else
        {
            MoveUp();
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);

        if (transform.position.y > 7f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(gameObject);
        }
    }

    void MoveDown()
    {
        transform.Translate((Vector3.down + _offset) * _laserSpeed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _player.Damage();            
            Destroy(gameObject);
        }

        if (other.tag == "MegaLaser")
        {
            Destroy(gameObject);
        }
    }

    public void AssignEnemyLaser(bool isEnemy)
    {
        _isEnemy = isEnemy;
    }
}

