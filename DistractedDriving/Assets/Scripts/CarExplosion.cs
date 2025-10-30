using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarExplosion : MonoBehaviour
{
    List<Rigidbody> parts;
    // Start is called before the first frame update
    void Start()
    {
        parts = new List<Rigidbody>();
        parts.AddRange(GetComponentsInChildren<Rigidbody>());
        foreach(Rigidbody rb in parts)
        {
            rb.transform.parent = null;
            rb.AddForce((rb.transform.position-transform.position).normalized * Random.Range(1f,10f),ForceMode.Impulse);
            rb.AddTorque(Vector3.one*Random.Range(-6f,6f),ForceMode.Impulse);
            Destroy(rb.gameObject, 20f);
        }
        Destroy(gameObject, 10f);
    }
}
