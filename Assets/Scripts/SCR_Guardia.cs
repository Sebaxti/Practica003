using UnityEngine;
using UnityEngine.AI;

public class SCR_Guardia : MonoBehaviour
{
    private bool estoyDisparando = false;
    private string animacionActual = "";
    private Vector3 direccion;
    private NavMeshAgent agente;
    private Animator animador;
    private float tiempoUltimoDisparo=0f;
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
    public float velocidadRotacion;
    public float distanciaParaAtacar;
    public float disparoVelocidad;
    public float tiempoEntreDisparos;


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
        // Rayo para debug
        Vector3 direccionAdelante = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, direccionAdelante, Color.red);

        // Maquina de estados
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

        // Verificar la distancia para atacar
        if (estadoActual == Estados.Chase && jugador != null)
        {
            float distancia = Vector3.Distance(transform.position, jugador.transform.position);
            if (distancia <= distanciaParaAtacar)
            {
                estadoActual = Estados.Attack;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && estadoActual == Estados.Patrol)
        {
            jugador = other.gameObject;
            estadoActual = Estados.Chase;
            //Debug.Log("persiguiendo");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //patrullar
            estadoActual = Estados.Patrol;
            jugador = null;
            estoyDisparando = false;
            //Debug.Log("patrullar");
        }
    }

    void Patrullar()
    {
        CambiarAnimacion("Rifle Walk");

        // Si no hay puntos de patrulla
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

            // Siguiente punto
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

            // Volver a patrullar
            float distancia = Vector3.Distance(transform.position, jugador.transform.position);
            if (distancia > distanciaDejarDePerseguir)
            {
                estadoActual = Estados.Patrol;
                jugador = null;
                //Debug.Log("patrullar");
            }
        }
    }

    void Atacar()
    {
        if (jugador == null)
        {
            estadoActual = Estados.Patrol;
            return;
        }
        
        // Detener el movimiento
        agente.SetDestination(transform.position);
        agente.speed = 0f;

        
        // Miramos hacia el jugador
            Vector3 direccionAlJugador = jugador.transform.position - transform.position;
            direccionAlJugador.y = 0;

            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionAlJugador);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * velocidadRotacion);

            // Volver a perseguir
            float distancia = Vector3.Distance(transform.position, jugador.transform.position);
            if (distancia > distanciaParaAtacar)
            {
                estadoActual = Estados.Chase;
                estoyDisparando = false;
            }

        CambiarAnimacion("Firing Rifle");
        estoyDisparando = true;

        tiempoUltimoDisparo += Time.deltaTime;
        if (tiempoUltimoDisparo >= tiempoEntreDisparos)
        {
            FuncionDisparo();
            tiempoUltimoDisparo = 0f;
        }

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
        if (jugador == null || bala == null) return; 

        direccion = (jugador.transform.position - transform.position).normalized;
        GameObject proyectil = Instantiate(bala, transform.position + Vector3.up * 1.7f + transform.forward * 1f, Quaternion.identity);
                                                                  

        Rigidbody rb = proyectil.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direccion * disparoVelocidad;
        }
        Destroy(proyectil, 5f);

    }

    void OnDrawGizmosSelected()
    {
        // Rango de ataque
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaParaAtacar);
    }
}