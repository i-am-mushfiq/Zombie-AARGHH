using UnityEngine;

public class ShieldFollow : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position;
        }
    }
}
