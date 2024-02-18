using UnityEngine;
using UnityEngine.UI;

public class SelectButton : Button
{
    [SerializeField] private Unit unit;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject block;

    [SerializeField] private SelectState selectState;

    #region Interaction
    public void OnClickCharacter()
    {
        if (selectState == SelectState.Unselected)
        {
            if (LobbyManager.Instance.GetSelectedButton() != null)
            {
                LobbyManager.Instance.GetSelectedButton().UnSelect();
            }
            selectState = SelectState.Selected;
            LobbyManager.Instance.SelectCharacter(unit);
            LobbyManager.Instance.SelectedButton(this);
        }
        else
        {
            selectState = SelectState.Unselected;
            LobbyManager.Instance.SelectCharacter(null);
            LobbyManager.Instance.SelectedButton(null);
        }
        block.gameObject.SetActive(selectState == SelectState.Selected);
    }
    public void UnSelect()
    {
        selectState = SelectState.Unselected;
        LobbyManager.Instance.SelectCharacter(null);
        LobbyManager.Instance.SelectedButton(null);
        block.gameObject.SetActive(selectState == SelectState.Selected);
    }
    #endregion
}
