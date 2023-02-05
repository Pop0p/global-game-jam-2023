using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

    private Rigidbody _rb;
    public MeshRenderer Mesh;

    public GameObject Tuto;

    public bool IsGoingToVictory;
    public bool GOTOVICTORY;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        deplacement = GetComponent<Deplacement>();
        invisibilityDash = false;
        invisibilityTouch = false;
    }

    // Update is called once per frame
    void Update()
    {
        if ((deplacement.IsDashing || deplacement.movement != Vector2.zero) && Tuto.activeInHierarchy)
        {
            Tuto.SetActive(false);
            AudioManager.Instance.ChangeSound("begin");
            RootsManager.Instance.IsPlaying = true;
            FlowerManager.Instance.IsPlaying = true;
        }

        invisibilityDash = deplacement.IsDashing;
        if (invisibilityDash || invisibilityTouch)
        {
            if (!RootsManager.Instance.isCollisionDisabled)
                RootsManager.Instance.DisableCollisions();
            Collider_player.isTrigger = true;
        }
        else
        {
            if (RootsManager.Instance.isCollisionDisabled)
                RootsManager.Instance.EnableCollisions();
            Collider_player.isTrigger = false;
        }

        if (GOTOVICTORY && !IsGoingToVictory)
        {
            IsGoingToVictory = true;
            SceneManager.LoadScene("MenuFin");
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
            if (nb_object == 1)
            {
                Debug.Log("IcI EN FAIT !!!!!!!!!!!!!");
                AudioManager.Instance.ChangeSound("victory");
                StartCoroutine(SAMEREEEEEEEEEEEEEEEEE());
                RootsManager.Instance.IsPlaying= false;
                FlowerManager.Instance.IsPlaying= false;
            }
        }
    }

    public void LostPV(int pvLost)
    {
        PV -= pvLost;
        LifesObject.transform.GetChild(PV).GetChild(0).gameObject.SetActive(false);
        if (PV <= 0)
        {
            DieEffect.transform.position = transform.position;
            DieEffect.gameObject.SetActive(true);
            gameObject.SetActive(false);
            AudioManager.Instance.ChangeSound("death");
        }
    }

    private IEnumerator Clignote()
    {
        for (int i = 0; i < 7; i++)
        {
            Mesh.enabled = false;
            yield return new WaitForSeconds(0.1f);
            Mesh.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.25f);
        invisibilityTouch = false;
    }

    IEnumerator SAMEREEEEEEEEEEEEEEEEE()
    {
        yield return new WaitForSeconds(1.5f);
        GOTOVICTORY = true;
    }
}
