using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _objectPrefab;
    [SerializeField] private Transform _objectContainer;
    [SerializeField] private List<GameObject> _spawnedObjects;
    
    private const int DeleteCount = 100;
    private const int SpawnCount = 100;
    
    public void GenerateObjects()
    {
        for (var i = 0; i < SpawnCount; i++)
        {
            var spawnPosition = new Vector3(Random.Range(-10f, 10f), 0.5f, Random.Range(-10f, 10f));
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