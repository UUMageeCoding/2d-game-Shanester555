using UnityEngine;
using System.Collections;

public class OnCollection : MonoBehaviour
{
    public int collectableCount;
    public bool isCollected;
    public float animTimer = 2f;
    private Animator animator;

    [SerializeField] private CircleCollider2D collectableCollider;
    [SerializeField] private CapsuleCollider2D playerCollider;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        if (isCollected)
        {
            collectableCount++;
            Physics2D.IgnoreCollision(playerCollider, collectableCollider);

            animator.SetBool("Collected", true);
            Destroy(gameObject, 2);
            isCollected = false;
        }
    }
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(animTimer);
    }
}
