using UnityEngine;
using UnityEngine.SceneManagement;

namespace Singletons
{
    public class PauseMenu : Singleton<PauseMenu>
    {
        #region Singleton

        /// <inheritdoc/>
        protected override bool DestroyOnLoad => true;

        /// <inheritdoc/>
        protected override void OnAwake() => this.Resume();

        #endregion
        
        #region Actions

        public void Start_MainMenu() => Start_Scene("MainMenuScreen");
        public void Start_ShootingRange() => Start_Scene("CannonTestScene");
        public void Start_Game() => Start_Scene("GameScene");

        private static void Start_Scene(string name)
        {
            PlayerPrefs.SetString("LevelToLoad", name);
            PlayerPrefs.Save();

            SceneManager.LoadScene("LoadingScreen");
        }

        public void Exit() => Application.Quit();

        #endregion
    
        #region Pause

        private CursorLockMode lockMode;
        private bool isCursorVisible;

        public void Resume()
        {
            Cursor.lockState = this.lockMode;
            Cursor.visible = this.isCursorVisible;
            Time.timeScale = 1;
            SetPaused(false);
        }

        public static void Pause()
        {
            if (!IsPaused())
            {
                // Save current state
                Instance.lockMode = Cursor.lockState;
                Instance.isCursorVisible = Cursor.visible;

                // Set state
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;
                SetPaused(true);
            }
            else 
            {
                Instance.Resume();
            }
        }
        
        private static void SetPaused(bool isPaused) => Instance.gameObject.SetActive(isPaused);
        public static bool IsPaused() => Instance != null && Instance.gameObject != null && Instance.gameObject.activeInHierarchy;

        #endregion
    }
}