using UnityEngine;
using UnityEngine.UI;

public class PositionButton : Button
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private PositionState positionState;

    private void Start()
    {
        ChangeState(PositionState.Unselected);
    }
    public void ChangeState(PositionState _positionState)
    {
        positionState = _positionState;
        spriteRenderer.gameObject.SetActive(_positionState == PositionState.Selected);
    }
    public void SetUnit(Unit _unit)
    {
        if (_unit != null)
        {
            spriteRenderer.sprite = _unit.GetThumbnail();
        }
        spriteRenderer.gameObject.SetActive(_unit != null);
    }
    public void OnClickPosition(int _index)
    {
        if (positionState == PositionState.Unselected)
        {
            if (LobbyManager.Instance.GetSelectedButton() == null)
                return;

            positionState = PositionState.Selected;
            GameManager.Instance.characterArray[_index] = LobbyManager.Instance.GetSelectedUnit();
            SetUnit(LobbyManager.Instance.GetSelectedUnit());
        }
        else
        {
            if (LobbyManager.Instance.GetSelectedUnit() != null)
            {
                GameManager.Instance.characterArray[_index] = LobbyManager.Instance.GetSelectedUnit();
                SetUnit(LobbyManager.Instance.GetSelectedUnit());
            }
            else
            {
                positionState = PositionState.Unselected;
                GameManager.Instance.characterArray[_index] = null;
                SetUnit(null);
            }
        }
        LobbyManager.Instance.UnSelectCharacter();
    }
}
