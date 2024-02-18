using UnityEngine;
using UnityEngine.UI;

public class IntroCanvas : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        startButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();

        startButton.onClick.AddListener(delegate { LobbyManager.Instance.ChangeLobbyState(LobbyState.StageSelect); });
        exitButton.onClick.AddListener(delegate { Application.Quit(); });
    }
}
