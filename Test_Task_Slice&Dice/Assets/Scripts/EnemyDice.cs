using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyDice : Dice
{
    private Vector3 _endPoint = new Vector3(6, 0.5f, 1);
    private void Roll()
    {
        transform.DOMove(_endPoint, 1);
    }
}
