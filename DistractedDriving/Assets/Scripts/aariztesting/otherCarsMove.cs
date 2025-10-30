using UnityEngine;
using UnityEngine.AI;

public class CarAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public float wanderRadius = 50f;
    public float wanderInterval = 5f;

    float timer;

    void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        timer = wanderInterval;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Pick a new random destination every few seconds
        if (timer >= wanderInterval)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    // Finds a random point on the NavMesh near a position
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layerMask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layerMask);

        return navHit.position;
    }
}