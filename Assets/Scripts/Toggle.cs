using UnityEngine;
using System.Collections;

public class Toggle : MonoBehaviour
{
    public PlatformEffector2D effector;
    public CircleCollider2D leftCollider;
    public CircleCollider2D rightCollider;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.transform.position.y > 2.0f)
                effector.surfaceArc = 20.0f;
            else
                effector.surfaceArc = 0.0f;
        }
        leftCollider.enabled = true;
        rightCollider.enabled = true;
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
              effector.surfaceArc = 20.0f;
        }
        leftCollider.enabled = false;
        rightCollider.enabled = false;
    }
}
