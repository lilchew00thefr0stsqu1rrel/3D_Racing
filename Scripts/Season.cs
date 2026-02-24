using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;


    /// <summary>
    /// Определение эпизода как набора уровней. Уровни идут последовательно.
    /// </summary>
    [SerializeField]
    [CreateAssetMenu]
    public class Season : ScriptableObject
    {

        /// <summary>
        /// Название эпизода.
        /// </summary>
        [SerializeField] private string m_SeasonName;
        public string SeasonName => m_SeasonName;

        /// <summary>
        /// Список названий сцен. Последовательно.
        /// </summary>
        [SerializeField] private RaceInfo[] m_Races;
        public RaceInfo[] Races => m_Races;

        /// <summary>
        /// Превьюшка эпизода. Квадратная картинка например.
        /// </summary>
        [SerializeField] private Sprite m_PreviewImage;
        public Sprite PreviewImage => m_PreviewImage;

        /// <summary>
        /// Порядковый идентификатор
        /// </summary>
        
        [SerializeField] private int m_SeasonID;
        public int SeasonID => m_SeasonID;



        [SerializeField] private int m_BranchID;
        public int BranchID => m_BranchID;


        public void SetRaceScore(string sceneName, int score)
        {
            PlayerPrefs.SetInt(sceneName, score);
        }

        public void GetRaceScore(string sceneName, int score)
        {
            PlayerPrefs.SetInt(sceneName, score);
        }
    }
