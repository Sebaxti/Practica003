using UnityEngine;
using UnityEngine.AI;

public class SCR_Guardia : MonoBehaviour
{
    private bool estoyDisparando = false;
    private string animacionActual = "";
    private Vector3 direccion;
    private NavMeshAgent agente;
    private Animator animador;
    public enum Estados { Patrol, Chase, Attack };

    [Header("ESTADOS")]
    public Estados estadoActual;
    public GameObject[] puntosDePatrulla;
    private int puntoActual = 0;
    public GameObject jugador, bala;

    [Header("PATRULLAR")]
    public float distanciaDejarDePerseguir;
    public float velocidadPatrullar;
    [Header("PERSEGUIR")]
    public float velocidadPerseguir; 
    [Header("ATACAR")]
    public float distanciaParaAtacar;
    public float disparoVelocidad;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animador = GetComponent<Animator>();
        agente = GetComponent<NavMeshAgent>();

        estadoActual = Estados.Patrol;


    }

    // Update is called once per frame
    void Update()
    {
        // Dibujamos el rayo para debug
        Vector3 direccionAdelante = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, direccionAdelante, Color.red);

        // Ejecutamos el comportamiento seg�n el estado
        switch (estadoActual)
        {
            case Estados.Patrol:
                Patrullar();
                break;

            case Estados.Chase:
                Perseguir();
                break;

            case Estados.Attack:
                Atacar();
                break;
        }

        // Verificamos la distancia para atacar si estamos persiguiendo
        if (estadoActual == Estados.Chase && jugador != null)
        {
            float distancia = Vector3.Distance(transform.position, jugador.transform.position);
            if (distancia <= distanciaParaAtacar)
            {
                estadoActual = Estados.Attack;
            }
        }
    }

    // Cuando algo entra en el trigger
    void OnTriggerEnter(Collider other)
    {
        // Si es el jugador y estamos patrullando
        if (other.CompareTag("Player") && estadoActual == Estados.Patrol)
        {
            jugador = other.gameObject;
            estadoActual = Estados.Chase;
            Debug.Log("persiguiendo");
        }
    }

    // Cuando algo sale del trigger
    void OnTriggerExit(Collider other)
    {
        // Si el jugador sale del trigger
        if (other.CompareTag("Player"))
        {
            // Volvemos a patrullar
            estadoActual = Estados.Patrol;
            jugador = null;
            estoyDisparando = false;
            Debug.Log("Te perdi de vista, vuelvo a patrullar");
        }
    }

    void Patrullar()
    {
        CambiarAnimacion("Rifle Walk");

        // Si no hay puntos de patrulla, solo idle
        if (puntosDePatrulla == null || puntosDePatrulla.Length == 0)
        {
            CambiarAnimacion("Rifle Idle");
            return;
        }

        // Vamos al punto actual
        GameObject puntoDestino = puntosDePatrulla[puntoActual];
        if (puntoDestino != null)
        {
            agente.SetDestination(puntoDestino.transform.position);

            // Si llegamos al punto, vamos al siguiente
            float distanciaAlPunto = Vector3.Distance(transform.position, puntoDestino.transform.position);
            if (distanciaAlPunto < 1f)
            {
                puntoActual++;
                if (puntoActual >= puntosDePatrulla.Length)
                {
                    puntoActual = 0;
                }
            }
        }

        agente.speed = velocidadPatrullar;
    }

    void Perseguir()
    {
        CambiarAnimacion("Rifle Run");

        if (jugador != null)
        {
            agente.SetDestination(jugador.transform.position);
            agente.speed = velocidadPerseguir;

            // Si el jugador se aleja mucho, volvemos a patrullar
            float distancia = Vector3.Distance(transform.position, jugador.transform.position);
            if (distancia > distanciaDejarDePerseguir)
            {
                estadoActual = Estados.Patrol;
                jugador = null;
                Debug.Log("Est� muy lejos, vuelvo a patrullar");
            }
        }
    }

    void Atacar()
    {
        if (jugador.gameObject == null)
        {
            estadoActual = Estados.Patrol;
        }
        
        // Detenemos el movimiento
        agente.SetDestination(transform.position);
        agente.speed = 0f;

        // Miramos hacia el jugador
        if (jugador != null)
        {
            Vector3 direccionAlJugador = jugador.transform.position - transform.position;
            direccionAlJugador.y = 0;

            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionAlJugador);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * 5f);

            // Si se aleja, volvemos a perseguir
            float distancia = Vector3.Distance(transform.position, jugador.transform.position);
            if (distancia > distanciaParaAtacar)
            {
                estadoActual = Estados.Chase;
                estoyDisparando = false;
            }
        }

        CambiarAnimacion("Firing Rifle");
        estoyDisparando = true;
    }

    void CambiarAnimacion(string nuevaAnimacion)
    {
        if (animacionActual != nuevaAnimacion)
        {
            animador.Play(nuevaAnimacion);
            animacionActual = nuevaAnimacion;
        }
    }

    private void FuncionDisparo()
    {
        direccion = (jugador.transform.position - transform.position).normalized;
        GameObject proyectil = Instantiate(bala,transform.position,Quaternion.identity);

        Rigidbody rb = proyectil.GetComponent<Rigidbody>();
        if (rb != null) {
            rb.linearVelocity = direccion * disparoVelocidad;
        }
        Destroy(proyectil,5f); 

    }

    // Para visualizar los rangos en el editor
    void OnDrawGizmosSelected()
    {
        // Rango de ataque en rojo
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaParaAtacar);
    }
}