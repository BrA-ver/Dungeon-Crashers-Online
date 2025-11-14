using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] Animator animator;
    bool isOpen = false;

    private void Start()
    {
        InputHandler.instance.onTutotialPressed += Toggle;
    }

    public void Toggle()
    {
        isOpen = !isOpen;
        animator.SetBool("Open", isOpen);
    }
}
