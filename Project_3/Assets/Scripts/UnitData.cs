using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class UnitData
{
    public Vector3 position;
    public string name;
    public float maxHealth;
    public float currentHealth;
    public UnitType type;
}