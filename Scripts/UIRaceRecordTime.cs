using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRaceRecordTime : MonoBehaviour, IDependency<RaceResultTime>, IDependency<RaceStateTracker>
{
    [SerializeField] private GameObject goldRecordObject;
    [SerializeField] private GameObject playerRecordObject;
    [SerializeField] private Text goldRecordTime;
    [SerializeField] private Text playerRecordTime;

    private RaceResultTime raceResultTime;
    public void Construct(RaceResultTime obj) => raceResultTime = obj;

    private RaceStateTracker raceStateTracker;
    public void Construct(RaceStateTracker obj) => raceStateTracker = obj;
    // Start is called before the first frame update
    void Start()
    {
        raceStateTracker.Started += OnRaceStart;
        raceStateTracker.Completed += OnRaceCompleted;

        goldRecordObject.SetActive(false);
        playerRecordObject.SetActive(false);
    }

  

    private void OnRaceStart()
    {
        if (raceResultTime.PlayerRecordTime > raceResultTime.GoldTime || raceResultTime.RecordWasSet == false)
        {
            goldRecordObject.SetActive(true);
            goldRecordTime.text = StringTime.SecondToTimeString(raceResultTime.GoldTime);
        }

        if (raceResultTime.RecordWasSet == true)
        {
            playerRecordObject.SetActive(true);
            playerRecordTime.text = StringTime.SecondToTimeString(raceResultTime.PlayerRecordTime);
        }
    }
    private void OnRaceCompleted()
    {

    }
    private void OnDestroy()
    {
        raceStateTracker.Started -= OnRaceStart;
        raceStateTracker.Completed -= OnRaceCompleted;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
