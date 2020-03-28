using Ez.Threading;
using UnityEngine;

public class MoveOnUpdate : MonoBehaviour
{
    public float Speed;

    void Start()
    {
        UnityContext.Update
            .CreateTask()
            .RelativeToLast()
            .ProcessWith(deltaTime =>
            {
                var direction = new Vector3(
                    Input.GetAxis("Horizontal"),
                    Input.GetAxis("Vertical"));

                transform.position += direction * Speed * deltaTime;
            });
    }
}
