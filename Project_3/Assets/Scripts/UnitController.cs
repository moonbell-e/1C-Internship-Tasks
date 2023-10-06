using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    private readonly List<UnitData> _unitData = new();

    private void Awake()
    {
        RegisterAllUnits();
    }
    
    public IEnumerable<UnitData> SelectUnitsInRadius(Transform center, float radius)
    {
        return _unitData.Where(unit => Vector3.Distance(center.position, unit.position) <= radius);
    }
    
    public IEnumerable<UnitData> SelectUnitsWithTypeInRadius(Transform center, float radius, UnitType type)
    {
        return _unitData.Where(unit => Vector3.Distance(center.position, unit.position) <= radius).Where(unit => type == unit.type);
    }

    public IEnumerable<UnitData> SelectAllUnits()
    {
        return _unitData;
    }

    private void RegisterAllUnits()
    {
        var units = FindObjectsOfType<Unit>();
        foreach (var unit in units)
        {
            SetDataFromUnit(unit);
        }
    }

    private void SetDataFromUnit(Unit unit)
    {
        var unitData = new UnitData();
        unit.SetUnitData(unitData);
        _unitData.Add(unitData);
    }
}