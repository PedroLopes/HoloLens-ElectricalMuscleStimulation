using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EscapeGate : GateScript
{
    public int currentValue;
    public int targetValue;

    public override void onPress(string args)
    {
        switch (args)
        {
            case "True":
                currentValue++;
                break;

            case "False":
                currentValue--;
                break;
        }

        if (opening != (currentValue == targetValue))
            GetComponent<AudioSource>().Play();

        opening = currentValue == targetValue;
    }
}
