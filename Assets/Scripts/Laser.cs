using UnityEngine;

public class Laser : MonoBehaviour
{
    Player _player;

    private int _laserSpeed = 5;

    private bool _isEnemy;

    [SerializeField] GameObject _explosionPrefab;


    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if(_player == null)
        {
            Debug.LogError("Player script not found in Laser script");
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

        if (transform.position.y > 6)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _laserSpeed * Time.deltaTime);

        if (transform.position.y < -6)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
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

    public void AssignEnemyLaser()
    {
        _isEnemy = true;
    }
}

