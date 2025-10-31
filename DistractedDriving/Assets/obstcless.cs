using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstcless : MonoBehaviour
{
    CarManager carManager;
    Rigidbody rb;
    void Start()
    {
        carManager = FindObjectOfType<CarManager>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NpcCheck")
        {
            Debug.Log("Obstacle collided with Player");
            carManager.CameraShake(1f); carManager.HitSomething();
            rb.useGravity = true;
            rb.AddForce((rb.transform.position - carManager.transform.position).normalized * Random.Range(10f, 25f), ForceMode.Impulse);
            rb.AddTorque(Vector3.one * Random.Range(-10f, 10f), ForceMode.Impulse);
        }
    }
}
