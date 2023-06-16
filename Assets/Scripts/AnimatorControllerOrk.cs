using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorControllerOrk : MonoBehaviour
{
    private Animator animator;
	public ObjectThrower objectThrower;

    // Start is called before the first frame update
    void Start()
    {
        // Get a reference to the Animator component
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Example: Trigger an animation
        if (Input.GetKeyDown(KeyCode.Space) && objectThrower.isReady == true)
        {
            // Set the "Throw" trigger parameter to play the throw animation
            animator.SetTrigger("Throw");
			objectThrower.ThrowObject();
			Debug.Log("This is a log message.");
        }

    }
}
