using UnityEngine;
using System.Collections;

public class AnimationDelay : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float timer;
    [SerializeField] float speed;
    void Start()
    {
        StartCoroutine(DelayAnimation());
    }

    
    private IEnumerator DelayAnimation()
    {
        yield return new WaitForSeconds(timer);
        animator.SetTrigger("Start");
        animator.speed = speed;
    }
}
