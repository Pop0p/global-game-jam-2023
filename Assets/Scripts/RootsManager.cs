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
    private List<CapsuleCollider> _rootsPoolRB;
    public Cell[,] _cells;
    public bool isCollisionDisabled;
    public bool IsPlaying = false;

    private AudioSource _audioSource;
    public AudioClip ClipSameTime;



    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);



        Assert.IsNotNull(_rootPrefab, "the field _rootPrefab shouldn't be null !");

        if (_audioSource == null)
            _audioSource = GetComponent<AudioSource>();

        _rootsPool = new List<GameObject>();
        _rootsPoolRB = new List<CapsuleCollider>();
        for (int i = 0; i < 50; i++)
        {
            var obj = Instantiate(_rootPrefab);
            obj.transform.parent = transform;
            _rootsPool.Add(obj);
            _rootsPoolRB.Add(obj.transform.Find("THERACINE").GetComponent<CapsuleCollider>());
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
        if (!IsPlaying)
            return;

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
        _rootsPoolRB.Add(obj.transform.Find("THERACINE").GetComponent<CapsuleCollider>());

        return obj;
    }

    public void DisableCollisions()
    {
        for (int i = 0; i < _rootsPoolRB.Count; i++)
            _rootsPoolRB[i].isTrigger = true;

        isCollisionDisabled = true;
    }
    public void EnableCollisions()
    {
        for (int i = 0; i < _rootsPool.Count; i++)
            _rootsPoolRB[i].isTrigger = false;

        isCollisionDisabled = false;
    }
    void DoAttack()
    {
        var r = Random.Range(0f, 1f);
        // Full 5%
        if (r <= 0.05 && (from Cell cell in _cells where cell.HasRoot select cell).Count() < 4)
        {
            SpawnFull();
        }
        // Square 15%
        else if (r <= 0.20)
        {
            SpawnSquare();
        }
        // Line 20 %
        else if (r <= 0.40)
        {
            SpawnLine();
        }
        // cross 15 %
        else if (r <= 0.55)
        {
            SpawnCross();

        }
        // grid 25 %
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
        bool with_different_delay = Random.Range(0, 2) == 0;
        var size = Random.Range(2, 5);
        var center_x = Random.Range(Mathf.FloorToInt(size / 2), _cells.GetLength(0) - Mathf.FloorToInt(size / 2));
        var center_z = Random.Range(Mathf.FloorToInt(size / 2), _cells.GetLength(1) - Mathf.FloorToInt(size / 2));
        for (int x = 0; x < size; x++)
        {
            var x_index = Mathf.FloorToInt(center_x - size / 2) + x;
            for (int z = 0; z < size; z++)
            {
                var z_index = Mathf.FloorToInt(center_z - size / 2) + z;
                DoSpawnRoot(_cells[x_index, z_index], with_different_delay ? Random.Range(0.35f, 0.65f) : 0.35f, !with_different_delay);
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
        int i = 0;
        bool with_different_delay = Random.Range(0, 2) == 0;
        for (int x = 0; x < _cells.GetLength(0); x++)
        {
            for (int z = 0; z < _cells.GetLength(1); z++)
            {
                if (Random.Range(0, 4) == 0)
                    DoSpawnRoot(_cells[x, z], with_different_delay ? 0.35f + (0.15f * i) : 0.35f, !with_different_delay);
                i += 1;
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
        var size = Random.Range(2, 5);
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

        var delay = Random.Range(0.35f, 0.60f);
        bool gg = false;
        foreach (Cell c in _cells)
        {
            if (safes.Contains(c))
                continue;

            if (!gg)
            {
                DoSpawnRoot(c, delay, true);
                gg = true;
            }
            else
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
    private void DoSpawnRoot(Cell cell, float delay, bool same_time_full = false)
    {
        if (cell.HasRoot)
            return;

        GameObject new_root = GetFromPool();
        new_root.SetActive(true);
        new_root.transform.Find("THERACINE").GetComponent<Root>().Setup(cell, delay + .25f, !same_time_full);
        cell.HasRoot = true;

        if (same_time_full)
            StartCoroutine(JOUELESON(delay + .25f));
    }

    IEnumerator JOUELESON(float delay)
    {
        yield return new WaitForSeconds(delay);
        _audioSource.clip = ClipSameTime;
        _audioSource.Play();
    }

}
