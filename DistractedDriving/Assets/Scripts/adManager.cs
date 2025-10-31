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
        bool runAds = true;
        while (runAds)
        {
            // stop if ads are gone (player is dead)
            if (loveAd == null || techAd == null) { runAds = false; yield return null; }

            // Turn both off first
            loveAd.SetActive(false);
            techAd.SetActive(false);

            // Decide if we should play an ad (20% chance)
            float chance = Random.value; // value between 0.0 and 1.0
            if (chance <= 0.3f)
            {
                // Pick a random ad
                int randomChoice = Random.Range(0, 2);

                if (randomChoice == 0)
                    currentAd = loveAd;
                else
                    currentAd = techAd;

                // Enable it
                currentAd.SetActive(true);

                // Show ad for 3 seconds
                yield return new WaitForSeconds(3f);

                // Hide ad after 3 seconds
                currentAd.SetActive(false);

                // Wait for 3 seconds
                yield return new WaitForSeconds(3f);
            }
            else
            {
                // No ad — just wait the same amount of time before checking again
                yield return new WaitForSeconds(6f);
            }
        }
    }
}