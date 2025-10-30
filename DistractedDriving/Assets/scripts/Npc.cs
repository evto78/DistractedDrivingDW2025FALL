using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{

    public Animator anim;
    public GameObject npc;
    CarManager car;
    ControllerManager controller;
    public GameObject granny;
    bool canLeave = false;

    bool collided = false;
    void Start()
    {
        car = FindObjectOfType<CarManager>();
        controller = FindObjectOfType<ControllerManager>();

    }

    void Update()
    {
        if(canLeave == true)
        {
            if(controller.buttonsPressed || Input.GetKey(KeyCode.Space))
            {
                car.canDrive = true;
                StartCoroutine(NPCRuns());
            }
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
        yield return new WaitForSeconds(0.1f);
        car.currentSpeed = 0;
        car.canDrive = false;
        canLeave = true;
    }

    private IEnumerator NPCRuns()
    {
        anim.SetBool("isScared", true);
        yield return new WaitForSeconds(2f);
        granny.SetActive(false);
    }
}
