using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChanger : MonoBehaviour
{
    public static LevelChanger instance;

    public static void Reset(bool complete = false)
    {
        if (instance != null)
            instance.changeLevel(complete ? 0 : instance.currentLevel);
    }

    public static void ChangeLevel(int index)
    {
        if (instance != null)
            instance.changeLevel(index);
    }

    private void Start()
    {
        instance = this;
    }

    public int currentLevel;

    public GameObject[] levels;

    public void changeLevel(int index)
    {
        index = Mathf.Clamp(index, 0, levels.Length - 1);

        currentLevel = index;

        for (int i = 0; i < levels.Length; i++)
        {
            GameObject level = levels[i];
            if (i == index)
            {
                level.SetActive(true);
                level.BroadcastMessage("StartGame");
            }
            else
            {
                level.SetActive(false);
            }
        }
    }
}