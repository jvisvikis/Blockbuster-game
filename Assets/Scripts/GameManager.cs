using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{    
    public GameObject completeLevelUI;
    public GameObject gameOverUI;
    
    //public CheckpointManager CPManager;

    //Main Menu objects
    public GameObject controlPanel;
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject soundPanel;

    public static bool resetToCheckpointOrNot = false;

    public static bool gameHasEnded = false;

    void Start()
    {
        PlayerPrefs.GetFloat("MouseSens", 0.5f);
        PlayerPrefs.Save();

        //CPManager = CheckpointManager.Inst;
        
        // if (resetToCheckpointOrNot) 
        // {
        //     CPManager.ResetToCheckpoint();
        //     resetToCheckpointOrNot = false;
        // }
    }

    public void EndGame (){
        if(gameHasEnded == false){
            // GameObject.Find("BackgroundMusic").GetComponent<AudioSource>().volume = 0.15f;
            gameHasEnded = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            gameOverUI.SetActive(true);            
        }

    }
    public void Quit(){
        Application.Quit();
    }
    
    public void Restart (){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ScoreManager.ResetScores();
        // CheckpointManager.ResetCheckpoints();
        gameHasEnded = false;
    }

    // public void LoadCheckpoint(){
    //     Debug.Log("Attempted to reset to checkpoint");
    //     if(CPManager != null && CheckpointManager.activeCheckpoint != null){
    //         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //         resetToCheckpointOrNot = true;
    //     }
    //     gameHasEnded = false;
    // }

    public void CompleteLevel(){
        if (gameHasEnded)
            return;
        gameHasEnded = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        completeLevelUI.SetActive(true);

        PlayerStats.getInst().log();
        PlayerStats.getInst().reset();
    }

    public void Load1(){
        SceneManager.LoadScene(1);
    }

    public void ActivateControlPanel()
    {
        controlPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        soundPanel.SetActive(false);
    }

    public void ActivateMenuPanel()
    {
        controlPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        soundPanel.SetActive(false);
    }

    public void ActivateSettingsPanel()
    {
        controlPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
        soundPanel.SetActive(false);
    }

    public void ActivateSoundPanel()
    {
        controlPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        soundPanel.SetActive(true);
    }

    public void OpenWindow()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSeImuncEjWfrFVEE-joCW1IxG83RbtGpwTC4PQKUXh8vaY7aA/viewform?usp=sf_link");
    }

}
