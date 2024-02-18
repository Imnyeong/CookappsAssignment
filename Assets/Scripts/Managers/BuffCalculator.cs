using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffCalculator : MonoBehaviour
{
    private List<Tuple<Elemental, int>> elementalTupleList;
    private List<Tuple<Profession, int>> professionTupleList;

    [SerializeField] private List<ElementalIcon> elementalIcons;
    [SerializeField] private List<ProfessionIcon> professionIcons;

    public List<Tuple<Elemental, int>> GetElementalTupleList()
    {
        return elementalTupleList;
    }
    public List<Tuple<Profession, int>> GetProfessionTupleList()
    {
        return professionTupleList;
    }
    public void CalculateElemental()
    {
        List<Unit> checkList = GameManager.Instance.characterArray.ToList();
        checkList.RemoveAll(x => x == null);

        elementalTupleList = new List<Tuple<Elemental, int>>();

        for (int i = 0; i < checkList.Count; i++)
        {
            if (checkList.Count == 0)
                break;
            if (checkList[i] != null)
            {
                Elemental checkElemental = checkList[i].GetUnitElemental();
                int count = checkList.Count(x => x.GetUnitElemental() == checkElemental);
                Tuple<Elemental, int> tmpTuple = new Tuple<Elemental, int>(checkElemental, count);
                elementalTupleList.Add(tmpTuple);
                checkList.RemoveAll(x => x.GetUnitElemental() == checkElemental);
                Debug.Log($"{checkElemental} 속성을 가진 {count}명");
            }
        }
    }
    public void CalculateProfession()
    {
        List<Unit> checkList = GameManager.Instance.characterArray.ToList();
        checkList.RemoveAll(x => x == null);

        professionTupleList = new List<Tuple<Profession, int>>();

        for (int i = 0; i < checkList.Count; i++)
        {
            if (checkList.Count == 0)
                break;
            if (checkList[i] != null)
            {
                Profession checkProfession = checkList[i].GetProfession();
                int count = checkList.Count(x => x.GetProfession() == checkProfession);
                Tuple<Profession, int> tmpTuple = new Tuple<Profession, int>(checkProfession, count);
                professionTupleList.Add(tmpTuple);
                checkList.RemoveAll(x => x.GetProfession() == checkProfession);
                Debug.Log($"{checkProfession} 직업을 가진 {count}명");
            }
        }
    }
    public void CheckBuff()
    {
        CalculateElemental();
        CalculateProfession();

        for (int i = 0; i < elementalTupleList.Count; i++)
        {
            if(elementalTupleList[i].Item2 > 1)
            {
                elementalIcons.Find(x => x.GetElemental() == elementalTupleList[i].Item1).SetElementalIcon(elementalTupleList[i].Item1, elementalTupleList[i].Item2);
            }
        }
        for (int i = 0; i < professionTupleList.Count; i++)
        {
            if (professionTupleList[i].Item2 > 1)
            {
                professionIcons.Find(x => x.GetProfession() == professionTupleList[i].Item1).SetProfessionIcon(professionTupleList[i].Item1, professionTupleList[i].Item2);
            }
        }
    }
}
