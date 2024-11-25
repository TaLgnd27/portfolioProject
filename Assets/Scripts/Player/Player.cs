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

    public static Player Instance;

    public override void Awake()
    {
        if (Player.Instance != null && Player.Instance != this)
        {
            Player.Instance.transform.position = this.transform.position;
            Destroy(gameObject);
            return;
        } else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        base.Awake();
    }

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
        yield return new WaitForSeconds(0.01f);
        while (isFiring)
        {
            base.Shoot();
            delay = gun.gun.rof;
            yield return new WaitForSeconds(delay);
        }
    }

    public override void Damage(int hp)
    {
        base.Damage(hp);

        if (!isInvuln)
        {
            Debug.Log(isInvuln);
            isInvuln = true;
            StartCoroutine("Invuln");
        }
    }

    private IEnumerator Invuln()
    {
        Debug.Log("Invuln");
        gameObject.layer = base.dodgeLayer;
        StartCoroutine("InvulnFlash");

        yield return new WaitForSeconds(iTime.GetModifiedValue());

        StopCoroutine("InvulnFlash");
        Color color = base.spriteRenderer.color;
        color.a = 1;
        base.spriteRenderer.color = color;
        isInvuln = false;
        gameObject.layer = base.startLayer;
    }

    private IEnumerator InvulnFlash()
    {
        Debug.Log("Flash");
        Color color = base.spriteRenderer.color;
        color.a = 0;
        base.spriteRenderer.color = color;
        yield return new WaitForSeconds(0.15f);

        color.a = 1;
        base.spriteRenderer.color = color;
        yield return new WaitForSeconds(0.15f);
        StartCoroutine("InvulnFlash");
    }
}
