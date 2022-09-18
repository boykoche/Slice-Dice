using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    #region Fields
    public static Dictionary<string, int> achivementsValue;
    public static Dictionary<string, int> lifes;
    public static Dictionary<string, int> shield;
    public static Dictionary<string, float> UIposition = new Dictionary<string, float>()
    {

    };

    public static List<string> targets;

    public static int enemyTarget;
    #endregion

    public static void OnStart()
    {
        lifes = new Dictionary<string, int>()
    {
        {"Heart",3},
        {"Shield",3},
        {"Sword",3},
        {"Enemy", 5 }
    };
        shield = new Dictionary<string, int>()
    {
        {"Heart",0},
        {"Shield",0},
        {"Sword",0},
    };
        achivementsValue = new Dictionary<string, int>()
    {
        {"Heart",0},
        {"Shield",0},
        {"Sword",0},
        {"Enemy", 0}
    };
        targets = new List<string> { "Heart", "Shield", "Sword" };
    }

    public static void SetAchivementsValue(string key, int value)
    {
        achivementsValue[key] = value;
    }

    public static void ChangeLifeCount(string key, int count)
    {
        lifes[key] = lifes[key] - count;
    }

    public static List<Vector3> SetVectors(string tag)
    {
        List<Vector3> vectors = new List<Vector3>();
        switch (tag)
        {
            case "Heart":
                vectors.Add(new Vector3(-1.5f, 0.5f, -0.8f));
                vectors.Add(new Vector3(-5, 0.5f, 3));
                break;
            case "Shield":
                vectors.Add(new Vector3(0, 0.5f, 0.75f));
                vectors.Add(new Vector3(-5, 0.5f, 1));
                break;
            case "Sword":
                vectors.Add(new Vector3(1.5f, 0.5f, -0.8f));
                vectors.Add(new Vector3(-5, 0.5f, -1));
                break;
            case "Enemy":
                vectors.Add(new Vector3(0, 0.5f, 0f));
                vectors.Add(new Vector3(6, 0.5f, 1));
                break;
        }
        return vectors;
    }

    public static void SetStartPosition(string key, float value)
    {
        UIposition[key] = value;
    }

    public static void PlussingLife(string key)
    {
        lifes[key] += achivementsValue["Heart"];
        if (lifes[key] > 3) { lifes[key] = 3; }
        Debug.Log("life " + lifes[key]);
        achivementsValue["Heart"] = 0;
    }
    public static void SetEnemyTarget()
    {
        enemyTarget = Random.Range(0, targets.Count);
    }

    public static void SetShield(string key)
    {
        shield[key] += achivementsValue["Shield"];
        achivementsValue["Shield"] = 0;
    }

    public static void MinusingLife()
    {
        if (achivementsValue["Enemy"] >= lifes[targets[enemyTarget]])
        {
            lifes[targets[enemyTarget]] = 0;
        }
        else
        {
            lifes[targets[enemyTarget]] -= achivementsValue["Enemy"];
        }
        achivementsValue["Enemy"] = 0;
    }

    public static void MinusingShield()
    {
        Debug.Log(achivementsValue["Enemy"]);

        if (achivementsValue["Enemy"] >= shield[targets[enemyTarget]])
        {
            achivementsValue["Enemy"] -= shield[targets[enemyTarget]];
            shield[targets[enemyTarget]] = 0;
        }
        else
        {
            shield[targets[enemyTarget]] -= achivementsValue["Enemy"];
            achivementsValue["Enemy"] = 0;
        }
    }

    public static void AchivementsZeroing()
    {
        achivementsValue["Heart"] = 0;
        achivementsValue["Shield"] = 0;
        achivementsValue["Sword"] = 0;
        achivementsValue["Enemy"] = 0;
    }

    public static void SwordZeroing()
    {
        achivementsValue["Sword"] = 0;
    }

    public static void RemoveTarget()
    {
        targets.Remove(targets[enemyTarget]);
    }

}

