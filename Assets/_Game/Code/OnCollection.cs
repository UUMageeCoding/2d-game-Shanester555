using UnityEngine;
using System.Collections;

public class OnCollection : MonoBehaviour
{
    public int collectableCount;
    public bool isCollected;
    public float animTimer = 2f;
    private Animator animator;

    [SerializeField] private BoxCollider2D collectableCollider;
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
            StartCoroutine(CollectAnimTrigger)
            isCollected = false;
        }
    }

    private void IEnumerator CollectAnimTrigger()
    {
        Physics2D.IgnoreCollision(playerCollider, collectableCollider);

        animator.SetTrigger("Collected");
        yield return new WaitForSeconds(animTimer);
        Destroy(gameObject);
    }


}
