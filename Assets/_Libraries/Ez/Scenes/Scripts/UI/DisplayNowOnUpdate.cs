using UnityEngine;
using UnityEngine.UI;

public class DisplayNowOnUpdate : MonoBehaviour
{

    public Text NowDiplay;
    public Text DeltaDiplay;

    float lastUpdate = -10f;
    public float interval = 1;

    void Update()
    {
        var now = Time.time;

        if (lastUpdate + interval < now)
        {
            var deltaTime = now - lastUpdate;
            lastUpdate = now;

            NowDiplay.text = "Update [Normal] now : " + now;
            DeltaDiplay.text = "Update [Normal] deltaTime :" + deltaTime;
        }
    }
}
