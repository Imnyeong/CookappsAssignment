using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Singleton")]
    public static GameManager Instance;

    [Header("Unit Information")]
    public Unit[] characterArray;
    public StageInfo[] stageInfoArray;
    public int index;
    public SceneType sceneType;

    void Start()
    {
        //sceneType = SceneType.Lobby;
        DontDestroyOnLoad(this);
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void SetStageIndex(int _index)
    {
        index = _index;
    }
    //public void SetStage()
    //{
    //    StageManager.Instance.SetStage(index);
    //}
}
