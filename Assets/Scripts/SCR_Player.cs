using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SCR_Player : MonoBehaviour
{
    [Header("VIDA")]
    public float vidamaxima;
    public float vida;

    public bool canvasOn;

    [Header("UI")]
    public Slider barraDeVida;
    public GameObject boton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasOn = false;
        vida = vidamaxima;
        boton.SetActive(true);

        
    }

    // Update is called once per frame
    void Update()
    {
        ActualizarBarraDeVida();
        
        Debug.Log(canvasOn);

       
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
        canvasOn=true;
       gameObject.SetActive(false);   
    }

    public void Restart()
    {
        gameObject.SetActive(true);
        
    }
}
