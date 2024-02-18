
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [Header("Singleton")]
    public static LobbyManager Instance;

    [Header("Canvas")]
    [SerializeField] private GameObject IntroCanvas;
    [SerializeField] private GameObject stageCanvas;
    [SerializeField] private GameObject characterCanvas;

    [Header("Stage Buttons")]
    [SerializeField] private Button[] StageButtons;

    [Header("Unit Information")]
    [SerializeField] private Unit[] enemyArray;

    private LobbyState lobbyState;
    [SerializeField] private Unit selectedUnit;
    [SerializeField] private SelectButton selectedButton;


    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        lobbyState = LobbyState.Intro;
        selectedUnit = null;
    }

    public void ChangeLobbyState(LobbyState _lobbyState)
    {
        IntroCanvas.SetActive(_lobbyState == LobbyState.Intro);
        stageCanvas.SetActive(_lobbyState == LobbyState.StageSelect);
        characterCanvas.SetActive(_lobbyState == LobbyState.CharacterSelect || _lobbyState == LobbyState.PositionSelect);
    }

    public void OnClickStageButton(int _index)
    {
        GameManager.Instance.SetStageIndex(_index);
        SetStage(_index);

        ChangeLobbyState(LobbyState.CharacterSelect);
    }

    public void SetStage(int _index)
    {
        for (int i = 0; i < GameManager.Instance.stageInfoArray[_index].enemyArray.Length; i++)
        {
            if (GameManager.Instance.stageInfoArray[_index].enemyArray[i] != null)
            {
                enemyArray[i].SetUnitInfo(GameManager.Instance.stageInfoArray[_index].enemyArray[i]);
                enemyArray[i].gameObject.SetActive(true);
            }
        }
    }

    public void SelectCharacter(Unit _unit)
    {
        selectedUnit = _unit;
    }
    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
    public void SelectedButton(SelectButton _selectedButton)
    {
        selectedButton = _selectedButton;
    }
    public SelectButton GetSelectedButton()
    {
        return selectedButton;
    }
    public void UnSelectCharacter()
    {
        if(selectedButton != null)
        {
            selectedButton.UnSelect();
        }
    }
}
