using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class SkillButton : Button
{
    private Unit unit;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject block;
    public void SetUnit(Unit _unit)
    {
        unit = _unit;
        spriteRenderer.sprite = _unit.GetThumbnail();
        this.gameObject.SetActive(_unit != null);
    }
    public void DoSkill()
    {
        if (unit == null || StageManager.Instance.GetStageState() != StageState.Play)
            return;
        unit.DoSkill();

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
}
