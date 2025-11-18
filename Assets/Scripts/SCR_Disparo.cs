using UnityEngine;

public class SCR_Disparo : MonoBehaviour
{
    public float danyo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Ha golpeado");
            var vidaPlayer=other.gameObject.GetComponent<SCR_Player>().vida-=danyo;

            if (vidaPlayer < 0)
            {
                Destroy(other.gameObject);
            }

            Destroy(gameObject);
        }
    }
}
