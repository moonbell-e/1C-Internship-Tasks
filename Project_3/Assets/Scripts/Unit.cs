using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;
    [SerializeField] private UnitType _unitType;

    public void SetUnitData(UnitData unitData)
    {
        unitData.position = transform.position;
        unitData.name = _name;
        unitData.maxHealth = _maxHealth;
        unitData.currentHealth = _currentHealth;
        unitData.type = _unitType;
    }
}
