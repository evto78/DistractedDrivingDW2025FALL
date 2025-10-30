using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{

    public Animator anim;
    public GameObject npc;
    CarManager car;

    bool collided = false;
    void Start()
    {
        car = FindObjectOfType<CarManager>();
    }

    void Update()
    {
        if(collided == true && car.horn==true)
        {
            anim.enabled = false;
            car.canDrive = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            StartCoroutine(Npcs());

            collided = true;

        }
    }

    private IEnumerator Npcs()
    {
        anim.enabled = true;
        Debug.Log("NPC collided with Player");
        yield return new WaitForSeconds(0.3f);
        car.currentSpeed = 0;
        car.canDrive = false;
    }
}
