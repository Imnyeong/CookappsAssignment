using UnityEngine;
using UnityEngine.UI;

public class DataBase : MonoBehaviour
{
    [Header("Singleton")]
    public static DataBase Instance;

    [Header("Unit Information")]
    public Unit[] characterArray;
    public StageInfo[] stageInfoArray;
    [SerializeField] private int index;

    void Start()
    {
        DontDestroyOnLoad(this);
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void SetStage()
    {
        StageManager.Instance.SetStage(index);
    }
}
