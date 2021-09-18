using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private int rotateSpeed = 20;

    SpawnManager _spawnManager;

    UIManager _uiManager;

    [SerializeField] private GameObject _explosionPrefab;


    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if(_spawnManager == null)
        {
            Debug.LogError("SpawnManager is NULL in Asteroid");
        }

        if(_uiManager == null)
        {
            Debug.Log("UIManager is NULL in Asteroid");
        }
    }

    void Update()
    {
        transform.Rotate(0, 0, 1 * rotateSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            Destroy(gameObject, 2.633f);
            Destroy(GetComponent<SpriteRenderer>());
            Destroy(GetComponent<Asteroid>());
        }
    }
}
