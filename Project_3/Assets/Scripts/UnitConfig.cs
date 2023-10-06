using UnityEngine;

[CreateAssetMenu(fileName = "UnitConfig", menuName = "Units/UnitConfig")]
public class UnitConfig : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private float _maxHealth;
    [SerializeField] private UnitType _unitType;

    public string Name => _name;
    public float MaxHealth => _maxHealth;
    public UnitType UnitType => _unitType;
}
