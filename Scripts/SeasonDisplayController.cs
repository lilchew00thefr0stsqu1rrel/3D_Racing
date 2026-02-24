using SpaceShooter;
using System;
using UnityEngine;
using UnityEngine.Events;

    public class SeasonDisplayController : MonoBehaviour
    {
        [SerializeField] private UISeason[] seasons;

        [SerializeField] private Season firstSeason;
        
        void Start()
        {
            print("showing levels");
            var drawSeason = 0;
            var score = 1;
            while (score != 0 && drawSeason < seasons.Length)
            {

                score = seasons[drawSeason].Initialize();
            
                drawSeason += 1;



            }
            for (int i = drawSeason; i < seasons.Length; i++)
            {
                seasons[i].gameObject.SetActive(false);
            }

             
            

            
        }

        
       
    }



