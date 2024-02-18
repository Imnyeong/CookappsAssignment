using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffCalculator : MonoBehaviour
{
    private List<Tuple<Elemental, int>> elementalTupleList;
    private List<Tuple<Profession, int>> professionTupleList;


    [Header("User")]
    [SerializeField] private List<ElementalIcon> elementalIcons;
    [SerializeField] private List<ProfessionIcon> professionIcons;

    [Header("Enemy")]
    [SerializeField] private List<ElementalIcon> enemyElementalIcons;
    [SerializeField] private List<ProfessionIcon> enemyProfessionIcons;

    #region Calculate
    public void CalculateElemental(UnitType _unitType)
    {
        List<Unit> checkList = new List<Unit>();
        if (_unitType == UnitType.Character)
        {
            checkList = GameManager.Instance.characterArray.ToList();
        }
        else
        {
            checkList = GameManager.Instance.stageInfoArray[GameManager.Instance.GetStageIndex()].enemyArray.ToList();
        }
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
            }
        }

        if (_unitType == UnitType.Character)
        {
            for (int i = 0; i < elementalTupleList.Count; i++)
            {
                if (elementalTupleList[i].Item2 > 1)
                {
                    elementalIcons.Find(x => x.GetElemental() == elementalTupleList[i].Item1).SetElementalIcon(elementalTupleList[i].Item1, elementalTupleList[i].Item2);
                }
            }
        }
        else
        {
            for (int i = 0; i < elementalTupleList.Count; i++)
            {
                if (elementalTupleList[i].Item2 > 1)
                {
                    enemyElementalIcons.Find(x => x.GetElemental() == elementalTupleList[i].Item1).SetElementalIcon(elementalTupleList[i].Item1, elementalTupleList[i].Item2);
                }
            }
        }
    }
    public void CalculateProfession(UnitType _unitType)
    {
        List<Unit> checkList = new List<Unit>();
        if (_unitType == UnitType.Character)
        {
            checkList = GameManager.Instance.characterArray.ToList();
        }
        else
        {
            checkList = GameManager.Instance.stageInfoArray[GameManager.Instance.GetStageIndex()].enemyArray.ToList();
        }
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
            }
        }

        if (_unitType == UnitType.Character)
        {
            for (int i = 0; i < professionTupleList.Count; i++)
            {
                if (professionTupleList[i].Item2 > 1)
                {
                    professionIcons.Find(x => x.GetProfession() == professionTupleList[i].Item1).SetProfessionIcon(professionTupleList[i].Item1, professionTupleList[i].Item2);
                }
            }
        }
        else
        {
            for (int i = 0; i < professionTupleList.Count; i++)
            {
                if (professionTupleList[i].Item2 > 1)
                {
                    enemyProfessionIcons.Find(x => x.GetProfession() == professionTupleList[i].Item1).SetProfessionIcon(professionTupleList[i].Item1, professionTupleList[i].Item2);
                }
            }
        }
    }
    #endregion
    #region Check
    public void CheckBuff(UnitType _unitType)
    {
        CalculateElemental(_unitType);
        CalculateProfession(_unitType);
    }
    #endregion
}
