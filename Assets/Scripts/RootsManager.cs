using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

public class RootsManager : MonoBehaviour
{
    [SerializeField] private GameObject _rootPrefab;
    [SerializeField] private float _spawnMinDelay;
    [SerializeField] private float _spawnMaxDelay;


    [SerializeField] private MeshRenderer _ground;

    private float _spawnMaxCount = 1;
    private float _spawnTime;
    private List<GameObject> _rootsPool;

    private float _currentTime;

    private List<Vector3> _usedPositions;


    private void Awake()
    {
        Assert.IsNotNull(_rootPrefab, "the field _rootPrefab shouldn't be null !");
        Assert.IsTrue(_ground.transform.position.Equals(Vector3.zero), "the ground should be at position 0");

        _rootsPool = new List<GameObject>();
        for (int i = 0; i < 5; i++)
        {
            var obj = Instantiate(_rootPrefab);
            obj.transform.parent = transform;
            _rootsPool.Add(obj);
        }

        _usedPositions = new List<Vector3>();
        _spawnTime = GetNewSpawnTime();



    }
    // Update is called once per frame
    void Update()
    {
        if (_spawnTime < 0)
        {
            SpawnRoot();

            _spawnTime = GetNewSpawnTime();
        }

        _spawnTime -= Time.deltaTime;
        _currentTime += Time.deltaTime;

        if (_currentTime >= 10 && _spawnMaxCount < 2)
            _spawnMaxCount = 2;
        else if (_currentTime >= 25 && _spawnMaxCount < 3)
            _spawnMaxCount = 3;
        else if (_currentTime >= 45 && _spawnMaxCount < 5)
            _spawnMaxCount = 5;

    }

    void SpawnRoot()
    {
        List<GameObject> spawning = new();
        for (var i = 0; i < Random.Range(1, _spawnMaxCount); i++)
        {
            Vector3 target_position = GetPosition();

            // If the position is already used simply don't spawn the root... (maybe try another position.. ? )
            if (!isPositionFree(target_position))
            {
                continue;
            }

            _usedPositions.Add(target_position);
            GameObject new_root = GetFromPool();
            new_root.transform.position = target_position;
            new_root.SetActive(true);
            spawning.Add(new_root);
        }


        if (spawning.Count <= 0)
            return;

        var action = Random.Range(0, 1);
        var wait_before_activate_time = Random.Range(.75f, 1.25f);
        if (action <= 0.5) // Same time.
        {
            foreach (GameObject item in spawning)
            {
                var root = item.GetComponent<Root>();
                root.ApparitionTime = wait_before_activate_time;
            }

        }
        else if (action <= 1)
        {
            var interval = Random.Range(0.15f, .4f);
            for (int i = 0; i < spawning.Count; i++)
            {
                var root = spawning[i].GetComponent<Root>();
                root.ApparitionTime = wait_before_activate_time + (i * interval);
            }
        }













    }

    float GetNewSpawnTime() => Random.Range(_spawnMinDelay, _spawnMaxDelay);

    Vector3 GetPosition()
    {
        var size = _ground.bounds.size / 2;

        // -3.5f because atm the root height is 3 so i just add 0.5.
        var position = new Vector3(Random.Range(-size.x + 0.25f, size.x - 0.25f), -3.5f, Random.Range(-size.z + 0.25f, size.z - 0.25f));

        return position;
    }
    bool isPositionFree(Vector3 pos) => !_usedPositions.Any(up => Vector3.Distance(up, pos) <= .5f);



    GameObject GetFromPool()
    {
        for (int i = 0; i < _rootsPool.Count; i++)
        {
            if (!_rootsPool[i].activeInHierarchy)
                return _rootsPool[i];
        }

        var obj = Instantiate(_rootPrefab);
        obj.transform.parent = transform;
        _rootsPool.Add(obj);

        return obj;
    }
}
