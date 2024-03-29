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
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackDelay;

    [Header("UI")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Slider hpBar;

    [Header("Target")]
    [SerializeField] private Unit currentTarget = null;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private float targetDelay = 0.1f;
    private float skillDelay = 0.5f;
    private bool usingSkill = false;

    #region Get or Set
    public UnitType GetUnitType()
    {
        return this.unitType;
    }
    public Elemental GetUnitElemental()
    {
        return this.elemental;
    }
    public Profession GetProfession()
    {
        return this.profession;
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
        this.gameObject.name = _unit.gameObject.name;
        this.thumbnail = _unit.thumbnail;
        this.unitState = _unit.unitState;
        this.unitType = _unit.unitType;
        this.elemental = _unit.elemental;
        this.profession = _unit.profession;

        if (_unit.skill != null)
        {
            this.skill = _unit.skill;
            this.skillLine.text = _unit.skill.line;
        }
        this.level = _unit.level;
        this.maxHp = _unit.maxHp;
        this.currentHp = _unit.currentHp;
        this.attackPoint = _unit.attackPoint;
        this.moveSpeed = _unit.moveSpeed;
        this.attackRange = _unit.attackRange;
        this.attackDelay = _unit.attackDelay;

        this.animator = _unit.animator;

        this.spriteRenderer.sprite = _unit.spriteRenderer.sprite;
        this.GetComponentInChildren<Animator>().runtimeAnimatorController = _unit.GetComponentInChildren<Animator>().runtimeAnimatorController;
        this.animator = this.GetComponentInChildren<Animator>();
    }
    #endregion
    #region Unity Life Cycle
    private void Start()
    {
        currentHp = maxHp;

        SetUnitState(UnitState.Idle);
        StartCoroutine(TargetTimer());
    }
    #endregion
    #region Timer
    private IEnumerator TargetTimer()
    {
        currentTarget = StageManager.Instance.ChangeTarget(this);
        if (currentTarget != null)
        {
            if (CheckRange())
            {
                SetUnitState(UnitState.Idle);
                yield return StartCoroutine(AttackTimer());
            }
            else
            {
                DoMove();
            }
            yield return new WaitForSecondsRealtime(targetDelay);
            StartCoroutine(TargetTimer());
        }
        else
        {
            SetUnitState(UnitState.Idle);
            StopCoroutine(TargetTimer());
        }
    }
    public IEnumerator AttackTimer()
    {
        DoAttack();
        if (currentTarget.GetUnitState() != UnitState.Death)
        {
            yield return new WaitForSecondsRealtime(attackDelay);
        }
        StopCoroutine(AttackTimer());
    }
    public IEnumerator SkillTimer()
    {
        if(usingSkill)
        {
            StopCoroutine(SkillTimer());
        }
        else
        {
            usingSkill = true;
            skillLine.gameObject.SetActive(false);
            StopCoroutine(AttackTimer());
            
            DoSkill();
            if (currentTarget.GetUnitState() != UnitState.Death)
            {
                yield return new WaitForSecondsRealtime(skillDelay);
            }
            usingSkill = false;
            skillLine.gameObject.SetActive(false);
            StopCoroutine(SkillTimer());
        }
    }
    public IEnumerator DeathCoroutine()
    {
        skillLine.gameObject.SetActive(false);
        SetUnitState(UnitState.Death);
        StageManager.Instance.CheckTeamHP(this);
        GetComponent<CapsuleCollider2D>().enabled = false;
        if (this.unitType == UnitType.Character && this.skill != null)
        {
            StageManager.Instance.DisableSkill(this);
        }
        yield return new WaitForSecondsRealtime(1.0f);
        animator.runtimeAnimatorController = null;
        StopCoroutine(DeathCoroutine());
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
        GameManager.Instance.audioManager.playEffect(this.gameObject.name);
    }
    public void DoHeal(Unit _target, float _skillValue)
    {
        if (_target == null || _target.GetUnitState() == UnitState.Death || _target.currentHp <= 0)
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
        StageManager.Instance.CheckTeamHP(_target);
    }
    public void DoSkill()
    {
        SetUnitState(UnitState.Skill);
        skillLine.gameObject.SetActive(true);
        GameManager.Instance.audioManager.SkillEffectSound();

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
                    ChangePosition(StageManager.Instance.FindLowHP(this.unitType));
                    break;
                }
        }
    }
    public void DoDeath()
    {
        StopAllCoroutines();
        StartCoroutine(DeathCoroutine());
    }
    public void ChangePosition(Unit _unit)
    {
        if (_unit == null)
            return;

        Vector3 tmpTransform;
        
        tmpTransform = this.GetComponent<RectTransform>().localPosition;
        this.GetComponent<RectTransform>().localPosition = _unit.GetComponent<RectTransform>().localPosition;
        _unit.GetComponent<RectTransform>().localPosition = tmpTransform;
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
        //Debug.Log($"{this.name} 가 {_target.name}을 때려서 남은 체력 {_target.currentHp}");

        if (_target.currentHp <= 0)
        {
            _target.DoDeath();
        }
        StageManager.Instance.CheckTeamHP(_target);
    }
    public void GetDamaged(Unit _target, float _skillValue)
    {
        float currentDamage = _target.currentHp < _skillValue ? _target.currentHp : _skillValue;

        _target.currentHp -= currentDamage;
        _target.hpBar.value = _target.currentHp <= 0.0f ? 0.0f : _target.currentHp / _target.maxHp;

        if (_target.currentHp <= 0)
        {
            _target.DoDeath();
        }
        StageManager.Instance.CheckTeamHP(_target);
    }
    public void OnClickSkill()
    {
        StartCoroutine(SkillTimer());
    }
    #endregion
    #region Animation
    public void CheckAnimation(UnitState _unitState)
    {
        //Debug.Log($"{this.name}의 상태를  {_unitState}로 변경");
        if (!isActiveAndEnabled)
            return;

        if (unitState == UnitState.Death && _unitState != UnitState.Death)
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
            case UnitState.Skill:
                {
                    animator.SetTrigger("Skill");
                    break;
                }
            case UnitState.Death:
                {
                    animator.SetTrigger("Death");
                    break;
                }
        }
    }
    #endregion
}