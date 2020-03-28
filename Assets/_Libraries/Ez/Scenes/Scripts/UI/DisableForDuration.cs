using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableForDuration : MonoBehaviour
{

    public Text Text;
    public Button Button;
    public float DisableDuration = 1;


    string originalText;
    bool isDisabled;
    float enableTime;

    // Update is called once per frame
    void Update()
    {
        if (isDisabled)
        {
            var now = Time.time;
            if (enableTime < now)
            {
                Text.text = originalText;
                Button.interactable = true;
                isDisabled = false;
            }
            else
            {
                var remaining = enableTime - now;
                Text.text = "Remaining " + remaining.ToString("0.00") + "s";
            }
        }
    }

    public void OnClick()
    {
        isDisabled = true;

        enableTime = Time.time + DisableDuration;
        Button.interactable = false;
        originalText = Text.text;
    }
}
