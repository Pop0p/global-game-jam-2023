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
    public GameObject LifesObject;
    public ParticleSystem DieEffect;

    public MeshRenderer Mesh;

    public GameObject Tuto;
    private bool first;

    // Start is called before the first frame update
    void Start()
    {
        deplacement = GetComponent<Deplacement>();
        invisibilityDash = false;
        invisibilityTouch = false;
        first = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (first && Input.anyKey)
        {
            Tuto.SetActive(false);
            AudioManager.Instance.ChangeSound("begin");
        }

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
            TextObject.text = nb_object.ToString();
            AudioManager.Instance.ChangeSound("flower " + nb_object);
            if (nb_object == 6)
            {
                AudioManager.Instance.ChangeSound("victory");
                // Fin de la partie
            }
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
            TextObject.text = nb_object.ToString();

            AudioManager.Instance.ChangeSound("flower " + nb_object);
            if (nb_object == 6)
            {
                AudioManager.Instance.ChangeSound("victory");
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
