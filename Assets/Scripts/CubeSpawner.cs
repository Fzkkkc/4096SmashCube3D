using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubeSpawner : MonoBehaviour
{
    public static CubeSpawner Instance;
    
    Queue<Cube> _cubesQueue = new Queue<Cube>();
    [SerializeField] private int _cubesQueueCapacity = 20;
    [SerializeField] private bool _autoQueueGrow = true;

    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private Color[] _cubeColors;

    [HideInInspector] public int MaxCubeNumber; //4096 (2*12)

    private int _maxPower = 12;

    private Vector3 _defaultSpawnPosition;

    private void Awake()
    {
        Instance = this;

        _defaultSpawnPosition = transform.position;
        MaxCubeNumber = (int) Mathf.Pow(2, _maxPower);

        InitializeCubeQueue();
    }

    private void InitializeCubeQueue()
    {
        for (int i = 0; i < _cubesQueueCapacity; i++)
            AddCubeToQueue();
    }

    private void AddCubeToQueue()
    {
        Cube cube = Instantiate(
                _cubePrefab,
                _defaultSpawnPosition,
                Quaternion.identity,
                transform).
                GetComponent<Cube>();
        
        cube.gameObject.SetActive(false);
        cube.IsMainCube = false;
        
        _cubesQueue.Enqueue(cube);
    }

    public Cube Spawn(int number, Vector3 position)
    {
        if (_cubesQueue.Count == 0)
        {
            if (_autoQueueGrow)
            {
                _cubesQueueCapacity++;
                AddCubeToQueue();
            }
            else
            {
                Debug.Log("no more cubes in the pool");
                return null;
            }
        }

        Cube cube = _cubesQueue.Dequeue();
        cube.transform.position = position;
        cube.SetNumber(number);
        cube.SetColor(GenerateColor(number));
        cube.gameObject.SetActive(true);

        return cube;
    }

    public Cube SpawnRandom() => 
        Spawn(GenerateRandomNumber(), _defaultSpawnPosition);

    public void DestroyCube(Cube cube)
    {
        cube.CubeRigidbody.velocity = Vector3.zero;
        cube.CubeRigidbody.angularVelocity = Vector3.zero;
        cube.transform.rotation = Quaternion.identity;
        cube.IsMainCube = false;
        cube.gameObject.SetActive(false);
        _cubesQueue.Enqueue(cube);
    }
    
    public int GenerateRandomNumber() => 
        (int) Mathf.Pow(2, Random.Range(1,6));

    private Color GenerateColor(int number) => 
        _cubeColors[(int) (Mathf.Log(number) / Mathf.Log(2)) - 1];
}
