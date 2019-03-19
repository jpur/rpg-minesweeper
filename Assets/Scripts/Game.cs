using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;

public class Game : MonoBehaviour {
    public static int Difficulty;

    public static bool IsGameOver;
    public static float SfxVolume = 1;

    public GameObject ClearedScreen;
    public GameObject GameOverScreen;

    private BannerView _bannerView;

    void Start() {
        Messenger.AddHandler(Message.GameOver, GameOver);
        Messenger.AddHandler(Message.AllTilesRevealed, (delegate {
                                                            if (!IsGameOver) {
                                                                GameCleared();
                                                            }
                                                        }));

        Difficulty = PlayerPrefs.HasKey("Difficulty") ? PlayerPrefs.GetInt("Difficulty") : 0;

        Board.Instance.Populate();
        LoadAd();
    }

    public void LoadAd() {
        // Create a 320x50 banner at the top of the screen.
        _bannerView = new BannerView("ca-app-pub-2741089983068744/7582605518", AdSize.Banner, AdPosition.Bottom);
        _bannerView.AdLoaded += (sender, args) => _bannerView.Show();
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the banner with the request.
        _bannerView.LoadAd(request);
        _bannerView.Show();
    }

    void GameCleared() {
        ClearedScreen.SetActive(true);
        Difficulty++;
    }

    public void GameOver() {
        Difficulty = Mathf.Max(0, Difficulty - 2);
        IsGameOver = true;
        foreach (Tile tile in Board.Instance.GetTiles()) {
            tile.Reveal(false);
        }
        GameOverScreen.SetActive(true);
    }

    public void Restart() {
        IsGameOver = false;
        Board.Instance.Populate();
        Messenger.SendMessage(Message.NewGame);
        ClearedScreen.SetActive(false);
        GameOverScreen.SetActive(false);
        Player.Instance.Reset();
    }

    public static void PlaySound(AudioClip clip) {
        GameObject go = new GameObject();
        AudioSource source = go.AddComponent<AudioSource>();
        source.loop = false;
        source.volume = SfxVolume;
        source.clip = clip;
        source.Play();
        go.AddComponent<DestroyAudioWhenDone>();
    }

    void OnApplicationQuit() {
        PlayerPrefs.SetInt("Difficulty", Difficulty);
        PlayerPrefs.Save();
    }
}
