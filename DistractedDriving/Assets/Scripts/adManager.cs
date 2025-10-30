using System.Collections;
using UnityEngine;

public class adManager : MonoBehaviour
{
    public GameObject loveAd;
    public GameObject techAd;

    private GameObject currentAd;

    void Start()
    {
        StartCoroutine(ShowRandomAds());
    }

    IEnumerator ShowRandomAds()
    {
        while (true)
        {
            // Turn both off first
            loveAd.SetActive(false);
            techAd.SetActive(false);

            // Decide if we should play an ad (20% chance)
            float chance = Random.value; // value between 0.0 and 1.0
            if (chance <= 0.5f)
            {
                // Pick a random ad
                int randomChoice = Random.Range(0, 2);

                if (randomChoice == 0)
                    currentAd = loveAd;
                else
                    currentAd = techAd;

                // Enable it
                currentAd.SetActive(true);

                // Show ad for 5 seconds
                yield return new WaitForSeconds(5f);

                // Hide ad after 5 seconds
                currentAd.SetActive(false);
            }
            else
            {
                // No ad — just wait the same amount of time before checking again
                yield return new WaitForSeconds(5f);
            }
        }
    }
}