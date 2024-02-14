using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("싱글턴")]
    public static StageManager Instance;

    [Header("유닛 정보")]
    [SerializeField] private List<Unit> characterList;
    [SerializeField] private List<Unit> enemyList;

    [Header("스테이지 정보")]
    [SerializeField] private StageState state = StageState.Ready;

    void Start()
    {
        Init();
    }
    void Update()
    {

    }

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

        if(targetList.Equals(null))
        {
            return target;
        }

        for(int i = 0; i < targetList.Count; i++)
        {
            if(!targetList[i].GetUnitState().Equals(UnitState.Death))
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
}


