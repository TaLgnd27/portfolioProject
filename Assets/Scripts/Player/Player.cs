using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Creature
{
    [SerializeField]
    float smoothSpeed;

    private Vector2 moveInput;
    private Vector2 moveBuffer;
    private Vector2 target;
    private bool isFiring = false;

    public bool isTransition = false;
    public delegate void onTransitionDoneEvent();
    public event onTransitionDoneEvent onTransitionDone;

    public Vector2 moveTarget;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isDashing)
        {
            moveInput = moveBuffer;
        }

        if (!isTransition)
        {
            base.Move(moveInput);
        }
        else
        {
            rb.velocity = Vector2.zero;
            Collider2D col = GetComponent<Collider2D>();
            col.enabled = false;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, moveTarget, smoothSpeed);
            rb.MovePosition(smoothedPosition);
            if (Vector3.Distance(transform.position, moveTarget) <= 0.05)
            {
                col.enabled = true;
                isTransition = false;
                if (onTransitionDone != null)
                    onTransitionDone();
            }
        }
        Vector3 lookPos = (Vector2)Camera.main.ScreenToWorldPoint(target) - (Vector2)transform.position;
        float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
        transform.rotation = rotation;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveBuffer = context.ReadValue<Vector2>();
    }

    public void Look(InputAction.CallbackContext context)
    {
        target = context.ReadValue<Vector2>();
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isFiring = true;
            StartCoroutine("Shooting", gun.gun.rof);
        }
        else if (context.canceled)
        {
            isFiring = false;
            StopCoroutine("Shooting");
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            base.Dash(moveInput);
        }
    }

    private IEnumerator Shooting(float delay)
    {
        while (isFiring)
        {
            base.Shoot();
            delay = gun.gun.rof;
            yield return new WaitForSeconds(delay);
        }
    }
}
