using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public int PV;
    private Deplacement deplacement;
    private bool invisibility;
    public Collider Collider_player;

    private int nb_object;
    public TMP_Text TextObject;
    public string DebutTextObject;

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
        // si objet alors ramasser
        if (collision.gameObject.tag == "Object")
        {
            collision.gameObject.SetActive(false);
            nb_object++;
            TextObject.text = DebutTextObject + " " + nb_object;
        }
        // retirer pv si racine
        if ((collision.gameObject.tag == "Roots") && !invisibility)
        {
            LostPV(1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // si objet alors ramasser
        if (other.gameObject.tag == "Object")
        {
            other.gameObject.SetActive(false);
            nb_object++;
            TextObject.text = DebutTextObject + " " + nb_object;
        }
    }

    public void LostPV(int pvLost)
    {
        PV -= pvLost;
    }
}
