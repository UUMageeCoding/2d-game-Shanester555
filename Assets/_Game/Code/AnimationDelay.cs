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

    // gets time to make the animation wait to start, then sets it to a set speed
    private IEnumerator DelayAnimation()
    {
        yield return new WaitForSeconds(timer);
        animator.SetTrigger("Start");
        animator.speed = speed;
    }
}
