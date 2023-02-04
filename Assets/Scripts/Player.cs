using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int PV;
    private Deplacement deplacement;
    private bool invisibility;
    public Collider Collider_player;

    // Start is called before the first frame update
    void Start()
    {
        deplacement = GetComponent<Deplacement>();
        invisibility = false;
    }

    // Update is called once per frame
    void Update()
    {
        invisibility = deplacement.IsDashing;
        if (invisibility)
        {
            Collider_player.isTrigger = true;
        }
        else
        {
            Collider_player.isTrigger = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // retirer pv si racine
        if (!invisibility)
        {
            LostPV(1);
        }
    }

    public void LostPV(int pvLost)
    {
        PV -= pvLost;
    }
}
