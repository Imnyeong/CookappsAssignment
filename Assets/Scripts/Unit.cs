using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    [SerializeField] private int attackPoint;
    [SerializeField] private float moveSpeed = 0.1f;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackDelay;

    [Header("UI")]
    [SerializeField] private Slider hpBar;

    [Header("Target")]
    [SerializeField] private Unit currentTarget = null;

    private float targetDelay = 0.3f;
    private float targetTimer = 0.0f;
    private float attackTimer = 0.0f;

    #region Get or Set
    public UnitType GetUnitType()
    {
        return this.unitType;
    }
    public UnitState GetUnitState()
    {
        return this.unitState;
    }
    public float GetMaxHp()
    {
        return this.maxHp;
    }
    public void SetUnitState(UnitState _state)
    {
        if (unitState != _state)
            unitState = _state;
    }
    #endregion
    #region Unity Life Cycle
    private void Start()
    {
        currentHp = maxHp;
        SetUnitState(UnitState.Idle);
    }
    private void Update()
    {
        if (StageManager.Instance.GetStageState() != StageState.Play)
            return;
        if (unitState == UnitState.Death)
            return;

        if (TargetTimer() != null)
        {
            if(CheckRange())
            {
                AttackTimer();
            }
            else
            {
                DoMove();
            }
        }
        else
        {
            SetUnitState(UnitState.Idle);
        }
    }
    #endregion
    #region Timer
    public Unit TargetTimer()
    {
        targetTimer += Time.deltaTime;

        if (targetTimer >= targetDelay)
        {
            currentTarget = StageManager.Instance.ChangeTarget(this);
            targetTimer = 0.0f;
        }
        return currentTarget;
    }
    public void AttackTimer()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackDelay)
        {
            DoAttack(currentTarget);
            attackTimer = 0.0f;
        }
    }
    #endregion
    #region DoAction
    public void DoMove()
    {
        SetUnitState(UnitState.Move);
        this.transform.position = Vector3.MoveTowards(this.transform.position, currentTarget.transform.position, moveSpeed);
    }
    public void DoAttack(Unit _target)
    {
        SetUnitState(UnitState.Attack);
        GetDamaged(_target);
    }
    #endregion
    #region Interaction
    public bool CheckRange()
    {
        return (this.transform.localPosition - currentTarget.transform.localPosition).sqrMagnitude < attackRange;
    }
    public void GetDamaged(Unit _target)
    {
        _target.currentHp -= this.attackPoint;
        _target.hpBar.value = _target.currentHp / _target.maxHp;
        StageManager.Instance.CheckTeamHP(_target, this.attackPoint);
        if (_target.currentHp <= 0)
        {
            _target.SetUnitState(UnitState.Death);
        }
    }
    #endregion
}