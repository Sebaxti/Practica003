using UnityEngine;
using UnityEngine.AI;

public class SCR_Guardia : MonoBehaviour
{
    NavMeshAgent Agente;
    public enum Estados{Patrol,Chase,Attack};
    public Estados myState;
    public GameObject punto1,punto2,punto3,punto4,punto5;
    public float mySpeed;
    public int numeracion=0;
    private Animator animador;
    private bool shooting;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animador = GetComponent<Animator>();
        Agente =  GetComponent<NavMeshAgent>();
        myState=Estados.Patrol;

        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position,forward,Color.red);

        Debug.Log(numeracion);
        switch (myState)
        {
            case Estados.Patrol:
                Patrulla();
                break;
            
            case Estados.Chase:
                Persigue();
                break;
            
            case Estados.Attack:
                //Ataca();
                break;
            default:
                break;
    }
}

void Patrulla()
    { 
        shooting=false;
        animador.Play("Rifle Walk");
        if(numeracion==0)
        {
            Agente.SetDestination(punto1.transform.position);
            if(Vector3.Distance(transform.position,punto1.transform.position)<0.5f)
            {
                numeracion++;
            }
        }

        if(numeracion==1)
        {
            Agente.SetDestination(punto2.transform.position);
            if(Vector3.Distance(transform.position,punto2.transform.position)<0.5f)
            {
                numeracion++;
            }
        }

        if(numeracion==2)
        {
            Agente.SetDestination(punto3.transform.position);
            if(Vector3.Distance(transform.position,punto3.transform.position)<0.5f)
            {
                numeracion++;
            }
        }

        if(numeracion==3)
        {
            Agente.SetDestination(punto4.transform.position);
            if(Vector3.Distance(transform.position,punto4.transform.position)<0.5f)
            {
                numeracion++;
            }
        }

        if(numeracion==4)
        {
            Agente.SetDestination(punto5.transform.position);
            if(Vector3.Distance(transform.position,punto5.transform.position)<0.5f)
            {
                numeracion=0;
            }
        }
    }

    void Persigue ()
    {
        
    }
}
