using System.Collections;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using TMPro;

public class ScreenAdMob : MonoBehaviour
{
    private void Start()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // ���� �ʱ�ȭ�� �� ȣ��.
            Debug.Log("Google Mobile Ads SDK �ʱ�ȭ �Ϸ�");
            LoadInterstitialAd(); // SDK �ʱ�ȭ �� ���� �ε�
        });
    }

    //-----------[���� ����]------------
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-4956833096962057/6073199467";
#elif UNITY_IPHONE
      private string _adUnitId = "ca-app-pub-4956833096962057/6073199467";
#else
      private string _adUnitId = "unused";
#endif

    private InterstitialAd _interstitialAd;

    // ���� �ε�
    public void LoadInterstitialAd()
    {
        // ���� ���� ������ ����
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // ���� ��û ����
        AdRequest adRequest = new AdRequest();

        // ���� �ε� ��û
        InterstitialAd.Load(_adUnitId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Interstitial ad failed to load an ad with error: " + error);
                return;
            }

            Debug.Log("Interstitial ad loaded with response: " + ad.GetResponseInfo());
            _interstitialAd = ad;

            RegisterEventHandlers(_interstitialAd);
        });
    }

    // ���� ǥ��
    public void ShowInterstitialAd()
    {
        StartCoroutine(showInterstitial());

        IEnumerator showInterstitial()
        {
            while (_interstitialAd == null && !_interstitialAd.CanShowAd())
            {
                yield return new WaitForSeconds(0.2f);
            }
            _interstitialAd.Show();
        }
        /*if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            _interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }*/
    }

    private void RegisterReloadHandler(InterstitialAd interstitialAd)
    {
        // ���� ���� ��ü ȭ�� �������� �ݾ��� �� ȣ��˴ϴ�.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial Ad full screen content closed.");

            // ������ �� ���� �ٸ� ���� ������ �� �ֵ��� ���� �ٽ� �ε��մϴ�.
            LoadInterstitialAd();
        };

        // ���� ���� ��ü ȭ�� �������� ���� ������ �� ȣ��˴ϴ�.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                       "with error : " + error);

            // ������ �� ���� �ٸ� ���� ������ �� �ֵ��� ���� �ٽ� �ε��մϴ�.
            LoadInterstitialAd();
        };
    }

    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.", adValue.Value, adValue.CurrencyCode));
        };

        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };

        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };

        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };

        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            SceneManager.LoadScene("MainMenu"); // ������ �޴��� ���ư���
            LoadInterstitialAd(); // ���� ���� �� �� ���� �ε�
        };

        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content with error: " + error);
            LoadInterstitialAd(); // ���� ���� �� �� ���� �ε�
        };
    }

    // ���� ������ ���� ǥ�� 
    public void GameOver()
    {
        ShowInterstitialAd();
    }
}
