using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectCanvas : MonoBehaviour
{
    [SerializeField] private Button startButton;

    #region Unity Life Cycle
    private void Start()
    {
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(DoStart);
    }
    #endregion
    #region Interaction
    public void DoStart()
    {
        int characterCount = 0;
        for(int i = 0; i < GameManager.Instance.characterArray.Length; i++)
        {
            if (GameManager.Instance.characterArray[i] != null)
                characterCount++;
        }
        if (characterCount == 0)
            return;

        GameManager.Instance.sceneType = SceneType.InGame;
        SceneManager.LoadScene("Stage");
    }
    #endregion
}
