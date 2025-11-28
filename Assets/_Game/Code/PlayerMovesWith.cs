using UnityEngine;
using System.Collections;

public class PlayerMovesWith : MonoBehaviour
{
    private Rigidbody2D rb;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SetParent(transform);
        rb = collision.gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
        rb.transform.eulerAngles = new Vector3(rb.transform.eulerAngles.x, rb.transform.eulerAngles.y, 0);
    }
}