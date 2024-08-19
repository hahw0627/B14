using GoogleMobileAds.Api;
using UnityEngine;

public class GoogleRewardedAds : MonoBehaviour
{
#if UNITY_ANDROID
<<<<<<< Updated upstream
    private const string AD_UNIT_ID = "ca-app-pub-3940256099942544/5354046379"; // 출시 전에는 구글 애드몹에서 제공하는 테스트 용 ID를 넣어야 함
=======
    private const string AD_UNIT_ID = "ca-app-pub-3940256099942544/5354046379";
>>>>>>> Stashed changes
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
    private string _adUnitId = "unused";
#endif

    [SerializeField]
    private GoldAcquireEffect _goldAcquireEffect;

    [SerializeField]
    private Transform _startPositionTransformOfEffect;

    private const byte AD_VIEW_GEM_AMOUNT = 50;

    // Start is called before the first frame update
    private void Start()
    {
        _goldAcquireEffect.OnEffectCompleted += HandleEffectCompleted;
        MobileAds.Initialize(_ =>
        {
            // This callback is called once the MobileAds SDK is initialized.
            LoadRewardedAd();
        });
    }

    private static void HandleEffectCompleted()
    {
        DataManager.Instance.AddGem(AD_VIEW_GEM_AMOUNT);
    }

    private RewardedAd _rewardedAd;

    /// <summary>
    /// Loads the rewarded ad.
    /// </summary>
    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("<color=#87ceeb>Loading the rewarded ad.</color>");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(AD_UNIT_ID, adRequest,
            (ad, error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("<color=red>Rewarded ad failed to load an ad " +
                                   "with error : " + error + "</color>");
                    return;
                }

                Debug.Log("<color=#87ceeb>Rewarded ad loaded with response : "
                          + ad.GetResponseInfo() + "</color>");

                _rewardedAd = ad;

                RegisterEventHandlers(_rewardedAd);
            });
    }

    public void ShowRewardedAd()
    {
        const string rewardMsg =
            "<color=#87ceeb>Rewarded ad rewarded the user. Type: {0}, amount: {1}.</color>";

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show(reward =>
            {
                // TODO: Reward the user.
                Debug.Log(string.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += adValue => { Debug.Log($"<color=#87ceeb>Rewarded ad paid {adValue.Value} {adValue.CurrencyCode}.</color>"); };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () => { Debug.Log("<color=#87ceeb>Rewarded ad recorded an impression.</color>"); };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () => { Debug.Log("<color=#87ceeb>Rewarded ad was clicked.</color>"); };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () => { Debug.Log("<color=#87ceeb>Rewarded ad full screen content opened.</color>"); };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("<color=#87ceeb>Rewarded ad full screen content closed.</color>");
            LoadRewardedAd();
<<<<<<< Updated upstream
            gameObject.GetComponent<AdButtonManager>().StartCooldown(AdButtonManager.COOLDOWN_DURATION); // 광고 쿨다운 시작
            _goldAcquireEffect.PlayGoldAcquireEffect(_startPositionTransformOfEffect.position,
                AD_VIEW_GEM_AMOUNT); // 코인 이펙트 시작
=======
            gameObject.GetComponent<AdButtonManager>().StartCooldown(AdButtonManager.COOLDOWN_DURATION); // ���� ��ٿ� ����
            _goldAcquireEffect.PlayGoldAcquireEffect(_startPositionTransformOfEffect.position,
                AD_VIEW_GEM_AMOUNT); // ���� ����Ʈ ����
>>>>>>> Stashed changes
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += error =>
        {
            Debug.LogError("<color=red>Rewarded ad failed to open full screen content " +
                           "with error : " + error + "</color>");
            LoadRewardedAd();
        };
    }
}