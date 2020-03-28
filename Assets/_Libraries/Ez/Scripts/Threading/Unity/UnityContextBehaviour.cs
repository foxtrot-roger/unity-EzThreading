using UnityEngine;

namespace Ez.Threading
{
    public class UnityContextBehaviour : MonoBehaviour
    {
        void Update()
        {
            UnityContext.Update.PublishMessage(Time.time);
        }

        void FixedUpdate()
        {
            UnityContext.FixedUpdate.PublishMessage(Time.time);
        }

        void OnApplicationQuit()
        {
            UnityContext.Update.PublishCompletion(false);
            UnityContext.FixedUpdate.PublishCompletion(false);
        }
    }
}