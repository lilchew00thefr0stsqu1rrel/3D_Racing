using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIRaceResult : MonoBehaviour, IDependency<RaceStateTracker>, IDependency<RaceTimeTracker>, IDependency<RaceResultTime>
{
    private RaceStateTracker raceStateTracker;
    public void Construct(RaceStateTracker obj) => raceStateTracker = obj;

    private RaceTimeTracker raceTimeTracker;
    public void Construct(RaceTimeTracker obj) => raceTimeTracker = obj;

    private RaceResultTime raceResultTime;
    public void Construct(RaceResultTime obj) => raceResultTime = obj;

    [SerializeField] private Text recordText;
    [SerializeField] private Text currentTimeText;

    [SerializeField] private GameObject raceResultUI;

    private void Start()
    {

        raceStateTracker.Completed += OnRaceCompleted;
        raceResultUI.SetActive(false);
    }

    private void OnDestroy()
    {
        raceStateTracker.Completed -= OnRaceCompleted;
    }

    private void OnRestart()
    {
        raceResultUI.SetActive(false);
    }


    private void OnRaceCompleted()
    {
        raceResultUI.SetActive(true);
        recordText.text = StringTime.SecondToTimeString(raceResultTime.GetAbsoluteRecord());
        currentTimeText.text = StringTime.SecondToTimeString(raceTimeTracker.CurrentTime);
    }

  
}
