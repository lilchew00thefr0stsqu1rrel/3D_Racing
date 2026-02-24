
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SpaceShooter
{
    /// <summary>
    /// Интерфейс условия прохождения уровня.
    /// </summary>
    public interface ILevelCondition
    {
        /// <summary>
        /// True если условие выполнено.
        /// </summary>
        bool IsCompleted { get; }
    }

    public class RaceController : MonoBehaviour
    {
        private const string MainMenuSceneName = "main_menu";


        /// <summary>
        /// Время прохождения в секундах за которое будут начисляться очки.
        /// </summary>
        [SerializeField] protected float m_ReferenceTime;
        public float ReferenceTime => m_ReferenceTime;

        /// <summary>
        /// Событие которое будет вызвано когда уровень будет выполнен. Вызывается один раз.
        /// </summary>
        [SerializeField] protected UnityEvent m_EventLevelCompleted;


        public event UnityAction LevelPassed;
        public event UnityAction LevelLost;

        [SerializeField] private RaceProperties m_RaceProperties;

        //[SerializeField] private LevelCondition[] m_Conditions;
        /// <summary>
        /// Массив условий для успешного прохождения уровня.
        /// </summary>
        private ILevelCondition[] m_Conditions;

        public RaceProperties LevelData => m_RaceProperties;

        private bool m_IsLevelCompleted;
        private float m_LevelTime;

        public bool HasNextLevel => m_RaceProperties.NextLevel != null;
        public float LevelTime => m_LevelTime;

        public bool IsLost;



       
        

        protected void Start()
        {
            Time.timeScale = 1;
            m_LevelTime = 0;
            m_Conditions = GetComponentsInChildren<ILevelCondition>();

        }

        private void Update()
        {
            if (m_IsLevelCompleted == false)
            {
                m_LevelTime += Time.deltaTime;
                CheckLevelConditions();
            }

           
        }

        private void CheckLevelConditions()
        {
            if (m_Conditions == null || m_Conditions.Length == 0)
                return;

            int numCompleted = 0;

            for (int i = 0; i < m_Conditions.Length; i++)
            {
                if (m_Conditions[i].IsCompleted == true)
                {
                    numCompleted++;
                }
            }

            if (numCompleted == m_Conditions.Length)
            {
                m_IsLevelCompleted = true;
                m_EventLevelCompleted?.Invoke();

                // Notify level sequence Unit3 code
                // RaceSequenceController.Instance?.FinishCurrentLevel(true);
            }
        }

        private void Lose()
        {
            LevelLost?.Invoke();
            //Time.timeScale = 0;
            IsLost = true;
        }

        private void Pass()
        {
            LevelPassed?.Invoke();
            //Time.timeScale = 0;
        }

        public void LoadNextLevel()
        {
            if (HasNextLevel == true)
                SceneManager.LoadScene(m_RaceProperties.NextLevel.SceneName);
            
                
            else
            {
                SceneManager.LoadScene(MainMenuSceneName);
            }

            IsLost = false;
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(m_RaceProperties.SceneName);

            // Player.Instance.ResetShipMaxHP();
            IsLost = false;
        }


        //
        public void FinishLevel(bool ef)
        {
            //if (ef) Pass() ; else Lose();
        }

        ///
        public void Show(bool success)
        {
           // if (success) Pass(); else Lose();
        }
    }

}
