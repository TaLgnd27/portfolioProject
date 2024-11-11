using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseTest : MonoBehaviour
{

    Vector2 moveInput;
    Vector2 mouse;
    [SerializeField]
    InputActionAsset action;

    // Start is called before the first frame update
    private void OnEnable()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mouse = Mouse.current.position.ReadValue();
        
        Debug.Log(Camera.main.ScreenToWorldPoint(mouse));
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnMousePosition(Vector2 pos)
    {
        mouse = pos;
        //mouse = context.ReadValue<Vector2>();
        Debug.Log("Mouse Position: " + mouse);
        // Use mousePosition as needed
    }
}
