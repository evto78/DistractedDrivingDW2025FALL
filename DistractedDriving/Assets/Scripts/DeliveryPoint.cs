using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryPoint : MonoBehaviour
{
    CarManager carManager;
    BoxCollider carCollider;
    BoxCollider myCollider;

    public GameObject happyIcon;
    public GameObject mildIcon;
    public GameObject angryIcon;

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
        if (carManager.currentSpeed < 200) { slowDownWarning.SetActive(false); } else { return; }
        if (carCollider == null) { return; }
        if (Vector3.Distance(carCollider.transform.position, transform.position) > 40) { return; }
        if (carCollider.bounds.Intersects(myCollider.bounds))
        {
            carManager.Deliver();
            gameObject.SetActive(false);
        }
    }
    public void IconUpdate(float timeLeft)
    {
        happyIcon.SetActive(false);
        mildIcon.SetActive(false);
        angryIcon.SetActive(false);
        if(timeLeft > 0.6f) { happyIcon.SetActive(true); }
        else if(timeLeft > 0.3f) { mildIcon.SetActive(true); }
        else { angryIcon.SetActive(true); }
    }
}
