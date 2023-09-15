using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ObjectGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _objectPrefab;
    [SerializeField] private Transform _objectContainer;
    [SerializeField] private List<GameObject> _spawnedObjects;
    [SerializeField] private Button _generateObjectsButton;
    [SerializeField] private Button _deleteObjectsButton;
    
    private const int DeleteCount = 100;
    private const int SpawnCount = 100;
    private const float LowerBound = -10f;
    private const float HigherBound = 10f;
    private const float PosY = 0.5f;



    private void Start()
    {
        _generateObjectsButton.onClick.AddListener(GenerateObjects);
        _deleteObjectsButton.onClick.AddListener(DeleteObjects);
    
    }

    public void GenerateObjects()
    {
        for (var i = 0; i < SpawnCount; i++)
        {
            var spawnPosition = new Vector3(Random.Range(LowerBound, HigherBound), PosY, Random.Range(LowerBound, HigherBound));
            var newObject = Instantiate(_objectPrefab, spawnPosition, Quaternion.identity, _objectContainer);
            _spawnedObjects.Add(newObject);
        }
    }

    public void DeleteObjects()
    {
        if (_spawnedObjects.Count <= 0) return;

        for (var i = 0; i < DeleteCount; i++)
        {
            var objectToDestroy = _spawnedObjects[^1];
            _spawnedObjects.RemoveAt(_spawnedObjects.Count - 1);
            Destroy(objectToDestroy);
        }
    }
}