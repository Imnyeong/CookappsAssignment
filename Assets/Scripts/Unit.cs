using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Unit Information")]
    [SerializeField] private UnitState unitState;
    [SerializeField] private UnitType unitType;
    [SerializeField] private Elemental elemental;
    [SerializeField] private Profession profession;

    [Header("Skill")]
    [SerializeField] private Skill skill;

    [Header("Unit Status")]
    [SerializeField] private int level;
    [SerializeField] private int hp;
    [SerializeField] private int attackPoint;
    [SerializeField] private float moveSpeed = 0.1f;
    [SerializeField] private float attackRange;

    [Header("Target")]
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
        TargetTimer();

        if (currentTarget != null)
        {
            if ((this.transform.localPosition - currentTarget.transform.localPosition).sqrMagnitude > attackRange * 1000.0f)
                DoMove();
        }            
    }
    #endregion
    public void TargetTimer()
    {
        targetTimer += Time.deltaTime;

        if (targetTimer >= targetDelay)
        {
            currentTarget = StageManager.Instance.ChangeTarget(this);
            targetTimer = 0.0f;
        }
    }
    public void DoMove()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, currentTarget.transform.position, moveSpeed);
    }
}