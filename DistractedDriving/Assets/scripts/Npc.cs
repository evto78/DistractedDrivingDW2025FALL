using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{

    public Animator anim;
    public GameObject npc;
    CarManager car;
    public GameObject granny;

    bool collided = false;
    void Start()
    {
        car = FindObjectOfType<CarManager>();
    }

    void Update()
    {
        if(car.canDrive == false && car.horn==true)
        {
            anim.enabled = false;
            car.canDrive = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player collided with NPC");
            StartCoroutine(Npcs());

            collided = true;

        }
    }

    private IEnumerator Npcs()
    {
        granny.SetActive(true);
        anim.enabled = true;
        //Debug.Log("NPC collided with Player");
        yield return new WaitForSeconds(0.3f);
        car.currentSpeed = 0;
        car.canDrive = false;
    }
}
