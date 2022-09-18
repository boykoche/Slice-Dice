using UnityEngine;

public class DiceSide : MonoBehaviour
{
    [SerializeField] private Dice _dice;
    private Rigidbody _rb;
    private int _sideIndex = 0;
    private bool _isValueChoosed = true;

    private void Awake()
    {
        _rb = _dice.GetComponent<Rigidbody>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ground" && _rb.velocity.x == 0 && _rb.velocity.y == 0 && _rb.velocity.z == 0 && !_isValueChoosed)
        {
            _isValueChoosed = true;
            SetSide();
            _dice.SetSide(_sideIndex);
        }
    }

    public void StartFlip()
    {
        _isValueChoosed = false;
    }
    private void SetSide()
    {
        switch (gameObject.name)
        {
            case "Side1":
                _sideIndex = 1;
                break;
            case "Side2":
                _sideIndex = 0;
                break;
            case "Side3":
                _sideIndex = 3;
                break;
            case "Side4":
                _sideIndex = 2;
                break;
            case "Side5":
                _sideIndex = 5;
                break;
            case "Side6":
                _sideIndex = 4;
                break;
        }
    }
}

