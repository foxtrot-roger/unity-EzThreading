using Ez.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThreadingDisableForDuration : MonoBehaviour
{

    public Text Text;
    public Button Button;
    public float DisableDuration = 1;

    public void OnClick()
    {
        Button.interactable = false;
        var text = Text.text;

        UnityContext.Update
            .CreateTask()
            .EndingIn(DisableDuration)
            .RelativeToStart()
            .Select(deltaTime => DisableDuration - deltaTime)
            .ProcessWith(remaining =>
            {
                Text.text = "Remaining " + remaining.ToString("0.00") + "s";
            })
            .ContinueWith(completed =>
            {
                Text.text = text;
                Button.interactable = true;
            });
    }
}
