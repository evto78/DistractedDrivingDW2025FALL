using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Npc : MonoBehaviour
{

    public Animator anim;
    CarManager car;
    ControllerManager controller;
    public GameObject granny;
    public List<Sprite> sprites;
    public SpriteRenderer img;
    bool canLeave = false;
    List<Transform> possibleLocations;
    void Start()
    {
        car = FindObjectOfType<CarManager>();
        controller = FindObjectOfType<ControllerManager>();
        possibleLocations = new List<Transform>();
        Transform parentLocations = GameObject.Find("NPCLOCATIONS").transform;
        for(int i = 0; i < parentLocations.childCount; i++)
        {
            possibleLocations.Add(parentLocations.GetChild(i));
        }
        transform.parent = possibleLocations[Random.Range(0, possibleLocations.Count)];
        transform.localPosition = Vector3.zero; transform.localEulerAngles = Vector3.zero;
        NewSprite();
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
        if (other.CompareTag("NpcCheck"))
        {
            Debug.Log("Player collided with NPC");
            StartCoroutine(Npcs());
        }
    }

    private IEnumerator Npcs()
    {
        granny.SetActive(true);
        anim.enabled = true;
        //Debug.Log("NPC collided with Player");
        yield return new WaitForSeconds(0.01f);
        car.currentSpeed = 0;
        car.canDrive = false;
        canLeave = true;
    }

    private IEnumerator NPCRuns()
    {
        anim.SetBool("isScared", true);
        yield return new WaitForSeconds(2f);
        NewSprite();
        granny.SetActive(false);
    }

    void NewSprite()
    {
        img.sprite = sprites[Random.Range(0, sprites.Count)];
    }
}
