using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("À¯´Ö Á¤º¸")]
    [SerializeField] private UnitState unitState;
    [SerializeField] private UnitType unitType;
    [SerializeField] private Elemental elemental;
    [SerializeField] private Profession profession;

    [Header("½ºÅ³")]
    [SerializeField] private Skill skill;

    [Header("À¯´Ö ´É·ÂÄ¡")]
    [SerializeField] private int level;
    [SerializeField] private int hp;
    [SerializeField] private int attackPoint;

    [Header("À¯´Ö Á¤º¸")]
    [SerializeField] private Vector2 position;
    [SerializeField] private Unit currentTarget;

    public Unit ChangeTarget(List<Unit> _targetList)
    {
        Unit target = null;
        float minDistance = float.MaxValue;

        for(int i = 0; i < _targetList.Count; i++)
        {
            if(!_targetList[i].unitState.Equals(UnitState.Death))
            {
                float currentDistance = (this.transform.localPosition - _targetList[i].transform.localPosition).sqrMagnitude;
                //Debug.Log(currentDistance.ToString());
                if (minDistance >= currentDistance)
                {
                    minDistance = currentDistance;
                    target = _targetList[i];
                }
            }
        }
        currentTarget = target;
        return target;
    }
}