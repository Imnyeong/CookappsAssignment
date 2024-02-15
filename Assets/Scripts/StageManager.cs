using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    [Header("Singleton")]
    public static StageManager Instance;

    [Header("Unit Information")]
    [SerializeField] private List<Unit> characterList;
    [SerializeField] private List<Unit> enemyList;

    [Header("Stage Information")]
    [SerializeField] private StageState stageState = StageState.Ready;
    [SerializeField] private float limitTime;
    [SerializeField] private Text textTime;

    [Header("UI")]
    [SerializeField] private Slider charactersHpBar;
    [SerializeField] private Slider enemysHpBar;

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
        stageState = StageState.Ready;
        Init();
        textTime.text = limitTime.ToString();
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
        if (Instance == null)
        {
            Instance = this;
        }

        for (int i = 0; i < characterList.Count; i++ )
        {
            charactersMaxHp += characterList[i].GetMaxHp();
        }
        charactersCurrentHp = charactersMaxHp;

        for (int i = 0; i < enemyList.Count; i++)
        {
            enemysMaxHp += enemyList[i].GetMaxHp();
        }
        enemysCurrentHp = enemysMaxHp;

        stageState = StageState.Play;
    }
    public void SetStage(StageInfo _stageInfo)
    {
        enemyList.Clear();
        enemyList = _stageInfo.enemyList;
    }
    public Unit ChangeTarget(Unit _unit)
    {
        Unit target = null;
        float minDistance = float.MaxValue;

        List<Unit> targetList = new List<Unit>();

        switch (_unit.GetUnitType())
        {
            case UnitType.Character:
                targetList = enemyList;
                break;
            case UnitType.Enemy:
                targetList = characterList;
                break;
        }

        if(targetList == null)
        {
            return target;
        }

        for(int i = 0; i < targetList.Count; i++)
        {
            if(targetList[i].GetUnitState() != UnitState.Death)
            {
                float currentDistance = (_unit.transform.localPosition - targetList[i].transform.localPosition).sqrMagnitude;
                if (minDistance >= currentDistance)
                {
                    minDistance = currentDistance;
                    target = targetList[i];
                }
            }
        }
        return target;
    }
    public void CheckTeamHP(Unit _target, int _damage)
    {
        switch (_target.GetUnitType())
        {
            case UnitType.Character:
                {
                    charactersCurrentHp -= _damage;
                    charactersHpBar.value = charactersCurrentHp / charactersMaxHp;
                    if(charactersCurrentHp <= 0)
                    {
                        SetStageState(StageState.Defeat);
                    }
                    break;
                }
            case UnitType.Enemy:
                {
                    enemysCurrentHp -= _damage;
                    enemysHpBar.value = enemysCurrentHp / enemysMaxHp;
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
}


