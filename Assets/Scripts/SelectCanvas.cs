using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectCanvas : MonoBehaviour
{
    [SerializeField] private Button startButton;
    private void Start()
    {
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(DoStart);
    }

    public void DoStart()
    {
        GameManager.Instance.sceneType = SceneType.InGame;
        SceneManager.LoadScene("InGame");
    }
}
