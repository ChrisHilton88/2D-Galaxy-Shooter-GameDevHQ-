using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private int rotateSpeed = 20;

    SpawnManager _spawnManager;

    [SerializeField] private GameObject _explosionPrefab;


    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        if(_spawnManager == null)
        {
            Debug.LogError("SpawnManager is Null : Asteroid");
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
            Destroy(this.gameObject);
        }
    }
}
