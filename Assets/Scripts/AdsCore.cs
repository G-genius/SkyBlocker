using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
public class AdsCore : MonoBehaviour, IUnityAdsShowListener
{
    [SerializeField] private bool _testMode = true;

    private string _gameId = "4776984";

    private string _video = "Interstitial_Android";
    private string _rewardedVideo = "Rewarded_Android";
    private string _banner = "Banner_Android";

    private void Start()
    {
        Advertisement.Initialize(_gameId, _testMode);

        #region Banner

        StartCoroutine(ShowBannerWhenInitialized());
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);

        #endregion
    }
    public static void ShowAdsVideo(string placementId)
    {
        //if (Advertisement.IsReady())
        //{
        //    Advertisement.Show(placementId);
        //}
        //else
        //{
        //    Debug.Log("Adverticement not ready!");
        //}
        Advertisement.Show(placementId);
    }
    IEnumerator ShowBannerWhenInitialized()
    {
        while (!Advertisement.isInitialized)
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.Show(_banner);
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        throw new System.NotImplementedException();
    }
}
