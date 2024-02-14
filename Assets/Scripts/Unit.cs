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
    [SerializeField] private float moveSpeed = 0.01f;
    [SerializeField] private int AttackRange;

    [Header("���� Ÿ��")]
    [SerializeField] private Unit currentTarget = null;
    [SerializeField] private float targetTimer = 0.0f;
    [SerializeField] private float targetDelay = 0.3f;

    #region Get or Set
    public UnitType GetUnitType()
    {
        return this.unitType;
    }
    public UnitState GetUnitState()
    {
        return this.unitState;
    }
    #endregion
    #region Unity Life Cycle
    private void Update()
    {
        targetTimer += Time.deltaTime;

        if(targetTimer >= targetDelay)
        {
            currentTarget = StageManager.Instance.ChangeTarget(this);
            targetTimer = 0.0f;
        }

        //if (currentTarget != null)
             //DoMove();
    }
    #endregion
    public void DoMove()
    {
        Vector2 vector = currentTarget.transform.localPosition - this.transform.position;
        float distance = vector.sqrMagnitude;
        this.transform.position += (Vector3)vector * moveSpeed * Time.deltaTime;
    }
}