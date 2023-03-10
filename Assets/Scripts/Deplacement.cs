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

    public Vector2 movement;
    private bool canDash;
    private bool canUse;
    public bool IsDashing;

    private Rigidbody rb;
    public GameObject TailDash;

    public bool IsPaused;

    [SerializeField] private AudioClip[] _clipDash;

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
        if (IsPaused)
            return;
        transform.Translate(new Vector3(movement.x, 0, movement.y) * Time.deltaTime * Speed);

        if (canDash && canUse)
        {
            IsDashing = true;
            TailDash.SetActive(true);
            rb.AddForce(new Vector3(movement.x, 0, movement.y) * DashSpeed, ForceMode.Impulse);
            canUse = false;

            StartCoroutine(UseDash());
            StartCoroutine(CooldownDash());

            GetComponent<AudioSource>().PlayOneShot(_clipDash[Random.Range(0, _clipDash.Length)]);
        }
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        if (IsPaused)
            return;
        movement = context.ReadValue<Vector2>();
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (IsPaused)
            return;
        canDash = context.action.triggered;
    }

    public void Back()
    {
        if (IsPaused)
            return;
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
