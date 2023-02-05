using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public int PV;
    private Deplacement deplacement;
    private bool invisibilityDash;
    private bool invisibilityTouch;
    public float InvinsibiliteTouchCooldown;
    public Collider Collider_player;

    private int nb_object;
    public TMP_Text TextObject;
    public string DebutTextObject;
    public GameObject LifesObject;
    public ParticleSystem DieEffect;

    public MeshRenderer Mesh;

    // Start is called before the first frame update
    void Start()
    {
        deplacement = GetComponent<Deplacement>();
        invisibilityDash = false;
        invisibilityTouch = false;
    }

    // Update is called once per frame
    void Update()
    {
        invisibilityDash = deplacement.IsDashing;
        if (invisibilityDash || invisibilityTouch)
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
        if ((collision.gameObject.tag == "Roots") && !invisibilityDash && !invisibilityTouch)
        {
            invisibilityTouch = true;
            deplacement.Back();

            StartCoroutine(Clignote());
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
            if (nb_object == 6)
            {
                // Fin de la partie
            }
        }
    }

    public void LostPV(int pvLost)
    {
        PV -= pvLost;
        Debug.Log(PV);
        LifesObject.transform.GetChild(PV).GetChild(0).gameObject.SetActive(false);
        if (PV <= 0)
        {
            DieEffect.transform.position = transform.position;
            DieEffect.gameObject.SetActive(true);
            
            gameObject.SetActive(false);
        }
    }

    private IEnumerator Clignote()
    {
        Mesh.enabled = false;
        yield return new WaitForSeconds(0.1f);
        Mesh.enabled = true;
        yield return new WaitForSeconds(0.1f);
        Mesh.enabled = false;
        yield return new WaitForSeconds(0.1f);
        Mesh.enabled = true;
        yield return new WaitForSeconds(0.1f);
        Mesh.enabled = false;
        yield return new WaitForSeconds(0.1f);
        Mesh.enabled = true;
        yield return new WaitForSeconds(0.1f);
        Mesh.enabled = false;
        yield return new WaitForSeconds(0.1f);
        Mesh.enabled = true;

        invisibilityTouch = false;
    }
}
