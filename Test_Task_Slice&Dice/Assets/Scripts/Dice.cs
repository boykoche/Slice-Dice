using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class Dice : MonoBehaviour
{
    #region Fields
    private Rigidbody _rb;
    private BoxCollider _bc;
    private GameObject _sideGameObject;

    private Sprite _sideSprite;

    private Vector3 _startPoint;
    private Vector3 _endPoint;

    [SerializeField] private CanvasController _canvasController;

    public GameObject[] allSides;


    private bool _touch = false;
    private string _achivement;
    private int _value = 0;
    #endregion

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _bc = GetComponent<BoxCollider>();

        if (gameObject.tag != "Enemy")
        {
            CanvasController.DiceMoveToCenter += MoveToCenter;
        }
        else
        {
            CanvasController.EnemyMoveToCenter += MoveToCenter;
            CanvasController.EnemyRoll += DiceRoll;
            CanvasController.EnemyMoveToImg += DiceTouch;
        }
        _startPoint = PlayerData.SetVectors(gameObject.tag)[0];
        _endPoint = PlayerData.SetVectors(gameObject.tag)[1];
    }

    private void MoveToCenter()
    {
        CanvasController.RollAction += DiceRoll;
        CanvasController.DiceEndPoint += DiceTouch;
        _rb.isKinematic = true;
        _bc.isTrigger = true;
        gameObject.SetActive(true);
        transform.DOMove(_startPoint, 1);
    }

    private void OnMouseDown()
    {
        if (gameObject.tag != "Enemy")
            DiceTouch();
    }

    private void DiceTouch()
    {
        if (_touch)
        {
            _rb.isKinematic = true;
            _bc.isTrigger = true;
            CanvasController.DiceEndPoint -= DiceTouch;
            CanvasController.RollAction -= DiceRoll;
            transform.DOMove(_endPoint, 1);
            StartCoroutine(Waiter(1));
            _canvasController.SetImage(gameObject.tag, _sideSprite);
            if (_achivement != null)
            {
                PlayerData.SetAchivementsValue(_achivement, _value);
            }
            _touch = false;
        }
    }

    IEnumerator Waiter(int seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public void DiceRoll()
    {
        {
            _rb.isKinematic = false;
            _bc.isTrigger = false;
            int x = UnityEngine.Random.Range(200, 500);
            int y = UnityEngine.Random.Range(200, 500);
            int z = UnityEngine.Random.Range(200, 500);
            _rb.AddForce(Vector3.up * 1000, ForceMode.Force);
            _rb.AddTorque(new Vector3(x, y, z));
            foreach (var item in allSides)
            {
                item.GetComponent<DiceSide>().StartFlip();
            }
        }
    }

    public void SetSide(int sideIndex)
    {
        _touch = true;
        _sideGameObject = allSides[sideIndex];
        _sideSprite = _sideGameObject.GetComponent<SpriteRenderer>().sprite;
        if (_sideSprite.name != "Nothing")
        {
            string[] parameters = _sideSprite.name.Split("_");
            _achivement = parameters[0];
            _value = Int32.Parse(parameters[1]);
        }
    }

    private void OnDisable()
    {
        CanvasController.DiceMoveToCenter -= MoveToCenter;
        CanvasController.RollAction -= DiceRoll;
        CanvasController.EnemyMoveToCenter -= MoveToCenter;
        CanvasController.EnemyRoll -= DiceRoll;
        CanvasController.EnemyMoveToImg -= DiceTouch;
    }

}

