using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : ButtonReceiver
{
    public override void onPress(string args)
    {
        SceneManager.LoadScene(0);
    }
}