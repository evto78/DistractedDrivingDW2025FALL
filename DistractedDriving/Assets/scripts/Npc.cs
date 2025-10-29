using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{

    public Animator animation;
    public GameObject npc;
    CarManager car;
    void Start()
    {
        car = FindObjectOfType<CarManager>();
    }

    void Update()
    {
        if(car.horned == true)
        {
            //gameObject.SetActive(false);
            car.canDrive = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            StartCoroutine(Npcs());
        }
    }

    private IEnumerator Npcs()
    {
        animation.enabled = true;
        Debug.Log("NPC collided with Player");
        yield return new WaitForSeconds(0.3f);
        car.currentSpeed = 0;
        car.canDrive = false;
    }
}
