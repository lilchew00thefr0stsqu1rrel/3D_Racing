using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRaceResultPanel : MonoBehaviour, IDependency<RaceResultTime>
{
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private Text recordTime;
    [SerializeField] private Text currentTime;

    private RaceResultTime raceResultTime;
    public void Construct(RaceResultTime obj) => raceResultTime = obj;
    // Start is called before the first frame update
    void Start()
    {
        resultPanel.SetActive(false);

        raceResultTime.ResultUpdated += OnUpdateResults;
    }

    private void OnDestroy()
    {

        raceResultTime.ResultUpdated -= OnUpdateResults;
    }

    private void OnUpdateResults()
    {
        resultPanel.SetActive(true);

        recordTime.text = StringTime.SecondToTimeString(raceResultTime.GetAbsoluteRecord());
        currentTime.text = StringTime.SecondToTimeString(raceResultTime.CurrentTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
