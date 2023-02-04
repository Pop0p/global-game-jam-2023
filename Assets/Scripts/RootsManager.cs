using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

public class RootsManager : MonoBehaviour
{
    public static RootsManager Instance;

    [SerializeField] private GameObject _rootPrefab;
    [SerializeField] private float _spawnMinDelay;
    [SerializeField] private float _spawnMaxDelay;
    [SerializeField] private MeshRenderer _ground;

    private float _spawnTime;
    private List<GameObject> _rootsPool;
    public Cell[,] _cells;




    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);


        Assert.IsNotNull(_rootPrefab, "the field _rootPrefab shouldn't be null !");

        _rootsPool = new List<GameObject>();
        for (int i = 0; i < 10; i++)
        {
            var obj = Instantiate(_rootPrefab);
            obj.transform.parent = transform;
            _rootsPool.Add(obj);
        }

        _spawnTime = GetNewSpawnTime();

        var width = _ground.bounds.size.x;
        var length = _ground.bounds.size.z;


        var cell_count = 12;
        var cell_row_width = width / cell_count;
        var cell_row_length = length / cell_count;

        _cells = new Cell[cell_count, cell_count];

        var start_position_x = width / -2 + cell_row_width / 2;
        var start_position_z = length / 2 - cell_row_length / 2;

        for (int x = 0; x < cell_count; x++)
        {
            var cell_x = start_position_x + (cell_row_width) * x;
            for (int z = 0; z < cell_count; z++)
            {
                var cell_z = start_position_z - (cell_row_length) * z;
                _cells[x, z] = new Cell(new Vector3(cell_x, -1.6f, cell_z));
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (_spawnTime < 0)
        {
            DoAttack();
            _spawnTime = GetNewSpawnTime();
        }

        _spawnTime -= Time.deltaTime;
    }


    float GetNewSpawnTime() => Random.Range(_spawnMinDelay, _spawnMaxDelay);

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

    void DoAttack()
    {
        var r = Random.Range(0f, 1f);
        // Full 5%
        if (r <= 0.05 && (from Cell cell in _cells where cell.HasRoot select cell).Count() < 4)
        {
            SpawnFull();
        }
        // Square 20%
        else if (r <= 0.25)
        {
            SpawnSquare();
        }
        // Line 25 %
        else if (r <= 0.50)
        {
            SpawnLine();
        }
        // cross 20 %
        else if (r <= 0.70)
        {
            SpawnCross();

        }
        // grid 10 %
        else if (r <= 0.80)
        {
            SpawnMultipleRandoms();
        }
        // Simple 20 %
        else
        {
            SpawnSimple();
        }
    }


    private void SpawnSquare()
    {
        var cells = new List<Cell>();
        var size = Random.Range(2, 5);
        var center_x = Random.Range(Mathf.FloorToInt(size / 2), _cells.GetLength(0) - Mathf.FloorToInt(size / 2));
        var center_z = Random.Range(Mathf.FloorToInt(size / 2), _cells.GetLength(1) - Mathf.FloorToInt(size / 2));
        for (int x = 0; x < size; x++)
        {
            var x_index = Mathf.FloorToInt(center_x - size / 2) + x;
            for (int z = 0; z < size; z++)
            {
                var z_index = Mathf.FloorToInt(center_z - size / 2) + z;
                cells.Add(_cells[x_index, z_index]);
            }
        }
    }
    private void SpawnLine()
    {
        var center_x = Random.Range(0, _cells.GetLength(0));
        var center_z = Random.Range(0, _cells.GetLength(1));
        bool is_vertical = Random.Range(0, 2) == 0;

        if (is_vertical)
        {
            for (int x = 0; x < _cells.GetLength(0); x++)
                DoSpawnRoot(_cells[x, center_z], 0.25f + (0.05f * x));
        }
        else
        {
            for (int z = 0; z < _cells.GetLength(1); z++)
                DoSpawnRoot(_cells[center_x, z], 0.25f + (0.05f * z));
        }
    }
    private void SpawnMultipleRandoms()
    {
        bool with_different_delay = Random.Range(0, 2) == 0;
        for (int x = 0; x < _cells.GetLength(0); x++)
        {
            for (int z = 0; z < _cells.GetLength(1); z++)
            {
                if (Random.Range(0, 4) == 0)
                    DoSpawnRoot(_cells[x, z], with_different_delay ? Random.Range(0.1f, 035f) : 0.15f);
            }
        }
    }
    private void SpawnSimple()
    {
        var center_x = Random.Range(0, _cells.GetLength(0));
        var center_z = Random.Range(0, _cells.GetLength(1));

        DoSpawnRoot(_cells[center_x, center_z], Random.Range(0.25f, 0.50f));
    }
    private void SpawnCross()
    {
        var center_x = Mathf.FloorToInt(_cells.GetLength(0) / 2);
        var center_z = Mathf.FloorToInt(_cells.GetLength(1) / 2);

        DoSpawnRoot(_cells[center_x, center_z], 0.20f);

        //right
        for (int x = center_x + 1; x < _cells.GetLength(0); x++)
            DoSpawnRoot(_cells[x, center_z], 0.25f + (0.15f * Mathf.Abs(center_x - x)));
        //// left
        for (int x = center_x - 1; x >= 0; x--)
            DoSpawnRoot(_cells[x, center_z], 0.25f + (0.15f * (center_x - x))); ;


        ////// bottom
        for (int z = center_z + 1; z < _cells.GetLength(1); z++)
            DoSpawnRoot(_cells[center_x, z], 0.25f + (0.15f * Mathf.Abs(center_z - z)));
        // Top
        for (int z = center_z - 1; z >= 0; z--)
            DoSpawnRoot(_cells[center_x, z], 0.25f + (0.15f * (center_z - z)));


    }
    private void SpawnFull()
    {
        Vector3 flower_pos = Vector3.one * -50;
        var safes = new List<Cell>();
        var size = Random.Range(2, 4);
        var center_x = Random.Range(Mathf.FloorToInt(size / 2), _cells.GetLength(0) - Mathf.FloorToInt(size / 2));
        var center_z = Random.Range(Mathf.FloorToInt(size / 2), _cells.GetLength(1) - Mathf.FloorToInt(size / 2));

        for (int x = 0; x < size; x++)
        {
            var x_index = Mathf.FloorToInt(center_x - size / 2) + x;
            for (int z = 0; z < size; z++)
            {
                var z_index = Mathf.FloorToInt(center_z - size / 2) + z;
                if (flower_pos == Vector3.one * -50)
                    flower_pos = _cells[x_index, z_index].Position;
                    safes.Add(_cells[x_index, z_index]);
            }
        }

        var delay = Random.Range(0.25f, 0.50f);
        foreach (Cell c in _cells)
        {
            if (safes.Contains(c))
                continue;

            DoSpawnRoot(c, delay);
        }
        flower_pos.y = 0.5f;
        FlowerManager.Instance.DoSpawnFlower(flower_pos, .25f);


    }


    public Vector3 GetFlowerPosition()
    {
        var center_x = Random.Range(1, _cells.GetLength(0));
        var center_z = Random.Range(1, _cells.GetLength(1));
        var pos = _cells[center_x, center_z].Position;

        pos.y = 0.25f;

        return pos;

    }
    private void DoSpawnRoot(Cell cell, float delay)
    {
        if (cell.HasRoot)
            return;

        GameObject new_root = GetFromPool();
        new_root.transform.position = cell.Position;
        new_root.GetComponent<Root>().Associated_cell = cell;
        new_root.GetComponent<Root>().ApparitionTime = delay;
        cell.HasRoot = true;
        new_root.SetActive(true);
    }

}
