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

    #region Get or Set
    public void SetStageIndex(int _index)
    {
        index = _index;
    }
    #endregion
    #region Unity Life Cycle
    void Start()
    {
        SetResolution();
        DontDestroyOnLoad(this);
        if (Instance == null)
        {
            Instance = this;
        }
    }
    #endregion
    #region Data

    public void ClearData()
    {
        for (int i = 0; i < characterArray.Length; i++)
        {
            characterArray[i] = null;
        }
    }
    #endregion
    #region Setting
    public void SetResolution()
    {
        int setWidth = 1920;
        int setHeight = 1080;

        int deviceWidth = Screen.width;
        int deviceHeight = Screen.height;

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight)
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight);
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);
        }
        else
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight);
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight);
        }
    }
    #endregion
}
