using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIRaceStateHint : MonoBehaviour, IDependency<RaceStateTracker>
{
    private RaceStateTracker raceStateTracker;
    public void Construct(RaceStateTracker obj) => raceStateTracker = obj;

    [SerializeField] private GameObject newGameHint;
    [SerializeField] private GameObject startHint;
    [SerializeField] private GameObject controlHint;
    [SerializeField] private GameObject goalHint;

    private void Start()
    {
        raceStateTracker.PreparationStarted += OnPreparationStarted;
        raceStateTracker.Started += OnRaceStarted;
        raceStateTracker.Completed += OnRaceCompleted;

        startHint.SetActive(true);
        controlHint.SetActive(true);
        goalHint.SetActive(true);
        newGameHint.SetActive(false);
    }

    private void OnDestroy()
    {

        raceStateTracker.PreparationStarted -= OnPreparationStarted;
        raceStateTracker.Started -= OnRaceStarted;
        raceStateTracker.Completed -= OnRaceCompleted;
    }

    private void OnPreparationStarted()
    {
        startHint.SetActive(false);
    }
    private void OnRaceStarted()
    {
        controlHint.SetActive(false);
    }
    private void OnRaceCompleted()
    {
        startHint.SetActive(false);
        newGameHint?.SetActive(true);
        controlHint.SetActive(true);
        goalHint.SetActive(true);
    }
    private void OnNewGame()
    {
        newGameHint?.SetActive(false);
        startHint.SetActive(true);
    }


}
