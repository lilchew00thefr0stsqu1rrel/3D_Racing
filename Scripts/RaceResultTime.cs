using SpaceShooter;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class RaceResultTime : MonoBehaviour, IDependency<RaceTimeTracker>, IDependency<RaceStateTracker>, 
    IDependency<AllSeasonsCompletion>, IDependency<CurrentRaceAndSeason>
{
    public static string SaveMark = "_player_best_time";

    public event UnityAction ResultUpdated;

    [SerializeField] private float goldTime;
    public float GoldTime => goldTime;

    private float playerRecordTime;
    public float PlayerRecordTime => playerRecordTime;
    private float currentTime;
    public float CurrentTime => currentTime;
    public bool RecordWasSet => playerRecordTime != 0;

    private RaceTimeTracker raceTimeTracker;
    public void Construct(RaceTimeTracker obj) => raceTimeTracker = obj;

    private RaceStateTracker raceStateTracker;
    public void Construct(RaceStateTracker obj) => raceStateTracker = obj;

    private AllSeasonsCompletion allSeasonsCompletion;
    public void Construct(AllSeasonsCompletion obj) => allSeasonsCompletion = obj;
    private CurrentRaceAndSeason currentRaceAndSeason;
    public void Construct(CurrentRaceAndSeason obj) => currentRaceAndSeason = obj;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        Load();

        raceStateTracker.Completed += OnRaceCompleted;
    }

    private void OnDestroy()
    {
        raceStateTracker.Completed -= OnRaceCompleted;
    }

    private void OnRaceCompleted()
    {
        float absoluteRecord = GetAbsoluteRecord();

        if (raceTimeTracker.CurrentTime < absoluteRecord || playerRecordTime == 0)
        {
            playerRecordTime = raceTimeTracker.CurrentTime;

            Save();

        }

        currentTime = raceTimeTracker.CurrentTime;

        ResultUpdated?.Invoke();




    }

    public float GetAbsoluteRecord()
    {
        if (playerRecordTime < goldTime && playerRecordTime != 0)
        {
            return playerRecordTime;
        }
        else
        {
            return goldTime;
        }
    }

    

    private void Load()
    {
        playerRecordTime = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + SaveMark, 0);

     
        int score = PlayerPrefs.GetInt($"{currentRaceAndSeason.Race}");

      

        currentRaceAndSeason.SetScore(score);


        allSeasonsCompletion.Initialize();

    }

    private void Save()
    {
        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + SaveMark, playerRecordTime);

        int score = playerRecordTime < goldTime && playerRecordTime != 0 ? 1 : 0;

        PlayerPrefs.SetInt($"{currentRaceAndSeason.Race}", score);
        currentRaceAndSeason.SetScore(score);

        allSeasonsCompletion.Initialize();

        print("Ana:-Ba");
    }
}
