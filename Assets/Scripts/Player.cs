using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int PV;
    private Deplacement deplacement;
    private bool invisibility;

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
