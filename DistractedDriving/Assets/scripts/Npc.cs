using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    CarManager car;
    void Start()
    {
        car = FindObjectOfType<CarManager>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("NPC collided with Player");
            car.currentSpeed = 0;
            car.canDrive = false;

        }
    }
}
