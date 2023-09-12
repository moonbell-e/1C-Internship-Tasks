using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _objectPrefab;
    [SerializeField] private Transform _objectContainer;
    [SerializeField] private List<GameObject> _spawnedObjects;

    private int _spawnCount;
    private int _deleteCount;

    private void Start()
    {
        _spawnCount = 100;
        _deleteCount = 100;
    }

    public void GenerateObjects()
    {
        for (int i = 0; i < _spawnCount; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-10f, 10f), 0.5f, Random.Range(-10f, 10f));
            GameObject newObject = Instantiate(_objectPrefab, spawnPosition, Quaternion.identity, _objectContainer);
            _spawnedObjects.Add(newObject);
        }
    }

    public void DeleteObjects()
    {
        if (_spawnedObjects.Count <= 0) return;

        for (int i = 0; i < _deleteCount; i++)
        {
            GameObject objectToDestroy = _spawnedObjects[^1];
            _spawnedObjects.RemoveAt(_spawnedObjects.Count - 1);
            Destroy(objectToDestroy);
        }
    }
}