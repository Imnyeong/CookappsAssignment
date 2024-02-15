using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("Singleton")]
    public static StageManager Instance;

    [Header("Unit Information")]
    [SerializeField] private List<Unit> characterList;
    [SerializeField] private List<Unit> enemyList;

    [Header("Stage Information")]
    [SerializeField] private StageState state = StageState.Ready;

    #region Unity Life Cycle
    void Start()
    {
        Init();
    }
    #endregion
    private void Init()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        state = StageState.Ready;
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

    public void CheckStageState()
    {
        //if()
    }
}


