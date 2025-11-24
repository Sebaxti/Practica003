using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SCR_Player : MonoBehaviour
{
    [Header("VIDA")]
    public float vidamaxima;
    public float vida;

    [Header("UI")]
    public Slider barraDeVida;
    public GameObject boton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vida = vidamaxima;
        boton.SetActive(false);

        
    }

    // Update is called once per frame
    void Update()
    {
        ActualizarBarraDeVida();

       
    }

    public void RecibirDanyo (float danyo) 
    {
        vida -= danyo;

        if(vida <= 0)
        {
            vida = 0;
            barraDeVida.value=0;
            Morir();
        }
    }

    public void ActualizarBarraDeVida()
    {
        if (barraDeVida != null)
        {
            barraDeVida.value = vida / vidamaxima;
        } 
    }

    public void Morir()
    {
        gameObject.SetActive(false);
        boton.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Restart()
    {
        vida = vidamaxima;

        boton.SetActive(false);

        gameObject.SetActive(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        transform.position = new Vector3(-2,1, 12);


    }
}
