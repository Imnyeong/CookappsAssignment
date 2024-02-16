using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    [Header("Singleton")]
    public static StageManager Instance;

    [Header("Unit Information")]
    [SerializeField] private Unit[] characterArray;
    [SerializeField] private Unit[] enemyArray;

    [Header("Stage Information")]
    [SerializeField] private StageState stageState = StageState.Ready;
    [SerializeField] private float limitTime;
    [SerializeField] private Text textTime;

    [Header("UI")]
    [SerializeField] private Slider charactersHpBar;
    [SerializeField] private Slider enemysHpBar;
    [SerializeField] private SkillButton[] SkillButtons;

    private float charactersMaxHp;
    private float charactersCurrentHp;

    private float enemysMaxHp;
    private float enemysCurrentHp;

    #region Get or Set
    public StageState GetStageState()
    {
        return stageState;
    }
    #endregion
    #region Unity Life Cycle
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        stageState = StageState.Ready;
        Init();
    }
    private void Update()
    {
        if(stageState == StageState.Play)
        CheckTime();
    }
    #endregion
    private void CheckTime()
    {
        limitTime -= Time.deltaTime;
        textTime.text = ((int)limitTime).ToString();
        if(limitTime <= 0)
        {
            SetStageState(StageState.Defeat);
        }
    }
    private void Init()
    {
        textTime.text = limitTime.ToString();
        stageState = StageState.Play;
    }
    public void SetStage(int _index)
    {
        for(int i = 0; i < DataBase.Instance.characterArray.Length; i++)
        {
            if(DataBase.Instance.characterArray[i] != null)
            {
                characterArray[i].SetUnitInfo(DataBase.Instance.characterArray[i]);
                characterArray[i].gameObject.SetActive(true);
                SkillButtons[i].SetUnit(characterArray[i]);
                charactersMaxHp += characterArray[i].GetMaxHp();
            }
        }
        charactersCurrentHp = charactersMaxHp;

        for(int i = 0; i < DataBase.Instance.stageInfoArray[_index].enemyArray.Length; i++)
        {
            if (DataBase.Instance.stageInfoArray[_index].enemyArray[i] != null)
            {
                enemyArray[i].SetUnitInfo(DataBase.Instance.stageInfoArray[_index].enemyArray[i]);
                enemyArray[i].gameObject.SetActive(true);
                enemysMaxHp += enemyArray[i].GetMaxHp();
            }
        }
        enemysCurrentHp = enemysMaxHp;

        limitTime = DataBase.Instance.stageInfoArray[_index].limitTime;
        Init();
    }

    public Unit ChangeTarget(Unit _unit)
    {
        Unit target = null;
        float minDistance = float.MaxValue;

        switch (_unit.GetUnitType())
        {
            case UnitType.Character:
                {
                    for (int i = 0; i < enemyArray.Length; i++)
                    {
                        if (enemyArray[i].gameObject.activeSelf && enemyArray[i].GetUnitState() != UnitState.Death)
                        {
                            float currentDistance = (_unit.transform.localPosition - enemyArray[i].transform.localPosition).sqrMagnitude;
                            if (minDistance >= currentDistance)
                            {
                                minDistance = currentDistance;
                                target = enemyArray[i];
                            }
                        }                       
                    }
                    break;
                }
            case UnitType.Enemy:
                {
                    for (int i = 0; i < characterArray.Length; i++)
                    {
                        if(characterArray[i].gameObject.activeSelf && characterArray[i].GetUnitState() != UnitState.Death)
                        {
                            float currentDistance = (_unit.transform.localPosition - characterArray[i].transform.localPosition).sqrMagnitude;
                            if (minDistance >= currentDistance)
                            {
                                minDistance = currentDistance;
                                target = characterArray[i];
                            }
                        }
                    }
                    break;
                }
        }
        return target;
    }
    public void CheckTeamHP(Unit _target, float _damage)
    {
        switch (_target.GetUnitType())
        {
            case UnitType.Character:
                {
                    charactersCurrentHp -= _damage;
                    //Debug.Log($"유저 팀 남은 체력 {charactersCurrentHp}");
                    charactersHpBar.value = charactersCurrentHp <= 0.0f ? 0.0f : charactersCurrentHp / charactersMaxHp;
                    if(charactersCurrentHp <= 0)
                    {
                        SetStageState(StageState.Defeat);
                    }
                    break;
                }
            case UnitType.Enemy:
                {
                    enemysCurrentHp -= _damage;
                    //Debug.Log($"상대 팀 남은 체력 {enemysCurrentHp}");
                    enemysHpBar.value = enemysCurrentHp <= 0.0f ? 0.0f : enemysCurrentHp / enemysMaxHp;
                    if (enemysCurrentHp <= 0)
                    {
                        SetStageState(StageState.Clear);
                    }
                    break;
                }
        }
    }
    public void SetStageState(StageState _stageState)
    {
        switch(_stageState)
        {
            case StageState.Clear:
                {
                    stageState = StageState.Clear;
                    Debug.Log("Clear");
                    break;
                }
            case StageState.Defeat:
                {
                    stageState = StageState.Defeat;
                    Debug.Log("Defeat");
                    break;
                }
        }
    }
    public Unit FindLowHP(UnitType _unitType)
    {
        Unit target = null;
        switch (_unitType)
        {
            case UnitType.Character:
                {
                    float defaultHp = float.MaxValue;

                    for (int i = 0; i < characterArray.Length; i++)
                    {
                        if(characterArray[i] != null)
                        {
                            if (characterArray[i].GetCurrentHp() < defaultHp)
                            {
                                target = characterArray[i];
                            }
                        }
                    }
                    break;
                }
            case UnitType.Enemy:
                {
                    float defaultHp = enemyArray[0].GetCurrentHp();

                    for (int i = 0; i < enemyArray.Length; i++)
                    {
                        if(enemyArray[i] != null)
                        {
                            if (enemyArray[i].GetCurrentHp() < defaultHp)
                            {
                                target = enemyArray[i];
                            }
                        }
                    }
                    break;
                }
        }
        return target;
    }
}


