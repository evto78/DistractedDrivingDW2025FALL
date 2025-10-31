using System.Collections;
using UnityEngine;

public class adManager : MonoBehaviour
{
    public GameObject loveAd;
    public GameObject techAd;

    private GameObject currentAd;

    SoundManager sm;

    void Start()
    {
        sm = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        sm.StopSoundByKey(4); //love
        sm.StopSoundByKey(5); //tech
        StartCoroutine(ShowRandomAds());
    }

    IEnumerator ShowRandomAds()
    {
        yield return new WaitForSeconds(5f);
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
                { currentAd = loveAd; sm.PlaySoundByKey(4); sm.ToggleLoopingSoundByKey(2); }
                else
                { currentAd = techAd; sm.PlaySoundByKey(5); sm.ToggleLoopingSoundByKey(2); }

                // Enable it
                currentAd.SetActive(true);

                // Show ad for 3 seconds
                yield return new WaitForSeconds(3f);

                // Hide ad after 3 seconds
                currentAd.SetActive(false);
                sm.ToggleLoopingSoundByKey(2);
                sm.StopSoundByKey(4);
                sm.StopSoundByKey(5);
                // Wait for 3 seconds
                yield return new WaitForSeconds(1f);
            }
            else
            {
                // No ad — just wait the same amount of time before checking again
                yield return new WaitForSeconds(4f);
            }
        }
    }
}