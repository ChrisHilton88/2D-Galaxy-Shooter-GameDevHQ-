using System.Collections;
using UnityEngine;

public class Powerups : MonoBehaviour
{ 
    private int _powerupSpeed = 0;
    private int _magnetSpeed = 5;

    private bool _isMagnetising = false;

    Vector3 playerPos;

    Player player;

    ThrusterController thrustCont;

    [SerializeField] private int _powerupID;

    [SerializeField] private AudioClip _clip;

    [SerializeField] GameObject _powerupExplosionPrefab;


    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        thrustCont = GameObject.Find("Thruster").GetComponent<ThrusterController>();

        if(player == null)
        {
            Debug.LogError("Player script not found in Powerups script");
        }

        if(thrustCont == null)
        {
            Debug.LogError("ThrusterController script not found within the Powerups script");
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_clip, transform.position, 1f);

            switch (_powerupID)
            {
                case 0:
                    player.TripleShotActive();
                    Destroy(gameObject);
                    break;
                case 1:
                    player.SpeedBoostActive();
                    Destroy(gameObject);
                    break;
                case 2:
                    player.ShieldBoostActive();
                    Destroy(gameObject);
                    break;
                case 3:
                    player.AmmoRefillActive();
                    Destroy(gameObject);
                    break;
                case 4:
                    player.HealthRefillActive();
                    Destroy(gameObject);
                    break;
                case 5:
                    player.MegaLaserActive();
                    Destroy(gameObject);
                    break;
                case 6:
                    player.NegativePickup();
                    StartCoroutine(thrustCont.NegativePickupRoutine());
                    Destroy(gameObject);
                    break;
                default:
                    Debug.Log("Unknown value");
                    break;
            }
        }

        if(other.tag == "Enemy")
        {
            Instantiate(_powerupExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (other.tag == "Laser")
        {
            Instantiate(_powerupExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    public void Magnetise()
    {
        _isMagnetising = true;
    }

    void PowerupMagnetMove()
     {
        playerPos = player.transform.position;
        Vector3 direction = transform.position - playerPos;
        direction = direction.normalized;
        transform.position += direction * _magnetSpeed * Time.deltaTime;
     }
}
