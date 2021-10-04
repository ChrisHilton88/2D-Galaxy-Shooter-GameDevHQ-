using UnityEngine;

public class Powerups : MonoBehaviour
{ 
    private int _powerupSpeed = 3;
    private int _magnetSpeed = 5;
    [SerializeField] private int _powerupID;

    private bool _isMagnetising;

    Vector3 _playerPos;

    [SerializeField] private AudioClip _clip;

    [SerializeField] GameObject _powerupExplosionPrefab;

    Player _player;

    ThrusterController _thrustCont;


    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _thrustCont = GameObject.Find("Thruster").GetComponent<ThrusterController>();

        if(_player == null)
        {
            Debug.LogError("Player is NULL in Powerups");
        }

        if(_thrustCont == null)
        {
            Debug.LogError("ThrusterController is NULL in Powerups");
        }
    }

    void Update()
    {
        if (_isMagnetising)
        {
            PowerupMagnetMove();
        }
        else
        {
            transform.Translate(Vector3.down * _powerupSpeed * Time.deltaTime);
        }

        if (transform.position.y < -6)
        {
            Destroy(gameObject);
        }
    }

    void PowerupMagnetMove()
    {
        _playerPos = _player.transform.position;
        Vector3 direction = transform.position - _playerPos;
        direction = -direction.normalized;
        transform.position += direction * _magnetSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_clip, transform.position, 1f);

            switch (_powerupID)
            {
                case 0:
                    _player.TripleShotActive();
                    Destroy(gameObject);
                    break;
                case 1:
                    _player.SpeedBoostActive();
                    Destroy(gameObject);
                    break;
                case 2:
                    _player.ShieldBoostActive();
                    Destroy(gameObject);
                    break;
                case 3:
                    _player.AmmoRefillActive();
                    Destroy(gameObject);
                    break;
                case 4:
                    _player.HealthRefillActive();
                    Destroy(gameObject);
                    break;
                case 5:
                    _player.MegaLaserActive();
                    Destroy(gameObject);
                    break;
                case 6:
                    _player.NegativePickup();
                    Destroy(gameObject);
                    break;
                default:
                    Debug.Log("Unknown value");
                    break;
            }
        }

        if (other.tag == "Laser")
        {
            Instantiate(_powerupExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        if (other.tag == "EnemyMegaLaser")
        {
            Instantiate(_powerupExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void Magnetise()
    {
        _isMagnetising = true;
    }
}
