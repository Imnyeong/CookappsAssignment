using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private GameObject gameEndPopup;
    [SerializeField] private Text gameEndText;

    private float charactersMaxHp;
    private float charactersCurrentHp;

    private float enemysMaxHp;
    private float enemysCurrentHp;
    private float oneSec = 1.0f;

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
        SetStage(GameManager.Instance.index);
    }
    #endregion
    #region Stage
    public void SetStage(int _index)
    {
        for (int i = 0; i < GameManager.Instance.characterArray.Length; i++)
        {
            if (GameManager.Instance.characterArray[i] != null)
            {
                characterArray[i].SetUnitInfo(GameManager.Instance.characterArray[i]);
                characterArray[i].gameObject.SetActive(true);
                SkillButtons[i].SetUnit(characterArray[i]);
                charactersMaxHp += characterArray[i].GetMaxHp();
            }
        }
        charactersCurrentHp = charactersMaxHp;

        for (int i = 0; i < GameManager.Instance.stageInfoArray[_index].enemyArray.Length; i++)
        {
            if (GameManager.Instance.stageInfoArray[_index].enemyArray[i] != null)
            {
                enemyArray[i].SetUnitInfo(GameManager.Instance.stageInfoArray[_index].enemyArray[i]);
                enemyArray[i].gameObject.SetActive(true);
                enemysMaxHp += enemyArray[i].GetMaxHp();
            }
        }
        enemysCurrentHp = enemysMaxHp;

        limitTime = GameManager.Instance.stageInfoArray[_index].limitTime;
        Init();
    }
    private void Init()
    {
        textTime.text = limitTime.ToString();
        stageState = StageState.Play;
        StartCoroutine(StageTimer());
    }
    private IEnumerator StageTimer()
    {
        if (stageState != StageState.Play)
            StopCoroutine(StageTimer());

        limitTime -= oneSec;
        textTime.text = ((int)limitTime).ToString();

        if (limitTime <= 0)
        {
            SetStageState(StageState.Defeat);
            StopCoroutine(StageTimer());
        }
        yield return new WaitForSecondsRealtime(oneSec);
        StartCoroutine(StageTimer());
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
                        StopAllCoroutines();
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
                        StopAllCoroutines();
                    }
                    break;
                }
        }
    }
    public void SetStageState(StageState _stageState)
    {
        switch (_stageState)
        {
            case StageState.Clear:
                {
                    stageState = StageState.Clear;
                    gameEndText.text = "전투 승리";
                    break;
                }
            case StageState.Defeat:
                {
                    stageState = StageState.Defeat;
                    gameEndText.text = "전투 패배";
                    break;
                }
        }
        gameEndPopup.SetActive(true);
    }
    public void RetryStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void BackToLobby()
    {
        GameManager.Instance.ClearData();
        GameManager.Instance.sceneType = SceneType.Lobby;
        SceneManager.LoadScene("Lobby");
    }
    #endregion
    #region Unit
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
                        if (characterArray[i].gameObject.activeSelf && characterArray[i].GetUnitState() != UnitState.Death)
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
    public Unit FindLowHP(UnitType _unitType)
    {
        Unit target = null;
        float minHp = float.MaxValue;

        switch (_unitType)
        {
            case UnitType.Character:
                {
                    for (int i = 0; i < characterArray.Length; i++)
                    {
                        if (characterArray[i].gameObject.activeSelf && characterArray[i].GetUnitState() != UnitState.Death)
                        {
                            if (characterArray[i].GetCurrentHp() < minHp)
                            {
                                target = characterArray[i];
                                minHp = characterArray[i].GetCurrentHp();
                            }
                        }
                    }
                    break;
                }
            case UnitType.Enemy:
                {
                    for (int i = 0; i < enemyArray.Length; i++)
                    {
                        if (enemyArray[i].gameObject.activeSelf && enemyArray[i].GetUnitState() != UnitState.Death)
                        {
                            if (enemyArray[i].GetCurrentHp() < minHp)
                            {
                                target = enemyArray[i];
                                minHp = enemyArray[i].GetCurrentHp();
                            }
                        }
                    }
                    break;
                }
        }
        return target;
    }
    #endregion
}