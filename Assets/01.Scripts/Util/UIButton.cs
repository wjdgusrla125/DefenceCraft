using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// public enum MainMenuButton
// {
//     START,
//     OPTION,
//     QUIT
// }

[RequireComponent(typeof(Animator))]
public class UIButton : MonoBehaviour
{
    public Animator animator;
    //public MainMenuButton mainMenuButton;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void MousePointerEnter()
    {
        animator.SetBool("selected", true);
    }

    public virtual void MousePointerExit()
    {
        animator.SetBool("selected", false);
    }

    public virtual void MousePointerClick()
    {
        animator.SetTrigger("pressed");
        Invoke("PressedButton", 2f);
    }

    // private void PressedButton()
    // {
    //     switch (mainMenuButton)
    //     {
    //         case MainMenuButton.START:
    //             SceneManager.LoadScene("LoadingScene");
    //             break;
    //         case MainMenuButton.QUIT:
    //             Application.Quit();
    //             break;
    //     }
    // }
}