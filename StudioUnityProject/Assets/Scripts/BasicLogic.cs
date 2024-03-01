using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicLogic : MonoBehaviour
{
    bool AndCondition(bool conditionA, bool conditionB)
    {
        return (conditionA && conditionB);
    }

    bool OrCondition(bool conditionA, bool conditionB)
    {
        return (conditionA || conditionB);
    }

    bool XorCondition(bool conditionA, bool conditionB)
    {
        return (conditionA ^ conditionB);
    }

    bool NotCondition(bool conditionA)
    {
        return (!conditionA);
    }
}
