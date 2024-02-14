using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("���� ����")]
    [SerializeField] private UnitState unitState;
    [SerializeField] private UnitType unitType;
    [SerializeField] private Elemental elemental;
    [SerializeField] private Profession profession;

    [Header("��ų")]
    [SerializeField] private Skill skill;

    [Header("���� �ɷ�ġ")]
    [SerializeField] private int level;
    [SerializeField] private int hp;
    [SerializeField] private int attackPoint;
    [SerializeField] private int moveSpeed = 10;
    [SerializeField] private int AttackRange;

    [Header("���� Ÿ��")]
    [SerializeField] private Unit currentTarget;

    public UnitType GetUnitType()
    {
        return this.unitType;
    }
    public UnitState GetUnitState()
    {
        return this.unitState;
    }
    private void Start()
    {

    }
    private void Update()
    {
        if(this.unitState.Equals(UnitState.Death))
            return;

        currentTarget = StageManager.Instance.ChangeTarget(this);
    }

    public void DoMove()
    {
        Vector2 direction = (currentTarget.transform.localPosition - this.transform.position).normalized;
        this.transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
    }
}