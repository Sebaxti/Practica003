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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vida = vidamaxima;
        ActualizarBarraDeVida();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RecibirDanyo (float danyo) 
    {
        vida -= danyo;

        if(vida <= 0)
        {
            vida = 0;
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
        Destroy(gameObject);
    }
}
