using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;




public class CanvasController : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject _heart;
    [SerializeField] private GameObject[] _heartLifes;
    [SerializeField] private GameObject _heartShield;
    [SerializeField] private Image _heartImg;

    [SerializeField] private GameObject _shield;
    [SerializeField] private GameObject[] _shieldLifes;
    [SerializeField] private Image _shieldImg;

    [SerializeField] private GameObject _sword;
    [SerializeField] private GameObject[] _swordLifes;
    [SerializeField] private GameObject _swordShield;
    [SerializeField] private Image _swordImg;

    [SerializeField] private GameObject[] _wolfLifes;
    [SerializeField] private Image _enemyImg;

    [SerializeField] private TextMeshProUGUI _rerollTxt;
    [SerializeField] private GameObject _rerollButton;
    [SerializeField] private GameObject _rollButton;
    [SerializeField] private GameObject _rollTxt;
    [SerializeField] private GameObject _endTurnButton;
    [SerializeField] private GameObject _pickUpButton;
    [SerializeField] private GameObject _startButton;

    [SerializeField] private List<GameObject> _directionLines;

    [SerializeField] private Sprite _nothingImg;
    [SerializeField] private GameObject _winRoundPanel;
    [SerializeField] private GameObject _loseRoundPanel;

    private Dictionary<string, Image> _allImage;
    private Dictionary<string, GameObject> _allCharacter;
    private Dictionary<string, GameObject[]> _allHearts;

    private int _rollCount = 3;
    private int _cubeCount = 0;

    public static Action RollAction;
    public static Action DiceMoveToCenter;
    public static Action DiceEndPoint;
    public static Action EnemyMoveToCenter;
    public static Action EnemyMoveToImg;
    public static Action EnemyRoll;

    private bool _IsSword = false;
    private bool _IsHeart = false;
    private bool _IsShield = false;
    private bool _rollAbility = false;
    #endregion

    private void Awake()
    {
        Initialisation();
    }

    private void Initialisation()
    {
        _allImage = new Dictionary<string, Image>()
        {
            {  "Heart", _heartImg},
            {  "Shield", _shieldImg},
            {  "Sword", _swordImg},
            {  "Enemy", _enemyImg}
        };
        _allCharacter = new Dictionary<string, GameObject>()
        {
            {"Heart", _heart },
            {"Shield", _shield},
            {"Sword", _sword },
        };
        _allHearts = new Dictionary<string, GameObject[]>()
        {
            {"Heart", _heartLifes },
            {"Shield", _shieldLifes},
            {"Sword", _swordLifes}
        };
        PlayerData.SetStartPosition("Heart", _heart.transform.position.x);
        PlayerData.SetStartPosition("Shield", _shield.transform.position.x);
        PlayerData.SetStartPosition("Sword", _sword.transform.position.x);
        PlayerData.OnStart();
    }

    public void StartRound()
    {
        Restart();
        _startButton.SetActive(false);
    }

    public void Restart()
    {
        PlayerData.AchivementsZeroing();
        if (_heart) { _heartShield.SetActive(false); }
        if (_sword) { _swordShield.SetActive(false); }

        StartCoroutine(EnemyTurn());
        foreach (var key in _allImage.Keys)
        {
            _allImage[key].sprite = _nothingImg;
        }

        _rollCount = 3;
        _rerollButton.SetActive(false);
        _cubeCount = 0;
        _rollTxt.SetActive(true);

    }

    public void PickUpAchivements()
    {
        DiceEndPoint?.Invoke();
        _pickUpButton.SetActive(false);
        _endTurnButton.SetActive(false);
        StartCoroutine(TurnButtonOn());
    }

    public void Roll()
    {
        if (_rollAbility && _rollCount > 0)
        {
            _rollCount--;
            if (_rollCount == 2)
            {
                _rollTxt.SetActive(false);
                _rerollButton.SetActive(true);
                _pickUpButton.SetActive(true);
            }
            RollAction?.Invoke();
            _rollAbility = false;

            _rerollTxt.text = $"{_rollCount.ToString()}/2";
            StartCoroutine(RollWaiter());

        }
    }

    private IEnumerator RollWaiter()
    {
        yield return new WaitForSeconds(2);
        _rollAbility = true;
        _rollButton.SetActive(true);
    }

    private IEnumerator TurnButtonOn()
    {
        yield return new WaitForSeconds(2);
        _endTurnButton.SetActive(true);
    }

    private IEnumerator EnemyTurn()
    {
        _rollButton.SetActive(false);
        EnemyMoveToCenter?.Invoke();
        yield return new WaitForSeconds(1);
        EnemyRoll?.Invoke();
        yield return new WaitForSeconds(4);
        EnemyMoveToImg?.Invoke();
        DiceMoveToCenter?.Invoke();
        StartCoroutine(RollWaiter());
        PlayerData.SetEnemyTarget();
        StartCoroutine(DirectionOn(PlayerData.enemyTarget));
    }

    private IEnumerator DirectionOn(int index)
    {
        _directionLines[index].SetActive(true);
        yield return new WaitForSeconds(1);
        _directionLines[index].SetActive(false);
    }

    public void SetImage(string key, Sprite sprite)
    {
        _cubeCount++;
        _allImage[key].sprite = sprite;
    }

    public void HeartTouch()
    {
        if (_IsShield)
        {
            PlayerData.SetShield("Heart");
            _IsShield = false;
            SetPosition(0, 0, 0);
            _heartShield.SetActive(true);
        }
        else if (!_IsHeart && PlayerData.achivementsValue["Heart"] > 0)
        {
            SetIsTouch(true, false, false);
            SetPosition(100, 0, 0);
        }
        else
        {
            _IsHeart = false;
            _heart.transform.DOMoveX(PlayerData.UIposition["Heart"], 1);
        }
    }

    public void ShieldTouch()
    {
        if (_IsHeart)
        {
            PlayerData.PlussingLife("Shield");
            _IsHeart = false;
            SetPosition(0, 0, 0);

            for (int i = 0; i < PlayerData.lifes["Shield"]; i++)
            {
                _shieldLifes[2 - i].SetActive(true);
            }
        }
        else if (!_IsShield && PlayerData.achivementsValue["Shield"] > 0)
        {
            SetIsTouch(false, false, true);
            SetPosition(0, 0, 100);
        }
        else
        {
            _IsShield = false;
            _shield.transform.DOMoveX(PlayerData.UIposition["Shield"], 0.5f);
        }
    }

    public void SwordTouch()
    {
        if (_IsHeart)
        {
            PlayerData.PlussingLife("Sword");
            _IsHeart = false;
            SetPosition(0, 0, 0);

            for (int i = 0; i < PlayerData.lifes["Sword"]; i++)
            {
                _swordLifes[2 - i].SetActive(true);
            }
        }
        else if (_IsShield)
        {
            PlayerData.SetShield("Sword");
            _IsShield = false;
            SetPosition(0, 0, 0);
            _swordShield.SetActive(true);
        }
        else if (!_IsSword && PlayerData.achivementsValue["Sword"] > 0)
        {
            SetIsTouch(false, true, false);
            SetPosition(0, 100, 0);
        }
        else
        {
            _IsSword = false;
            _heart.transform.DOMoveX(PlayerData.UIposition["Sword"], 1);
        }
    }

    public void EnemyTouch()
    {
        if (_IsSword)
        {
            PlayerData.ChangeLifeCount("Enemy", PlayerData.achivementsValue["Sword"]);
            if (PlayerData.lifes["Enemy"] <= 0)
            {
                EndRound(true);
            }
            else
            {
                for (int i = 0; i < 5 - PlayerData.lifes["Enemy"]; i++)
                {
                    _wolfLifes[i].SetActive(false);
                }
                PlayerData.SwordZeroing();
                _IsSword = false;
                SetPosition(0, 0, 0);
            }
        }
        else
        {
            StartCoroutine(DirectionOn(PlayerData.enemyTarget));
        }
    }

    private void SetIsTouch(bool heart, bool sword, bool shield)
    {
        _IsHeart = heart;
        _IsSword = sword;
        _IsShield = shield;
    }

    private void SetPosition(int heart, int sword, int shield)
    {
        if (_heart) { _heart.transform.DOMoveX(PlayerData.UIposition["Heart"] + heart, 0.5f); }
        if (_sword) { _sword.transform.DOMoveX(PlayerData.UIposition["Sword"] + sword, 0.5f); }
        if (_shield) { _shield.transform.DOMoveX(PlayerData.UIposition["Shield"] + shield, 0.5f); }

    }

    public void EndTurn()
    {
        _endTurnButton.SetActive(false);
        PlayerData.MinusingShield();
        PlayerData.MinusingLife();
        string key = PlayerData.targets[PlayerData.enemyTarget];
        int lifeCount = PlayerData.lifes[key];

        if (lifeCount > 0)
        {
            for (int i = 0; i < 3 - lifeCount; i++)
            {
                _allHearts[key][i].SetActive(false);
            }
        }
        if (lifeCount == 0)
        {
            Destroy(_allCharacter[key]);
            Destroy(GameObject.FindGameObjectWithTag(key));
            _allCharacter.Remove(key);
            _allImage.Remove(key);
            _directionLines.Remove(_directionLines[PlayerData.enemyTarget]);
            PlayerData.RemoveTarget();
        }
        if (PlayerData.targets.Count == 0) { EndRound(false); }
        Restart();
    }

    private void EndRound(bool win)
    {
        if (win) { _winRoundPanel.SetActive(true); }
        else { _loseRoundPanel.SetActive(true); }
    }

    public void NewLevel()
    {
        if (PlayerPrefs.GetInt("level") == 0)
        {
            SceneManager.LoadScene(1);
            PlayerPrefs.SetInt("level", 1);
        }
        else
        {
            SceneManager.LoadScene(0);
            PlayerPrefs.SetInt("level", 0);
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("level"));
    }
}
