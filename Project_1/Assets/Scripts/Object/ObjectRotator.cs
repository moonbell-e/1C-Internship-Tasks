using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    private float _rotationSpeed;

    private void Start()
    {
        RandomizeRotationSpeed();
    }

    private void Update()
    {
        RotateObject();
    }

    private void RotateObject()
    {
        transform.Rotate(Vector3.right * (_rotationSpeed * Time.deltaTime));
    }

    private void RandomizeRotationSpeed()
    {
        _rotationSpeed = Random.Range(50f, 100f);
    }
}