using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _objectPrefab;
    [SerializeField] private Transform _objectContainer;
    [SerializeField] private List<GameObject> _spawnedObjects;
    private int _deleteCount;

    private int _spawnCount;

    private void Start()
    {
        _spawnCount = 100;
        _deleteCount = 100;
    }

    public void GenerateObjects()
    {
        for (var i = 0; i < _spawnCount; i++)
        {
            var spawnPosition = new Vector3(Random.Range(-10f, 10f), 0.5f, Random.Range(-10f, 10f));
            var newObject = Instantiate(_objectPrefab, spawnPosition, Quaternion.identity, _objectContainer);
            _spawnedObjects.Add(newObject);
        }
    }

    public void DeleteObjects()
    {
        if (_spawnedObjects.Count <= 0) return;

        for (var i = 0; i < _deleteCount; i++)
        {
            var objectToDestroy = _spawnedObjects[^1];
            _spawnedObjects.RemoveAt(_spawnedObjects.Count - 1);
            Destroy(objectToDestroy);
        }
    }
}