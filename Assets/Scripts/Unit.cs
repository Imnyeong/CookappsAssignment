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
    [SerializeField] private float attackPoint;
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
    public float GetCurrentHp()
    {
        return this.currentHp;
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
            DoAttack();
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
    public void DoAttack()
    {
        SetUnitState(UnitState.Attack);
        GetDamaged(currentTarget);
    }
    public void DoHeal(Unit _target, float _skillValue)
    {
        if (_target.GetUnitState() == UnitState.Death)
            return;
    
        _target.currentHp += _skillValue;
        _target.hpBar.value = _target.currentHp / _target.maxHp;
        StageManager.Instance.CheckTeamHP(_target, _skillValue * -1.0f);
    }
    public void DoSkill()
    {
        if (unitState != UnitState.Attack)
            return;

        switch (this.skill.skillType)
        {
            case SkillType.Attack:
                {
                    GetDamaged(currentTarget, this.skill.value);
                    break;
                }
            case SkillType.Heal:
                {
                    //GetDamaged(StageManager.Instance.FindLowHP(this.unitType), this.skill.value * -1.0f);
                    DoHeal(StageManager.Instance.FindLowHP(this.unitType), this.skill.value);
                    break;
                }
            case SkillType.Special:
                {
                    break;
                }
        }
        attackTimer = 0.0f;
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
    public void GetDamaged(Unit _target, float _skillValue)
    {
        _target.currentHp -= _skillValue;
        _target.hpBar.value = _target.currentHp / _target.maxHp;
        StageManager.Instance.CheckTeamHP(_target, _skillValue);
        if (_target.currentHp <= 0)
        {
            _target.SetUnitState(UnitState.Death);
        }
    }
    #endregion
}