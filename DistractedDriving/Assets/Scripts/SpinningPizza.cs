using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningPizza : MonoBehaviour
{
    public float spinSpeed;
    public bool pickUpAble;

    float timer = 0;

    CarManager carManager;
    BoxCollider carCollider;
    BoxCollider myCollider;
    // Start is called before the first frame update
    void Start()
    {
        carCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider>();
        carManager = carCollider.gameObject.GetComponent<CarManager>();
        myCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles += Vector3.up * Time.deltaTime * spinSpeed;
        timer -= Time.deltaTime;
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(timer < 0);
        }
        if (!pickUpAble || timer > 0f) { return; }
        if (carCollider == null) { return; }
        if (Vector3.Distance(carCollider.transform.position, transform.position) > 40) { return; }
        if (carCollider.bounds.Intersects(myCollider.bounds)) 
        {
            carManager.PickUpPizza();
            timer = 2f;
        }
    }
}
