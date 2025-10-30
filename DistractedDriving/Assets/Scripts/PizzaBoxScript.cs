using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaBoxScript : MonoBehaviour
{
    public bool coverBox;
    public float baseStability;
    public float currentStability;
    bool gone;
    Vector3 baseRotation;
    void Start()
    {
        gone = false;
        baseRotation = transform.localEulerAngles;
    }
    void Update()
    {
        ManageStability();
    }
    void ManageStability()
    {
        if (gone) { return; }
        if (coverBox) 
        {

        }
        else
        {
            transform.localEulerAngles = new Vector3(baseRotation.x, baseRotation.y, baseRotation.z + (currentStability * 13 * (Mathf.Abs(currentStability)/Mathf.Abs(baseStability))));
        }
        if(Mathf.Abs(currentStability) > baseStability)
        {
            gone = true;
            gameObject.AddComponent<Rigidbody>().AddForce(Vector3.up * 5f, ForceMode.Impulse);
            transform.SetParent(null);
            Destroy(gameObject, 2f);
            Destroy(this);
        }
    }
}
