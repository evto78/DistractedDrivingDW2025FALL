using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryPoint : MonoBehaviour
{
    CarManager carManager;
    BoxCollider carCollider;
    BoxCollider myCollider;

    public GameObject slowDownWarning; float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        carCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider>();
        carManager = carCollider.gameObject.GetComponent<CarManager>();
        myCollider = GetComponent<BoxCollider>();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * 4f; if (timer > 1) { timer = -1; }
        slowDownWarning.SetActive(timer > 0);
        if (carManager.currentSpeed < 30) { slowDownWarning.SetActive(false); } else { return; }
        if (carCollider == null) { return; }
        if (Vector3.Distance(carCollider.transform.position, transform.position) > 40) { return; }
        if (carCollider.bounds.Intersects(myCollider.bounds))
        {
            carManager.Deliver();
            gameObject.SetActive(false);
        }
    }
}
