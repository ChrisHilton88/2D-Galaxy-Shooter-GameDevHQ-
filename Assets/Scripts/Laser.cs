using UnityEngine;

public class Laser : MonoBehaviour
{
    Player player;

    public int laserSpeed = 5;

    [SerializeField] private bool _isEnemy = false;

    void Start()
    {
        // Lasers getting instaniated after the player dies is causing errors in the Console Log
        player = GameObject.Find("Player").GetComponent<Player>();

        if(player == null)
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
        transform.Translate(Vector3.up * laserSpeed * Time.deltaTime);

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
        transform.Translate(Vector3.down * laserSpeed * Time.deltaTime);

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
        if(other.tag == "Player" && _isEnemy == true)
        {
            player.Damage();
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemy = true;
    }
}

