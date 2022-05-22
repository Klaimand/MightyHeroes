using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KLD_EndingScreen : MonoBehaviour
{
    KLD_MissionData data;

    [Header("Parameters")]
    [SerializeField] float timeBetweenFields = 1f;
    [SerializeField] float fieldAnimationTime = 0.8f;
    [SerializeField] float fieldAnimationRefreshTime = 0.02f;

    //references
    [Header("References")]
    [SerializeField] Animator animator;

    [Space(10)]
    [SerializeField] TMP_Text text_mapName;
    [SerializeField] TMP_Text text_gameMode;
    [SerializeField] TMP_Text text_difficulty;
    [SerializeField] TMP_Text text_remainingTime;
    [SerializeField] TMP_Text text_totalKill;

    [SerializeField] TMP_Text text_softCurrency;
    [SerializeField] TMP_Text text_hardCurrency;
    [SerializeField] TMP_Text text_energy;

    [Space(10)]
    [SerializeField] Transform missionFailedTransform;
    [SerializeField] Transform missionCompleteTransform;
    [SerializeField] Transform winSplashes;
    [SerializeField] Transform looseSplashes;


    //global vars
    int minutes;

    [ContextMenu("Initialize")]
    public void Initialize()
    {
        KLD_MissionInfos.instance.RefreshMissionInfos(true);

        data = KLD_MissionInfos.instance.missionData;

        if (data.succeeded)
        {
            missionCompleteTransform.gameObject.SetActive(true);
            if (XL_PlayerInfo.instance != null)
            {
                winSplashes.GetChild((int)XL_PlayerInfo.instance.menuData.character).gameObject.SetActive(true);
            }
        }
        else
        {
            missionFailedTransform.gameObject.SetActive(true);
            if (XL_PlayerInfo.instance != null)
            {
                looseSplashes.GetChild((int)XL_PlayerInfo.instance.menuData.character).gameObject.SetActive(true);
            }
        }


        text_mapName.text = XL_GameModeManager.instance.GetMapName();

        text_gameMode.text = XL_GameModeManager.instance.GetGamemodeName();

        text_difficulty.text = ((Difficulty)data.difficulty).ToString();

        text_remainingTime.text = FloatToTime(0f);

        text_totalKill.text = "000";


        text_softCurrency.text = "0000";

        text_hardCurrency.text = "000";

        text_energy.text = "000";

        animator.SetTrigger("showMenu");
    }

    [ContextMenu("ShowStats")]
    public void ShowStats()
    {
        StartCoroutine(ShowStatsCoroutine());
    }

    IEnumerator ShowStatsCoroutine()
    {
        StartCoroutine(DoTextAnimation(text_remainingTime, 0, data.remainingTime, fieldAnimationTime, fieldAnimationRefreshTime, "", true));
        yield return new WaitForSeconds(timeBetweenFields);

        StartCoroutine(DoTextAnimation(text_totalKill, 0, data.killedEnemies, fieldAnimationTime, fieldAnimationRefreshTime, "000", false));
        yield return new WaitForSeconds(timeBetweenFields);

        StartCoroutine(DoTextAnimation(text_softCurrency, 0, data.GetSoftCurrency(), fieldAnimationTime, fieldAnimationRefreshTime, "0000", false));
        yield return new WaitForSeconds(timeBetweenFields);

        StartCoroutine(DoTextAnimation(text_hardCurrency, 0, data.GetHardCurrency(), fieldAnimationTime, fieldAnimationRefreshTime, "000", false));
        yield return new WaitForSeconds(timeBetweenFields);

        StartCoroutine(DoTextAnimation(text_energy, 0, data.GetEnergy(), fieldAnimationTime, fieldAnimationRefreshTime, "000", false));
        yield return new WaitForSeconds(timeBetweenFields);
    }

    string FloatToTime(float _time)
    {
        minutes = (int)Mathf.Floor(_time / 60);

        return minutes.ToString("00") + ":" + ((_time - (60 * minutes)).ToString("00"));
    }

    float t;
    float r;
    IEnumerator DoTextAnimation(TMP_Text _text, int _startValue, int _endValue, float _time, float _refreshSpeed,
    string _format, bool _timeFormat)
    {
        t = 0;
        while (t < _time)
        {
            if (r > _refreshSpeed)
            {
                if (!_timeFormat)
                {
                    _text.text = Mathf.Lerp(_startValue, _endValue, t / _time).ToString(_format);
                }
                else
                {
                    _text.text = FloatToTime(Mathf.Lerp(_startValue, _endValue, t / _time));
                }
                r = 0f;
            }

            yield return null;

            r += Time.deltaTime;
            t += Time.deltaTime;
        }
    }



}
