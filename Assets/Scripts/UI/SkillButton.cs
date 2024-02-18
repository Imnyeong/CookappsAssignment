using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class SkillButton : Button
{
    private Unit unit;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject block;

    #region Get or Set
    public void SetUnit(Unit _unit)
    {
        unit = _unit;
        spriteRenderer.sprite = _unit.GetThumbnail();
        this.gameObject.SetActive(_unit != null);
    }
    #endregion
    #region Interaction
    public void DoSkill()
    {
        if (unit == null || StageManager.Instance.GetStageState() != StageState.Play ||
            ((unit.GetUnitState() == UnitState.Death) || (unit.GetUnitState() == UnitState.Skill)))
            return;

        unit.OnClickSkill();
        StartCoroutine(CoolDown());
    }
    private IEnumerator CoolDown()
    {
        this.interactable = false;
        block.SetActive(true);

        yield return new WaitForSeconds(unit.GetSkill().coolDown);

        this.interactable = true;
        block.SetActive(false);
    }
    #endregion
}
