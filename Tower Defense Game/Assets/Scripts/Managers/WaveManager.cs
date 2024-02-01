using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics.Tracing;

namespace TowerDefence
{
    public class WaveManager : MonoBehaviour
    {
        GameManager gameManager;
        SceneFader fader;
        Arrows arrow;

        [Header("Wave Information")]
        public static int EnemiesAlive = 0;
        public WaveSpawner[] waveSpawners;
        public Wave[] waves;
        [Space]
        public float countdown = 10;
        public float timeBetweenWaves = 5;
        [HideInInspector]
        public bool beginDownTimer;
        [HideInInspector]
        public int waveIndex = 0;
        [Space]
        public TextMeshProUGUI waveNumberText;
        public GameObject waveButton;
        
        // Boolean
        bool displayIncomingEnemies = true;

        void Start()
        {
            if (gameManager == null)
                gameManager = FindObjectOfType<GameManager>();
            if (waveButton == null)
                Debug.LogError("Missing waveButton reference!");
            fader = FindObjectOfType<SceneFader>();
            arrow = Arrows.instance;

            Button waveBut = waveButton.gameObject.GetComponent<Button>();
            waveBut.onClick.AddListener(StartWaveButton);

            EnemiesAlive = 0;
            waveNumberText.text = PlayerStats.Rounds.ToString() + " / " + waves.Length;
            GameManager.BeginGame = false;
            displayIncomingEnemies = true;
        }

        public void Update()
        {
            Debug.Log("wave "+waveIndex);

            Wave wave;
            try
            {

                wave = waves[waveIndex];
            }
            catch(System.Exception ex)
            {
                //Debug.LogError(ex.ToString());
                waveIndex = 0;
                wave = waves[waveIndex];
                return;
            }
            

            // Ensures the wave spawners display incoming enemies before game starts
            if (displayIncomingEnemies)
            {
                displayIncomingEnemies = false;
                DisplayEnemiesOnWaveSpawner(wave, 1);
            }

            if (GameManager.BeginGame == false)
                return;

            if (GameManager.IsPaused)
            {
                waveButton.SetActive(false);
                return;
            }
            
            BeginDownTimerForWaves(wave);
            
            if ((EnemiesAlive > 0 || AllEnemiesDeployedInWave(wave) == false && beginDownTimer || GameManager.GameIsOver) && waveIndex < 6)
            return;
           

            // if (!waveButton.activeInHierarchy)
            //waveButton.SetActive(true);

            if (countdown <= 0)
            {
                StartWave(wave);
                return;
            }

            // cooldown handler
            if (AllEnemiesDeployedInWave(wave))
            {
                beginDownTimer = false;
                waveIndex++;
                
            }

            DisplayEnemiesOnWaveSpawner(wave,0);

            countdown -= Time.deltaTime;
            countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

            // player has completed the level
            LevelCompleted();
        }

        private void DisplayEnemiesOnWaveSpawner(Wave wave, int count)
        {
            
            for (int i = 0; i < wave.enemyInfo.Length; i++)
            {
                for (int j = 0; j < waveSpawners.Length; j++)
                {
                    if (waveSpawners[j].route == wave.enemyInfo[i].route)
                        waveSpawners[j].DisplayEnemyIcon(wave.enemyInfo[i].enemyType);
                }
            }
            
        }

        private void BeginDownTimerForWaves(Wave wave)
        {
            if (beginDownTimer)
            {
                for (int i = 0; i < wave.enemyInfo.Length; i++)
                {
                    wave.enemyInfo[i].downTime -= Time.deltaTime;
                }
            }
        }

        private void LevelCompleted()
        {

            if (waveIndex.Equals(waves.Length) && EnemiesAlive <= 0)
                {
                Debug.Log("lvl");
                gameManager.LevelCompleted();
                enabled = false;
            }
        }

        private void StartWave(Wave wave)
        {
            // start wave
            PlayerStats.Rounds++;
            waveNumberText.text = PlayerStats.Rounds.ToString() + " / " + waves.Length;
            waveButton.SetActive(false);
            beginDownTimer = true;

            for (int i = 0; i < wave.enemyInfo.Length; i++)
            {
                EnemiesAlive += wave.enemyInfo[i].count;
            }

            for (int i = 0; i < waveSpawners.Length; i++)
                waveSpawners[i].CloseEnemyIcons();

            countdown = timeBetweenWaves;
        }

        public void StartWaveButton()
        {
            if (Time.timeScale.Equals(0))
                return;

            // start game
            GameManager.BeginGame = true;

            // stops waypoint arrows from running once game starts
           //// for (int i = 0; i < arrow.arrowScript.Count; i++)
           // {
               // arrow.arrowScript[i].stop = true;
           // }

            countdown = 0;
            waveButton.SetActive(false);
        }

        private bool AllEnemiesDeployedInWave(Wave wave)
        {
            for (int i = 0; i < wave.enemyInfo.Length; i++)
            {
                if (wave.enemyInfo[i].waveHasBegun == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//namespace TowerDefence
//{
//    public class WaveManager : MonoBehaviour
//    {
//        GameManager gameManager;
//        SceneFader fader;
//        Arrows arrow;

//        [Header("Wave Information")]
//        public static int EnemiesAlive = 0;
//        public WaveSpawner[] waveSpawners;
//        public Wave[] waves;
//        [Space]
//        public float countdown = 10;
//        public float timeBetweenWaves = 1;
//        [HideInInspector]
//        public bool beginDownTimer;
//        [HideInInspector]
//        public int waveIndex = 0;
//        [Space]
//        public TextMeshProUGUI waveNumberText;
//        public GameObject waveButton;

//        // Boolean
//        bool displayIncomingEnemies = true;

//        private bool mergedWavesStarted = false; // Added a flag to track if merged waves started

//        void Start()
//        {
//            if (gameManager == null)
//                gameManager = FindObjectOfType<GameManager>();
//            if (waveButton == null)
//                Debug.LogError("Missing waveButton reference!");
//            fader = FindObjectOfType<SceneFader>();
//            arrow = Arrows.instance;

//            Button waveBut = waveButton.gameObject.GetComponent<Button>();
//            waveBut.onClick.AddListener(StartWaveButton);

//            EnemiesAlive = 0;
//            waveNumberText.text = PlayerStats.Rounds.ToString() + " / " + waves.Length;
//            GameManager.BeginGame = false;
//            displayIncomingEnemies = true;
//        }

//        public void Update()
//        {
//            if (GameManager.BeginGame == false)
//                return;

//            if (GameManager.IsPaused)
//            {
//                waveButton.SetActive(false);
//                return;
//            }

//            Wave wave = waves[waveIndex];

//            if (waveIndex == 7) // Check for the 8th wave
//            {
//                if (!mergedWavesStarted)
//                {
//                    StartMergedWaves();
//                    mergedWavesStarted = true;
//                }
//            }
//            else
//            {
//                BeginDownTimerForWaves(wave);

//                if (EnemiesAlive > 0 && !AllEnemiesDeployedInWave(wave) && beginDownTimer && !GameManager.GameIsOver)
//                    return;

//                if (!waveButton.activeInHierarchy)
//                    waveButton.SetActive(true);

//                if (countdown <= 0)
//                {
//                    StartWave(wave);
//                    return;
//                }

//                // cooldown handler
//                if (AllEnemiesDeployedInWave(wave))
//                {
//                    beginDownTimer = false;
//                    waveIndex++;
//                }

//                DisplayEnemiesOnWaveSpawner(wave);

//                countdown -= Time.deltaTime;
//                countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

//                LevelCompleted();
//            }
//        }

//        private void StartMergedWaves()
//        {
//            // Configure the merged 8th, 9th, and 10th waves
//            Wave mergedWave = new Wave();
//            // Configure the enemies in the merged wave, e.g., mergedWave.enemyInfo = ...

//            // Start the merged wave
//            StartWave(mergedWave);
//        }

//        private void DisplayEnemiesOnWaveSpawner(Wave wave)
//        {
//            for (int i = 0; i < wave.enemyInfo.Length; i++)
//            {
//                for (int j = 0; j < waveSpawners.Length; j++)
//                {
//                    if (waveSpawners[j].route == wave.enemyInfo[i].route)
//                        waveSpawners[j].DisplayEnemyIcon(wave.enemyInfo[i].enemyType);
//                }
//            }
//        }

//        private void BeginDownTimerForWaves(Wave wave)
//        {
//            if (beginDownTimer)
//            {
//                for (int i = 0; i < wave.enemyInfo.Length; i++)
//                {
//                    wave.enemyInfo[i].downTime -= Time.deltaTime;
//                }
//            }
//        }

//        private void LevelCompleted()
//        {
//            if (waveIndex.Equals(waves.Length) && EnemiesAlive <= 0)
//            {
//                gameManager.LevelCompleted();
//                enabled = false;
//            }
//        }

//        private void StartWave(Wave wave)
//        {
//            // start wave
//            PlayerStats.Rounds++;
//            waveNumberText.text = PlayerStats.Rounds.ToString() + " / " + waves.Length;
//            waveButton.SetActive(false);
//            beginDownTimer = true;

//            for (int i = 0; i < wave.enemyInfo.Length; i++)
//            {
//                EnemiesAlive += wave.enemyInfo[i].count;
//            }

//            for (int i = 0; i < waveSpawners.Length; i++)
//                waveSpawners[i].CloseEnemyIcons();

//            countdown = timeBetweenWaves;
//        }

//        public void StartWaveButton()
//        {
//            if (Time.timeScale.Equals(0))
//                return;

//            // start game
//            GameManager.BeginGame = true;

//            // stops waypoint arrows from running once game starts
//            for (int i = 0; i < arrow.arrowScript.Count; i++)
//            {
//                arrow.arrowScript[i].stop = true;
//            }

//            countdown = 0;
//            waveButton.SetActive(false);
//        }

//        private bool AllEnemiesDeployedInWave(Wave wave)
//        {
//            for (int i = 0; i < wave.enemyInfo.Length; i++)
//            {
//                if (wave.enemyInfo[i].waveHasBegun == false)
//                {
//                    return false;
//                }
//            }

//            return true;
//        }
//    }
//}
