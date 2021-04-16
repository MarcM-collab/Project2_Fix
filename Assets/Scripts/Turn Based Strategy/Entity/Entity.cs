using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    TeamA,
    TeamB,
}
public class Entity : MonoBehaviour
{
    public delegate void ChangeHealthDelegate(float fractionHealth);
    public ChangeHealthDelegate OnChangeHealth;

    [Header("Stats")]
    public float HP;
    public int AttackPoints;
    private float MaxHP;
    public int Range;

    [Header("Other")]
    public bool Exhausted;
    public Team Team;
    public bool IsAlive => HP > 0;
    public bool IsActive => IsAlive && !Exhausted;

    private void Start()
    {
        MaxHP= HP;
        OnChangeHealth?.Invoke(HP / MaxHP);
    }
    public void ChangeHealth()
    {
        OnChangeHealth?.Invoke(HP / MaxHP);
    }
    public static bool operator ==(Entity e1, Entity e2)
    {
        var positionE1 = e1.gameObject.transform.position;
        var positionE2 = e2.gameObject.transform.position;

        return positionE1.x == positionE2.x && positionE1.y == positionE2.y;
    }
    public static bool operator !=(Entity e1, Entity e2)
    {
        var positionE1 = e1.gameObject.transform.position;
        var positionE2 = e2.gameObject.transform.position;

        return !(positionE1.x == positionE2.x && positionE1.y == positionE2.y);
    }
}
