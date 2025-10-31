using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidesWithCar : MonoBehaviour
{
    CarManager carManager;
    public BoxCollider carCollider;
    public BoxCollider myCollider;
    void Start()
    {
        carCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider>();
        carManager = carCollider.gameObject.GetComponent<CarManager>();
        myCollider = GetComponent<BoxCollider>();
    }
    void Update()
    {
        return;
        if (carCollider == null) { return; }
        if (Vector3.Distance(carCollider.transform.position, transform.position) > 160) { return; }
        //if ((carCollider.ClosestPoint(myCollider.transform.position) + myCollider.ClosestPoint(carCollider.transform.position)).magnitude < 1) { Debug.Log("COLLISION WITH " + gameObject.name); carManager.Crash(); }
        if (carCollider.bounds.Intersects(myCollider.bounds)) { Debug.Log("COLLISION WITH " + gameObject.name); carManager.Crash(); }
    }
}
