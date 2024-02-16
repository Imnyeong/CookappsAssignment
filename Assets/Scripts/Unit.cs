using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    [Header("Unit Information")]
    [SerializeField] private Sprite thumbnail;
    [SerializeField] private UnitState unitState;
    [SerializeField] private UnitType unitType;
    [SerializeField] private Elemental elemental;
    [SerializeField] private Profession profession;

    [Header("Skill")]
    [SerializeField] private Skill skill;
    [SerializeField] private Text skillLine;

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

    [Header("Animation")]
    [SerializeField] private Animator animator;

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
    public Sprite GetThumbnail()
    {
        return thumbnail;
    }
    public Skill GetSkill()
    {
        return skill;
    }
    public void SetUnitState(UnitState _state)
    {
        if (unitState == _state)
            return;

        unitState = _state;
        CheckAnimation(_state);
    }
    public void SetUnitInfo(Unit _unit)
    {
        this.thumbnail = _unit.thumbnail;
        this.unitState = _unit.unitState;
        this.unitType = _unit.unitType;
        this.elemental = _unit.elemental;
        this.profession = _unit.profession;

        this.skill = _unit.skill;
        this.skillLine.text = _unit.skill.line;

        this.level = _unit.level;
        this.maxHp = _unit.maxHp;
        this.currentHp = _unit.currentHp;
        this.attackPoint = _unit.attackPoint;
        this.moveSpeed = _unit.moveSpeed;
        this.attackRange = _unit.attackRange;
        this.attackDelay = _unit.attackDelay;

        this.animator = _unit.animator;

        this.GetComponentInChildren<SpriteRenderer>().sprite = _unit.GetComponentInChildren<SpriteRenderer>().sprite;
        this.GetComponentInChildren<Animator>().runtimeAnimatorController = _unit.GetComponentInChildren<Animator>().runtimeAnimatorController;
        this.animator = this.GetComponentInChildren<Animator>();
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
                SetUnitState(UnitState.Idle);
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
        if (_target == null)
            return;
    
        if(_target.currentHp + _skillValue > _target.maxHp)
        {
            _target.currentHp = _target.maxHp;
        }
        else
        {
            _target.currentHp += _skillValue;
        }
        _target.hpBar.value = _target.currentHp / _target.maxHp;
        StageManager.Instance.CheckTeamHP(_target, _skillValue * -1.0f);
    }
    public void DoSkill()
    {
        if (this.unitState != UnitState.Idle)
            return;

        StartCoroutine(SkillLine());
        switch (this.skill.skillType)
        {
            case SkillType.Attack:
                {
                    GetDamaged(currentTarget, this.skill.value);
                    break;
                }
            case SkillType.Heal:
                {
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
        float currentDamage = _target.currentHp < this.attackPoint ? _target.currentHp : this.attackPoint;

        _target.currentHp -= currentDamage;
        _target.hpBar.value = _target.currentHp <= 0.0f ? 0.0f : _target.currentHp / _target.maxHp;
        //Debug.Log($"{this.name} �� {_target.name}�� ������ ���� ü�� {_target.currentHp}");
        if (_target.currentHp <= 0)
        {
            _target.SetUnitState(UnitState.Death);
            this.SetUnitState(UnitState.Idle);
        }
        StageManager.Instance.CheckTeamHP(_target, currentDamage);
    }
    public void GetDamaged(Unit _target, float _skillValue)
    {
        float currentDamage = _target.currentHp < _skillValue ? _target.currentHp : _skillValue;

        _target.currentHp -= currentDamage;
        _target.hpBar.value = _target.currentHp <= 0.0f ? 0.0f : _target.currentHp / _target.maxHp;
        if (_target.currentHp <= 0)
        {
            _target.SetUnitState(UnitState.Death);
            this.SetUnitState(UnitState.Idle);
        }
        StageManager.Instance.CheckTeamHP(_target, currentDamage);
    }
    #endregion
    #region Animation
    public void CheckAnimation(UnitState _unitState)
    {
        //Debug.Log($"{this.name}�� ���¸�  {_unitState}�� ����");
        if (!isActiveAndEnabled)
            return;
        switch (_unitState)
        {
            case UnitState.Idle:
                {
                    animator.SetTrigger("Idle");
                    break;
                }
            case UnitState.Move:
                {
                    animator.SetTrigger("Move");
                    break;
                }
            case UnitState.Attack:
                {
                    animator.SetTrigger("Attack");
                    break;
                }
            case UnitState.Death:
                {
                    animator.SetTrigger("Death");
                    break;
                }
        }
    }
    public IEnumerator SkillLine()
    {
        skillLine.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1.0f);
        skillLine.gameObject.SetActive(false);
        StopCoroutine(SkillLine());
    }
    #endregion
}