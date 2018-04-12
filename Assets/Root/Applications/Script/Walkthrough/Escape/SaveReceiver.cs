using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveReceiver : ButtonReceiver {
    public SaveHandler saveHandler;

    public override void onPress(string args)
    {
        saveHandler.Save();
    }
}
