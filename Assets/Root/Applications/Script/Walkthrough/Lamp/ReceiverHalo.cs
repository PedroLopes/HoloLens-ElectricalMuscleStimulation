using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiverHalo : ButtonReceiver {
    public override void onPress(string args)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
