using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Deplacement : MonoBehaviour
{
    public int Speed;
    private Vector2 movement;
    private bool canDash;
    private bool canUse;

    // Start is called before the first frame update
    void Start()
    {
        movement = Vector2.zero;
        canUse = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(movement.x, 0, movement.y) * Time.deltaTime * Speed);

        if (canDash && canUse)
        {
            transform.Translate(new Vector3(movement.x, 0, movement.y) * Time.deltaTime * Speed * 3);
            canDash = false;
            canUse = false;
            StartCoroutine(CooldownDash());
            Debug.Log("dash");
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public void Dash(InputAction.CallbackContext context)
    {
        canDash = context.action.triggered;
    }

    private IEnumerator CooldownDash()
    {
        yield return new WaitForSeconds(3f);
        canUse = true;
    }
}
