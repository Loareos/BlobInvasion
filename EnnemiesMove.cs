using UnityEngine;
using UnityEngine.AI;

public class EnnemiesMove : MonoBehaviour
{
    //public GameObject objectif;
    NavMeshAgent agent;
    GameObject cristalCible;
    // Update is called once per frame

    private void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        cristalCible = GameObject.FindGameObjectWithTag("cristal");
        agent.SetDestination(cristalCible.transform.position);
    }
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
//        agent.SetDestination(cristalCible.transform.position);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "cristal")
        {
            col.GetComponent<cristal>().life -= 1;
            Destroy(gameObject);
        }
    }
}
