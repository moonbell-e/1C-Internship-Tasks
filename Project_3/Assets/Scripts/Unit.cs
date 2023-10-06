using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float _currentHealth;
    [SerializeField] private UnitConfig _config;
    
    private string _name;
    private float _maxHealth;
    private UnitType _unitType;

    private void Awake()
    {
        _name = _config.Name;
        _maxHealth = _config.MaxHealth;
        _unitType = _config.UnitType;
    }

    public void SetUnitData(UnitData unitData)
    {
        unitData.position = transform.position;
        unitData.name = _name;
        unitData.maxHealth = _maxHealth;
        unitData.currentHealth = _currentHealth;
        unitData.type = _unitType;
    }
}
