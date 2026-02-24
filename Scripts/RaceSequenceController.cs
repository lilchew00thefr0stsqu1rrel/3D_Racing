using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceShooter
{
    /// <summary>
    /// Контроллер переходов между уровнями. Должен быть с пометкой DoNotDetroyOnLoad
    /// И лежать в сцене с главным меню. LevelController дернет завершение уровня.
    /// </summary>
    public class RaceSequenceController : MonoBehaviour, IDependency<RaceResultTime>
    {
        public static string MainMenuSceneNickname = "MainMenu";

        /// <summary>
        /// Текущий эпизод. Выставляется контроллером выбора эпизода перед началом игры.
        /// </summary>
        public Season CurrentSeason { get; private set; }

        /// <summary>
        /// Текущий уровень эпизода. Идшник относительно текущего выставленного эпизода.
        /// </summary>
        public int CurrentRace { get; private set; }

        /// <summary>
        /// Конструктор, реализующий интерфейс зависимости от RaceResultTime
        /// </summary>
        private RaceResultTime raceResultTime;
        public void Construct(RaceResultTime obj) => raceResultTime = obj;

        /// <summary>
        /// Метод запуска первого уровня эпизода.
        /// </summary>
        /// <param name="e"></param>
        public void StartEpisode(Season e)
        {
            CurrentSeason = e;
            CurrentRace = 0;

            // сбрасываем статы перед началом эпизода.
            RaceResultController.ResetPlayerStats();
            print($"Level load {e.SeasonID}");
            // запускаем первый уровень эпизода.
            SceneManager.LoadScene(e.Races[CurrentRace].SceneName);
        }

        /// <summary>
        /// Принудительный рестарт уровня.
        /// </summary>
        //public void RestartLevel()
        //{
        //    //SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
        //    SceneManager.LoadScene(LevelController.Instance.LevelData.SceneName);
        //}

        /// <summary>
        /// Завершаем уровень. В зависимости от результата будет показано окошко результатов.
        /// </summary>
        /// <param name="success">успешность или поражение</param>
        //public void FinishCurrentLevel(bool success)
        //{
        //    // после организации переходов
        //    LevelResultController.Instance.Show(success);
        //}

        /// <summary>
        /// Запускаем следующий уровень или выходим в главное меню если больше уровней нету.
        /// </summary>
        public void AdvanceRace()
        {
            CurrentRace++;

            // конец эпизода вываливаемся в главное меню.
            if(CurrentSeason.Races.Length <= CurrentRace)
            {
                SceneManager.LoadScene(MainMenuSceneNickname);
            }
            else
            {
                SceneManager.LoadScene(CurrentSeason.Races[CurrentRace].SceneName);
            }
        }

        #region Ship select

        /// <summary>
        /// Выбранный игроком корабль для прохождения.
        /// </summary>
        //public static SpaceShip PlayerShipPrefab { get; set; }

        #endregion
    }
}