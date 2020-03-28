using Ez.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ThreadingDisplayNowOnUpdate : MonoBehaviour
{

    public Text NowDiplay;
    public Text DeltaDiplay;

    void Start()
    {
        UnityContext.Update
            .CreateTask()
            .AtInterval(1)
            .ProcessWith(now =>
            {
                NowDiplay.text = "Update [Thread] now : " + now;
            })
            .RelativeToLast()
            .ProcessWith(deltaTime =>
            {
                DeltaDiplay.text = "Update [Thread] deltaTime :" + deltaTime;
            });
    }
}
