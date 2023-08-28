using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
public class WavesGameMode : GameModeManager
{
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] int _maxDeaths = 4;
    int _currentDeaths;

    [Header("Win/Lose Window Attributes")]
    [SerializeField] GameObject _window, _contentWindow;
    [SerializeField] TextMeshProUGUI _coinsWinnedTxt, _currentBetoCoinsTxt, _levelsReachedTxt, _winLoseTxt;
    public override void Start()
    {
        base.Start();
        _currentDeaths = Helpers.GameManager.SaveDataManager.GetInt("WavesCurrentDeaths", 0);
        _text.text = (_maxDeaths - _currentDeaths).ToString();

        EventManager.SubscribeToEvent(Contains.LOSE_WAVESGAME, LoseWindow);
    }
    private void OnDisable()
    {
        EventManager.UnSubscribeToEvent(Contains.LOSE_WAVESGAME, LoseWindow);
    }
    public override void PlayerDead()
    {
        playerHealth.EffectsOnDeath();
        playerHealth.RestartPosition();

        _currentDeaths++;
        _text.text = (_maxDeaths - _currentDeaths).ToString();
        Helpers.GameManager.SaveDataManager.SaveInt("WavesCurrentDeaths", _currentDeaths);

        if (_currentDeaths >= _maxDeaths)
            EventManager.TriggerEvent(Contains.LOSE_WAVESGAME);
    }

    public void RestartMinigame()
    {
        _currentDeaths = 0;
        Helpers.GameManager.SaveDataManager.SaveInt("WavesCurrentDeaths", _currentDeaths);
        Helpers.GameManager.LoadSceneManager.LoadLevel("MiniGame 1 0");
    }

    #region WIN-LOSE_UIWINDOW

    void LoseWindow(params object[] param)
    {
        _window.SetActive(true);
        _contentWindow.transform.localScale = Vector3.one * .5f;
        _winLoseTxt.text = "LOSE";
        _winLoseTxt.color = Color.red;

        float currentCoins = Helpers.PersistantData.persistantDataSaved.coins;
        float levelsReached = 0f;
        float coinsWinned = 0f;

        _currentBetoCoinsTxt.text = currentCoins.ToString();

        _contentWindow.transform.DOScale(Vector3.one, 1f).SetEase(Ease.InOutElastic)
                                          .OnComplete(() => DOTween.To(() => coinsWinned, x => coinsWinned = x, PlayerPrefs.GetInt("Coins"), .5f).OnUpdate(() => _coinsWinnedTxt.text = ((int)coinsWinned).ToString())
                                          .OnComplete(() => DOTween.To(() => levelsReached, x => levelsReached = x, PlayerPrefs.GetInt("LevelsWinned"), .5f).OnUpdate(() => _levelsReachedTxt.text = ((int)levelsReached).ToString())
                                          .OnComplete(() => _coinsWinnedTxt.transform.DOLocalMove(_currentBetoCoinsTxt.transform.position, .5f)
                                          .OnComplete(() => DOTween.To(() => currentCoins, x => currentCoins = (int)x, currentCoins + coinsWinned, 1f).OnUpdate(() => _currentBetoCoinsTxt.text = currentCoins.ToString())))));

       //OnComplete(() => DOTween.To(() => coinsWinned, x => coinsWinned = x, PlayerPrefs.GetInt("Coins"), .5f).OnUpdate(() => _coinsWinnedTxt.text = ((int)coinsWinned).ToString()).
       //OnComplete(() => DOTween.To(() => levelsReached, x => levelsReached = x, PlayerPrefs.GetInt("LevelsWinned"), .5f).OnUpdate(() => _levelsReachedTxt.text = ((int)levelsReached).ToString()).
       //OnComplete(() => _coinsWinnedTxt.transform.DOLocalMove(_currentBetoCoinsTxt.transform.position, .5f).
       //OnComplete(() => DOTween.To(() => currentCoins, x => currentCoins = (int)x, currentCoins + coinsWinned, 1f).OnUpdate(() => _currentBetoCoinsTxt.text = currentCoins.ToString())))));

                                          //_contentWindow.transform.DOScale(Vector3.one, 1f).SetEase(Ease.InOutElastic)
                                          //                                                 .OnComplete(() =>
                                          //                                                 {
                                          //                                                     DOTween.To(() => coinsWinned, x => coinsWinned = x, PlayerPrefs.GetInt("Coins"), .5f).OnUpdate(() => _coinsWinnedTxt.text = ((int)coinsWinned).ToString())
                                          //                                                     .OnComplete(() => DOTween.To(() => levelsReached, x => levelsReached = x, PlayerPrefs.GetInt("LevelsWinned"), .5f).OnUpdate(delegate { Debug.Log("AJAJ"); _levelsReachedTxt.text = ((int)levelsReached).ToString(); }))
                                          //                                                     .OnComplete(() => _coinsWinnedTxt.transform.DOLocalMove(_currentBetoCoinsTxt.transform.position, .5f))
                                          //                                                     .OnComplete(() => DOTween.To(() => currentCoins, x => currentCoins = (int)x, currentCoins + coinsWinned, 1f).OnUpdate(() => _currentBetoCoinsTxt.text = currentCoins.ToString()));
                                          //
                                          //                                                 });
                                          //Debug.Log($"LevelsWinned {levelsReached}");
                                          //.OnStepComplete(() => DOTween.To(() => levelsReached, x => levelsReached = (int)x, PlayerPrefs.GetInt("LevelsWinned"), .5f).OnUpdate(() => _levelsReachedTxt.text = levelsReached.ToString()))
                                          //.OnStepComplete(() => _coinsWinnedTxt.transform.DOLocalMove(_currentBetoCoinsTxt.transform.position, .5f))
                                          //.OnStepComplete(() => DOTween.To(() => currentCoins, x => currentCoins = (int)x, currentCoins + coinsWinned, 1f).OnUpdate(() => _currentBetoCoinsTxt.text = currentCoins.ToString()));
    }

void WinWindow()
{
    _winLoseTxt.text = "WIN";
    _winLoseTxt.color = Color.green;
    Helpers.PersistantData.persistantDataSaved.coins += PlayerPrefs.GetInt("Coins");
}

    #endregion

}
