using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carMovings : MonoBehaviour
{
    float xRotation;
    float yRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(x, 0f, z);
        transform.Translate(movement * Time.deltaTime * 10f);

       



    }
}
