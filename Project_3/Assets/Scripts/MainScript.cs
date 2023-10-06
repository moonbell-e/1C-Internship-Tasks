using System.Collections.Generic;
using UnityEngine;

public class MainScript: MonoBehaviour
{
    [SerializeField] private UnitController _unitController;
    [SerializeField] private UnitType _unitType;
    [SerializeField] private SelectType _selectType;
    [SerializeField] private float _radius;
    
    private IEnumerable<UnitData> _units;
    
    private void Start()
    {
        switch (_selectType)
        {
            case SelectType.SelectAll:
                _units =  _unitController.SelectAllUnits();
                break;
            case SelectType.SelectWithinRadius:
                _units =  _unitController.SelectUnitsInRadius(transform, _radius);
                break;
            case SelectType.SelectWithinRadiusAndType:
                _units =  _unitController.SelectUnitsWithTypeInRadius(transform, _radius, _unitType);
                break;
        }
        
        
        foreach (var unit in _units)
        {
            Debug.Log($"Name: {unit.name}, Position: {unit.position}, Max Health: {unit.maxHealth}, Current Health: {unit.currentHealth}");
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}