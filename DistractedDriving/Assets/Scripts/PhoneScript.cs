using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhoneScript : MonoBehaviour
{
    public enum state { pickup,dropoff,call}
    public state curState;
    public TextMeshProUGUI pickupTxt; public GameObject pickupScreen; public Color pickupTxtColor1; public Color pickupTxtColor2;
    float flashTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        curState = state.pickup;
    }

    // Update is called once per frame
    void Update()
    {
        flashTimer += Time.deltaTime*3f; if(flashTimer > 1f) { flashTimer = -1f; }
        switch (curState)
        {
            case state.pickup: pickupScreen.SetActive(true); if(flashTimer > 0) { pickupTxt.color = pickupTxtColor1; } else { pickupTxt.color = pickupTxtColor2; }
                    break;
            case state.dropoff:
                break;
            case state.call:
                break;
        }
    }
    public void ChangeState(state newState)
    {
        curState = newState;
        switch (newState) 
        {
            case state.pickup:
                pickupScreen.SetActive(true); 
                break;
            case state.dropoff:
                pickupScreen.SetActive(false);
                break;
            case state.call:
                pickupScreen.SetActive(false);
                break;
        }
    }
}
