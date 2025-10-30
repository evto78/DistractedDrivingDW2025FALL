using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCamera : MonoBehaviour
{
    Transform car; Vector3 lastCarPos;
    // Start is called before the first frame update
    void Start()
    {
        car = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float followSpeed = 4f;
        if (car != null) 
        { 
            lastCarPos = car.position;
            followSpeed = 2f;
        }
        transform.position = Vector3.Lerp(transform.position, lastCarPos + new Vector3(0, 10, -20), Time.deltaTime / followSpeed);
        transform.LookAt(lastCarPos);
    }
}
