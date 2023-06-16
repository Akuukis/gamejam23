using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimatorControllerOrk : MonoBehaviour
{
    private Animator animator;
	public ObjectThrower objectThrower;
    private bool threw = false;

    public void OnThrow(InputAction.CallbackContext context)
    {
        threw = context.action.triggered;
    }

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
        if(threw || transform.parent.parent.parent.GetComponent<PlayerInput>().currentControlScheme == "Keyboard" && Input.GetMouseButtonDown(0))
        {
            Debug.Log("PEW");
            if (objectThrower.isReady == true)
            {
                // Set the "Throw" trigger parameter to play the throw animation
                animator.SetTrigger("Throw");
                objectThrower.ThrowObject();
                Debug.Log("This is a log message.");
            }
        }
    }
}
