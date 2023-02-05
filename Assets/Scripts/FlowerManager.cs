using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerManager : MonoBehaviour
{
    public static FlowerManager Instance;
    public int FlowerCountToWin = 6;
    public float DelayToSpawnFlower = 20;
    [SerializeField] private GameObject _flowerPrefab;
    private List<Flower> _currentFlowers;
    private float _timeSinceLastSpawn = 0;

    public bool IsPlaying = false;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);

        _currentFlowers = new List<Flower>();
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!IsPlaying)
            return;
        if (_timeSinceLastSpawn > DelayToSpawnFlower)
        {
            DoSpawnFlower(RootsManager.Instance.GetFlowerPosition(), 1);
            _timeSinceLastSpawn = 0f;
        }

        _timeSinceLastSpawn += Time.deltaTime;
    }

    public void DoSpawnFlower(Vector3 pos, float delay)
    {
        if (_currentFlowers.Count == 6)
            return;
        IEnumerator doSpawn()
        {
            yield return new WaitForSeconds(delay);
            var obj = Instantiate(_flowerPrefab);
            obj.transform.parent = transform;
            obj.transform.position = pos;
            _currentFlowers.Add(obj.GetComponent<Flower>());
            _timeSinceLastSpawn = 0f;
        }

        StartCoroutine(doSpawn());

    }
}
