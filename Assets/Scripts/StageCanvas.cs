using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageCanvas : MonoBehaviour
{
    [SerializeField] private GameObject gameEndPopup;

    public void ShowPopup()
    {
        gameEndPopup.SetActive(true);
    }
    public void HidePopup()
    {
        gameEndPopup.SetActive(false);
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
}
