﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    string gameId = "1234567";
    string myPlacementId = "rewardedVideo";
    bool testMode = true;
    public ushort Gems =0;

    // Initialize the Ads listener and service:
    void Start()
    {
      
    }
    public void setGem(ushort gem)
    {
        Gems = gem;
    }
    public void showAd()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, testMode);

    }
    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            GameManager.instance.diamond += Gems;
            SaveSystem.SavePlayer();
            Advertisement.RemoveListener(this);
            GetComponent<loadingLevel>().LoadLevel(0);
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsReady(string placementId)
    {

        // If the ready Placement is rewarded, show the ad:
        if (placementId == myPlacementId)
        {
            
         
            Advertisement.Show(myPlacementId);
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }
}
