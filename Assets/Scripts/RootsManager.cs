using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class RootsManager : MonoBehaviour
{
    [SerializeField] private GameObject _rootPrefab;
    [SerializeField] private float _spawnRootRate;
    [SerializeField, Range(1, 5)] private float _spawnRootRateRange;
    

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(_rootPrefab, "the field _rootPrefab shouldn't be null !");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
