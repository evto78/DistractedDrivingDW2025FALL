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
            carManager.CameraShake(1f);
            rb.useGravity = true;
            rb.AddForce((rb.transform.position - carManager.transform.position).normalized * Random.Range(1f, 10f), ForceMode.Impulse);
            rb.AddTorque(Vector3.one * Random.Range(-6f, 6f), ForceMode.Impulse);
        }
    }
}
