using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Deplacement : MonoBehaviour
{
    public float Speed;
    public float DashSpeed;
    public float DashCooldown;
    public float DashDuration;

    private Vector2 movement;
    private bool canDash;
    private bool canUse;
    public bool IsDashing;

    private Rigidbody rb;
    public GameObject TailDash;

    // Start is called before the first frame update
    void Start()
    {
        movement = Vector2.zero;
        canUse = true;
        rb = GetComponent<Rigidbody>();
        IsDashing = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(movement.x, 0, movement.y) * Time.deltaTime * Speed);

        if (canDash && canUse)
        {
            TailDash.SetActive(true);
            rb.AddForce(new Vector3(movement.x, 0, movement.y) * DashSpeed, ForceMode.Impulse);
            canUse = false;

            StartCoroutine(UseDash());
            StartCoroutine(CooldownDash());
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public void Dash(InputAction.CallbackContext context)
    {
        canDash = context.action.triggered;
        IsDashing = true;
    }

    public void Back()
    {
        if (movement == Vector2.zero)
        {
            rb.AddRelativeForce(new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)) * 3, ForceMode.Impulse);
        }
        else
        {
            rb.AddRelativeForce(new Vector3(-movement.x, 0, -movement.y) * 3, ForceMode.Impulse);
        }
        StartCoroutine(StopBack());
    }

    private IEnumerator StopBack()
    {
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector3.zero;
    }

    private IEnumerator CooldownDash()
    {
        yield return new WaitForSeconds(DashCooldown);
        canUse = true;
    }

    private IEnumerator UseDash()
    {
        yield return new WaitForSeconds(DashDuration);
        IsDashing = false;
        rb.velocity = Vector3.zero;
        TailDash.SetActive(false);
    }
}
