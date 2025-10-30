using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RearViewScript : MonoBehaviour
{
    GameObject cam; Vector3 baseAngle; Vector3 basePos;
    void Start()
    {
        basePos = transform.localPosition;
        baseAngle = transform.localEulerAngles;
        cam = transform.GetChild(0).gameObject;
    }
    void Update()
    {
        cam.SetActive((Vector3.Distance(transform.localEulerAngles, baseAngle) < 0.03f) && (Vector3.Distance(transform.localPosition, basePos) < 0.03f));
    }
}
