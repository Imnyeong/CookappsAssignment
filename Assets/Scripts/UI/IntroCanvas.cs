using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroCanvas : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        startButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();

        startButton.onClick.AddListener(delegate
        {
            GameManager.Instance.audioManager.clickEffectSound();
            SceneManager.LoadScene("Lobby");
        });
        exitButton.onClick.AddListener(delegate 
        {
            GameManager.Instance.audioManager.clickEffectSound();
            Application.Quit();
        });
    }
}
