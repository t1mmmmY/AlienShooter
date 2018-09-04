using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdvertisingManager : BaseSingleton<AdvertisingManager>
{
	public int adFrequency { get; private set; }
	int defaultAdFrequency = 1;
	
	void Start()
	{
		adFrequency = defaultAdFrequency;
		RemoteSettings.Updated += new RemoteSettings.UpdatedEventHandler(HandleRemoteUpdate);
	}

	private void HandleRemoteUpdate()
	{
		adFrequency = RemoteSettings.GetInt("AdFrequency", defaultAdFrequency);
		Debug.Log("Update AdFrequency " + adFrequency.ToString());
	}

	public void ShowBanner(System.Action onClickBanner = null)
	{
	}

	public void HideBanner()
	{
	}

	public void ShowFullScreenAd(System.Action onCloseAd)
	{
		if (Advertisement.IsReady("fullscreenbanner"))
		{
			var options = new ShowOptions { resultCallback = HandleBannerResult };
			Advertisement.Show("fullscreenbanner", options);
		}
	}

	public void ShowVideo(System.Action onCloseAd)
	{
		if (Advertisement.IsReady("video"))
		{
			var options = new ShowOptions { resultCallback = HandleVideoResult };
			Advertisement.Show("video", options);
		}
	}

	private void HandleBannerResult(ShowResult result)
	{
		switch (result)
		{
			case ShowResult.Finished:
				Debug.Log("The ad was successfully shown.");
				//
				// YOUR CODE TO REWARD THE GAMER
				// Give coins etc.
				break;
			case ShowResult.Skipped:
				Debug.Log("The ad was skipped before reaching the end.");
				break;
			case ShowResult.Failed:
				Debug.LogError("The ad failed to be shown.");
				break;
		}
	}

	private void HandleVideoResult(ShowResult result)
	{
		switch (result)
		{
			case ShowResult.Finished:
				Debug.Log("The video was successfully shown.");
				//
				// YOUR CODE TO REWARD THE GAMER
				// Give coins etc.
				break;
			case ShowResult.Skipped:
				Debug.Log("The video was skipped before reaching the end.");
				break;
			case ShowResult.Failed:
				Debug.LogError("The video failed to be shown.");
				break;
		}
	}
}

//	#if UNITY_ANDROID || UNITY_EDITOR
//	private readonly string[] _bannerAdUnits = { "3a85f462201b43dd9b600544a7554b0b" };
//	private readonly string[] _interstitialAdUnits = { "60bac576b36b41c98b895eb281f01c61" };
//	private readonly string[] _rewardedVideoAdUnits = { "fe1670bf1da644189f0b6ac7633a3a00" };
//	#endif
//
//	string interstitialAdUnit = "";
//	string videoUnit = "";
//	string bannerUnit = "";
//	System.Action onCloseAd;
//	System.Action onBannerInit = null;
//	System.Action onInterstitialInit = null;
//	System.Action onVideoInit = null;
//
//	bool bannerLoaded = false;
//	bool interstitialLoaded = false;
//	bool videoLoaded = false;
//
//	void Start()
//	{
//
//		string anyAdUnitId = _bannerAdUnits[0];
//		MoPub.InitializeSdk(anyAdUnitId);
//
//		MoPub.LoadBannerPluginsForAdUnits(_bannerAdUnits);
//		MoPub.LoadInterstitialPluginsForAdUnits(_interstitialAdUnits);
//		MoPub.LoadRewardedVideoPluginsForAdUnits(_rewardedVideoAdUnits);
//
//		AddEventHandlers();
//	}
//
//	void GenerateNewBanner()
//	{
//		bannerLoaded = false;
//		bannerUnit = _bannerAdUnits[Random.Range(0, _bannerAdUnits.Length)];
//		MoPub.CreateBanner(bannerUnit, MoPubBase.AdPosition.BottomCenter);
//	}
//
//	void GenerateNewInterstitial()
//	{
//		interstitialLoaded = false;
//		interstitialAdUnit = _interstitialAdUnits[Random.Range(0, _interstitialAdUnits.Length)];
//		MoPub.RequestInterstitialAd(interstitialAdUnit);
//	}
//
//	void GenerateNewVideo()
//	{
//		videoLoaded = false;
//		videoUnit = _rewardedVideoAdUnits[Random.Range(0, _rewardedVideoAdUnits.Length)];
//		MoPub.RequestRewardedVideo(videoUnit);
//	}
//
//	protected override void OnDestroy()
//	{
//		base.OnDestroy();
//
//		RemoveEventHandlers();
//	}
//
//	public void ShowFullScreenAd(System.Action onCloseAd)
//	{
//		if (!interstitialLoaded)
//		{
//			onInterstitialInit = () =>
//			{
//				ShowFullScreenAd(onCloseAd);
//			};
//			return;
//		}
//		else
//		{
//			onInterstitialInit = null;
//		}
//		this.onCloseAd = onCloseAd;
//		MoPub.ShowInterstitialAd(interstitialAdUnit);
//	}
//
//	public void ShowVideo(System.Action onCloseAd)
//	{
//		if (!videoLoaded)
//		{
//			onVideoInit = () =>
//				{
//					ShowVideo(onCloseAd);
//				};
//			return;
//		}
//		else
//		{
//			onVideoInit = null;
//		}
//		this.onCloseAd = onCloseAd;
//		MoPub.ShowRewardedVideo(interstitialAdUnit);
//	}
//
//	public void ShowBanner(System.Action onCloseAd)
//	{
//		if (!bannerLoaded)
//		{
//			onBannerInit = () =>
//				{
//					ShowBanner(onCloseAd);
//				};
//			return;
//		}
//		else
//		{
//			onBannerInit = null;
//		}
//		this.onCloseAd = onCloseAd;
//		MoPub.ShowBanner(bannerUnit, true);
//	}
//
//	public void HideBanner()
//	{
//		if (!MoPub.IsSdkInitialized)
//		{
//			return;
//		}
//		MoPub.ShowBanner(bannerUnit, false);
//	}
//
//
//	#region EventListener
//
//	void AddEventHandlers()
//	{
//		MoPubManager.OnSdkInitalizedEvent += OnSdkInitializedEvent;
//
//		MoPubManager.OnConsentStatusChangedEvent += OnConsentStatusChangedEvent;
//		MoPubManager.OnConsentDialogLoadedEvent += OnConsentDialogLoadedEvent;
//		MoPubManager.OnConsentDialogFailedEvent += OnConsentDialogFailedEvent;
//		MoPubManager.OnConsentDialogShownEvent += OnConsentDialogShownEvent;
//
//		MoPubManager.OnAdLoadedEvent += OnAdLoadedEvent;
//		MoPubManager.OnAdFailedEvent += OnAdFailedEvent;
//		MoPubManager.OnAdClickedEvent += OnAdClickedEvent;
//		MoPubManager.OnAdExpandedEvent += OnAdExpandedEvent;
//		MoPubManager.OnAdCollapsedEvent += OnAdCollapsedEvent;
//
//		MoPubManager.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
//		MoPubManager.OnInterstitialFailedEvent += OnInterstitialFailedEvent;
//		MoPubManager.OnInterstitialShownEvent += OnInterstitialShownEvent;
//		MoPubManager.OnInterstitialClickedEvent += OnInterstitialClickedEvent;
//		MoPubManager.OnInterstitialDismissedEvent += OnInterstitialDismissedEvent;
//		MoPubManager.OnInterstitialExpiredEvent += OnInterstitialExpiredEvent;
//
//		MoPubManager.OnRewardedVideoLoadedEvent += OnRewardedVideoLoadedEvent;
//		MoPubManager.OnRewardedVideoFailedEvent += OnRewardedVideoFailedEvent;
//		MoPubManager.OnRewardedVideoExpiredEvent += OnRewardedVideoExpiredEvent;
//		MoPubManager.OnRewardedVideoShownEvent += OnRewardedVideoShownEvent;
//		MoPubManager.OnRewardedVideoClickedEvent += OnRewardedVideoClickedEvent;
//		MoPubManager.OnRewardedVideoFailedToPlayEvent += OnRewardedVideoFailedToPlayEvent;
//		MoPubManager.OnRewardedVideoReceivedRewardEvent += OnRewardedVideoReceivedRewardEvent;
//		MoPubManager.OnRewardedVideoClosedEvent += OnRewardedVideoClosedEvent;
//		MoPubManager.OnRewardedVideoLeavingApplicationEvent += OnRewardedVideoLeavingApplicationEvent;
//
//		#if mopub_native_beta
//		MoPubManager.OnNativeLoadEvent += OnNativeLoadEvent;
//		MoPubManager.OnNativeFailEvent += OnNativeFailEvent;
//		MoPubManager.OnNativeImpressionEvent += OnNativeImpressionEvent;
//		MoPubManager.OnNativeClickEvent += OnNativeClickEvent;
//		#endif
//	}
//
//	void RemoveEventHandlers()
//	{
//		MoPubManager.OnSdkInitalizedEvent -= OnSdkInitializedEvent;
//
//		MoPubManager.OnConsentStatusChangedEvent -= OnConsentStatusChangedEvent;
//		MoPubManager.OnConsentDialogLoadedEvent -= OnConsentDialogLoadedEvent;
//		MoPubManager.OnConsentDialogFailedEvent -= OnConsentDialogFailedEvent;
//		MoPubManager.OnConsentDialogShownEvent -= OnConsentDialogShownEvent;
//
//		MoPubManager.OnAdLoadedEvent -= OnAdLoadedEvent;
//		MoPubManager.OnAdFailedEvent -= OnAdFailedEvent;
//		MoPubManager.OnAdClickedEvent -= OnAdClickedEvent;
//		MoPubManager.OnAdExpandedEvent -= OnAdExpandedEvent;
//		MoPubManager.OnAdCollapsedEvent -= OnAdCollapsedEvent;
//
//		MoPubManager.OnInterstitialLoadedEvent -= OnInterstitialLoadedEvent;
//		MoPubManager.OnInterstitialFailedEvent -= OnInterstitialFailedEvent;
//		MoPubManager.OnInterstitialShownEvent -= OnInterstitialShownEvent;
//		MoPubManager.OnInterstitialClickedEvent -= OnInterstitialClickedEvent;
//		MoPubManager.OnInterstitialDismissedEvent -= OnInterstitialDismissedEvent;
//		MoPubManager.OnInterstitialExpiredEvent -= OnInterstitialExpiredEvent;
//
//		MoPubManager.OnRewardedVideoLoadedEvent -= OnRewardedVideoLoadedEvent;
//		MoPubManager.OnRewardedVideoFailedEvent -= OnRewardedVideoFailedEvent;
//		MoPubManager.OnRewardedVideoExpiredEvent -= OnRewardedVideoExpiredEvent;
//		MoPubManager.OnRewardedVideoShownEvent -= OnRewardedVideoShownEvent;
//		MoPubManager.OnRewardedVideoClickedEvent -= OnRewardedVideoClickedEvent;
//		MoPubManager.OnRewardedVideoFailedToPlayEvent -= OnRewardedVideoFailedToPlayEvent;
//		MoPubManager.OnRewardedVideoReceivedRewardEvent -= OnRewardedVideoReceivedRewardEvent;
//		MoPubManager.OnRewardedVideoClosedEvent -= OnRewardedVideoClosedEvent;
//		MoPubManager.OnRewardedVideoLeavingApplicationEvent -= OnRewardedVideoLeavingApplicationEvent;
//
//		#if mopub_native_beta
//		MoPubManager.OnNativeLoadEvent -= OnNativeLoadEvent;
//		MoPubManager.OnNativeFailEvent -= OnNativeFailEvent;
//		MoPubManager.OnNativeImpressionEvent -= OnNativeImpressionEvent;
//		MoPubManager.OnNativeClickEvent -= OnNativeClickEvent;
//		#endif
//	}
//
//	private void AdFailed(string adUnitId, string action, string error)
//	{
//		var errorMsg = "Failed to " + action + " ad unit " + adUnitId;
//		if (!string.IsNullOrEmpty(error))
//			errorMsg += ": " + error;
//		Debug.LogError(errorMsg);
//	}
//
//
//	private void OnSdkInitializedEvent(string adUnitId)
//	{
//		Debug.Log("OnSdkInitializedEvent: " + adUnitId);
//		GenerateNewBanner();
//		GenerateNewInterstitial();
//		GenerateNewVideo();
//	}
//
//
//	private void OnConsentStatusChangedEvent(MoPub.Consent.Status oldStatus, MoPub.Consent.Status newStatus,
//		bool canCollectPersonalInfo)
//	{
//		Debug.Log("OnConsetStatusChangedEvent: old=" + oldStatus + " new=" + newStatus + " personalInfoOk=" + canCollectPersonalInfo);
//	}
//
//
//	private void OnConsentDialogLoadedEvent()
//	{
//		Debug.Log("OnConsentDialogLoadedEvent: dialog loaded");
//	}
//
//
//	private void OnConsentDialogFailedEvent(string err)
//	{
//		Debug.Log("OnConsentDialogFailedEvent: " + err);
//	}
//
//
//	private void OnConsentDialogShownEvent()
//	{
//		Debug.Log("OnConsentDialogShownEvent: dialog shown");
//	}
//
//
//	// Banner Events
//
//
//	private void OnAdLoadedEvent(string adUnitId, float height)
//	{
//		Debug.Log("OnAdLoadedEvent: " + adUnitId + " height: " + height);
//		bannerLoaded = true;
//		if (onBannerInit != null)
//		{
//			onBannerInit();
//		}
//	}
//
//
//	private void OnAdFailedEvent(string adUnitId, string error)
//	{
//		AdFailed(adUnitId, "load banner", error);
//	}
//
//
//	private void OnAdClickedEvent(string adUnitId)
//	{
//		Debug.Log("OnAdClickedEvent: " + adUnitId);
//		GenerateNewBanner();
//		if (onCloseAd != null)
//		{
//			onCloseAd();
//		}
//	}
//
//
//	private void OnAdExpandedEvent(string adUnitId)
//	{
//		Debug.Log("OnAdExpandedEvent: " + adUnitId);
//	}
//
//
//	private void OnAdCollapsedEvent(string adUnitId)
//	{
//		Debug.Log("OnAdCollapsedEvent: " + adUnitId);
//	}
//
//
//	// Interstitial Events
//
//
//	private void OnInterstitialLoadedEvent(string adUnitId)
//	{
//		Debug.Log("OnInterstitialLoadedEvent: " + adUnitId);
//		interstitialLoaded = true;
//		if (onInterstitialInit != null)
//		{
//			onInterstitialInit();
//		}
//	}
//
//
//	private void OnInterstitialFailedEvent(string adUnitId, string error)
//	{
//		AdFailed(adUnitId, "load interstitial", error);
//	}
//
//
//	private void OnInterstitialShownEvent(string adUnitId)
//	{
//		Debug.Log("OnInterstitialShownEvent: " + adUnitId);
//	}
//
//
//	private void OnInterstitialClickedEvent(string adUnitId)
//	{
//		Debug.Log("OnInterstitialClickedEvent: " + adUnitId);
//		GenerateNewInterstitial();
//		if (onCloseAd != null)
//		{
//			onCloseAd();
//		}
//	}
//
//
//	private void OnInterstitialDismissedEvent(string adUnitId)
//	{
//		Debug.Log("OnInterstitialDismissedEvent: " + adUnitId);
//		GenerateNewInterstitial();
//		if (onCloseAd != null)
//		{
//			onCloseAd();
//		}
//	}
//
//
//	private void OnInterstitialExpiredEvent(string adUnitId)
//	{
//		Debug.Log("OnInterstitialExpiredEvent: " + adUnitId);
//	}
//
//
//	// Rewarded Video Events
//
//
//	private void OnRewardedVideoLoadedEvent(string adUnitId)
//	{
//		Debug.Log("OnRewardedVideoLoadedEvent: " + adUnitId);
//		videoLoaded = true;
//		if (onVideoInit != null)
//		{
//			onVideoInit();
//		}
//	}
//
//
//	private void OnRewardedVideoFailedEvent(string adUnitId, string error)
//	{
//		AdFailed(adUnitId, "load rewarded video", error);
//	}
//
//
//	private void OnRewardedVideoExpiredEvent(string adUnitId)
//	{
//		Debug.Log("OnRewardedVideoExpiredEvent: " + adUnitId);
//	}
//
//
//	private void OnRewardedVideoShownEvent(string adUnitId)
//	{
//		Debug.Log("OnRewardedVideoShownEvent: " + adUnitId);
//	}
//
//
//	private void OnRewardedVideoClickedEvent(string adUnitId)
//	{
//		Debug.Log("OnRewardedVideoClickedEvent: " + adUnitId);
//		GenerateNewVideo();
//		if (onCloseAd != null)
//		{
//			onCloseAd();
//		}
//	}
//
//
//	private void OnRewardedVideoFailedToPlayEvent(string adUnitId, string error)
//	{
//		AdFailed(adUnitId, "play rewarded video", error);
//	}
//
//
//	private void OnRewardedVideoReceivedRewardEvent(string adUnitId, string label, float amount)
//	{
//		Debug.Log("OnRewardedVideoReceivedRewardEvent for ad unit id " + adUnitId + " currency:" + label + " amount:" + amount);
//		GenerateNewVideo();
//		if (onCloseAd != null)
//		{
//			onCloseAd();
//		}
//	}
//
//
//	private void OnRewardedVideoClosedEvent(string adUnitId)
//	{
//		Debug.Log("OnRewardedVideoClosedEvent: " + adUnitId);
//		GenerateNewVideo();
//		if (onCloseAd != null)
//		{
//			onCloseAd();
//		}
//	}
//
//
//	private void OnRewardedVideoLeavingApplicationEvent(string adUnitId)
//	{
//		Debug.Log("OnRewardedVideoLeavingApplicationEvent: " + adUnitId);
//		GenerateNewVideo();
//		if (onCloseAd != null)
//		{
//			onCloseAd();
//		}
//	}
//
//
//	#if mopub_native_beta
//	private void OnNativeLoadEvent(string adUnitId, AbstractNativeAd.Data nativeAdData)
//	{
//	Debug.Log("OnNativeLoadEvent: ad unit id: " + adUnitId + " data: " + nativeAdData);
//	_demoGUI.NativeAdLoaded(adUnitId, nativeAdData);
//	}
//
//
//	private void OnNativeFailEvent(string adUnitId, string error)
//	{
//	AdFailed(adUnitId, "load native ad", error);
//	}
//
//
//	private void OnNativeImpressionEvent(string adUnitId)
//	{
//	Debug.Log("OnNativeImpressionEvent: " + adUnitId);
//	}
//
//
//	private void OnNativeClickEvent(string adUnitId)
//	{
//	Debug.Log("OnNativeClickEvent: " + adUnitId);
//	}
//	#endif
//
//	#endregion
//}
