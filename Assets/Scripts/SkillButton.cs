using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SkillButton : Button
{
    private Unit unit;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public void SetUnit(Unit _unit)
    {
        unit = _unit;
        spriteRenderer.sprite = _unit.GetThumbnail();
        this.gameObject.SetActive(_unit != null);
    }
    public void DoSkill()
    {
        if (unit == null)
            return;
        unit.DoSkill();
    }
}
