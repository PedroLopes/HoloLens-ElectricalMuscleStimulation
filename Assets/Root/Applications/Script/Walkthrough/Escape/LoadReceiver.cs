using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadReceiver : ButtonReceiver {
    public SaveHandler saveHandler;

    public override void onPress(string args)
    {
        saveHandler.Load();
    }
}
