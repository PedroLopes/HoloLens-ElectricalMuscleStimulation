using UnityEngine;

public abstract class ButtonReceiver : MonoBehaviour
{
    public abstract void onPress(string args);
}