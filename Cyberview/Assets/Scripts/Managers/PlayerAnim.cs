using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : AbstractBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
    }

    

    void Update()
    {
        var right = inputState.GetButtonValue(inputButtons[0]);
        var left = inputState.GetButtonValue(inputButtons[1]);
        var standing = GetComponent<PlayerManager>().isGrounded;

        if ((left || right) && standing) {
            animator.SetBool("walking", true);
        } else
        {
            animator.SetBool("walking", false);
        }
    }
}
