using UnityEngine;
using UnityEngine.UI;

public class ElementalIcon : MonoBehaviour
{
    [SerializeField] private Elemental elemental;
    [SerializeField] private Text buffName;
    [SerializeField] private Text count;

    public Elemental GetElemental()
    {
        return elemental;
    }
    public void SetElementalIcon(Elemental _elemental, int _count)
    {
        switch(_elemental)
        {
            case Elemental.Fire:
                {
                    buffName.text = "ºÒ";
                    break;
                }
            case Elemental.Water:
                {
                    buffName.text = "¹°";
                    break;
                }
            case Elemental.Grass:
                {
                    buffName.text = "Ç®";
                    break;
                }
        }
        count.text = _count.ToString();
        this.gameObject.SetActive(true);
    }
}
