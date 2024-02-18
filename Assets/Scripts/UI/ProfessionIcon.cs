using UnityEngine;
using UnityEngine.UI;

public class ProfessionIcon : MonoBehaviour
{
    [SerializeField] private Profession profession;
    [SerializeField] private Text buffName;
    [SerializeField] private Text count;

    #region Get or Set
    public Profession GetProfession()
    {
        return profession;
    }
    public void SetProfessionIcon(Profession _profession, int _count)
    {
        switch (_profession)
        {
            case Profession.Warrior:
                {
                    buffName.text = "��";
                    break;
                }
            case Profession.Tanker:
                {
                    buffName.text = "��";
                    break;
                }
            case Profession.Supporter:
                {
                    buffName.text = "��";
                    break;
                }
        }
        count.text = _count.ToString();
        this.gameObject.SetActive(true);
    }
    #endregion
}
