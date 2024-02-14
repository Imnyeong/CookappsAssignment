using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("유닛 정보")]
    [SerializeField] private List<Unit> characterList;
    [SerializeField] private List<Unit> enemyList;

    [Header("스테이지 정보")]
    [SerializeField] private StageState state = StageState.Ready;

    void Start()
    {
        state = StageState.Ready;
        StartStage();
    }
    void Update()
    {

    }

    public void StartStage()
    {
        //characterList.Clear();
        //enemyList.Clear();

        for(int i = 0; i < characterList.Count; i++)
        {
            characterList[i].ChangeTarget(enemyList);
        }
        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].ChangeTarget(characterList);
        }
    }
}


