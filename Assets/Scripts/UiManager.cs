/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///UiManager.cs
///Developed by Charlie Bullock
///This class manages the majority of user interface aspects of the game levels
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using TMPro;

public class UiManager : MonoBehaviour
{
    //Variables
    #region Variables
    [SerializeField]
    private Button winMenuButton;
    [SerializeField]
    private Button loseMenuButton;
    [SerializeField]
    private Button pauseMenuButton;
    [SerializeField]
    private Button retryButton;
    [SerializeField]
    private Button continueButton;
    [SerializeField]
    private GameObject winScreen;
    [SerializeField]
    private GameObject loseScreen;
    [SerializeField]
    private GameObject helpScreen;
    [SerializeField]
    private GameObject settings;
    [SerializeField]
    private GameObject help;
    [SerializeField]
    private Button switchButton;
    [SerializeField]
    private AudioClip selectSound;
    [SerializeField]
    private GameObject musicObject;
    private AudioManager aM;
    private bool settingsActive = true;
    [SerializeField]
    private Button tweetButton;
    [SerializeField]
    private string[] scenes;
    #endregion Variables
    
    //In this start function button listeners are set up for ui butons along with setting the various screens to false
    void Start()
    {
        aM = GameObject.FindObjectOfType<AudioManager>();
        tweetButton.onClick.AddListener(TweetScore);
        winMenuButton.onClick.AddListener(ReturnToMenu);
        loseMenuButton.onClick.AddListener(ReturnToMenu);
        pauseMenuButton.onClick.AddListener(ReturnToMenu);
        retryButton.onClick.AddListener(RestartLevel);
        continueButton.onClick.AddListener(NextLevel);
        switchButton.onClick.AddListener(SettingsOrHelp);
        loseScreen.SetActive(false);
        winScreen.SetActive(false);
        helpScreen.SetActive(false);
    }

    //When this fuction is called it will set the requested win or lose screen active whilst setting the alternative screen to false
    public void WonOrLost(bool hasLost)
    {
        if (winScreen.activeSelf == false && loseScreen.activeSelf == false)
        {
            //Lose
            if (hasLost)
            {
                loseScreen.SetActive(true);
                winScreen.SetActive(false);
            }
            //Win
            else
            {
                loseScreen.SetActive(false);
                winScreen.SetActive(true);

            }

            //Integrated Unity advertisement will play if on supported platform
            //if (Advertisement.isSupported)
            //{
            //    //Initialise the ads with this ID for Android
            //    Advertisement.Initialize("3927959", true);
            //    //Diplay video ad
            //    Advertisement.Show("video");
            //}
            DisplayAdvert();
        }
    }

    private void DisplayAdvert()
    {
        //Show the advert once
        Advertisement.Initialize("3927959", true);
        //Diplay video ad
        Advertisement.Show("video");
    }

    //This function when called will open twitter in the players browser of choice and attempt to tweet the scores this player achieved in the game
    public void TweetScore()
    {
        DisplayAdvert();
        //Twitter url to open and the string data wishing to be contained in the tweet
        Application.OpenURL("http://twitter.com/intent/tweet" +
           "?text=" + WWW.EscapeURL(
           "My score on Conquest Of Kingdoms " + SceneManager.GetActiveScene().name + " level is: " + PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "Time") + " seconds!"));
    }

    //This function will turn the help screen off or on depending on the current state of the gameobject, regardless of it's state the win and lose screens will be set inactive
    public void SettingsAndHelp()
    {
        //Screen currently active
        if (helpScreen.activeSelf)
        {
            helpScreen.SetActive(false);
            loseScreen.SetActive(false);
            winScreen.SetActive(false);
        }
        //Screen not currently active
        else
        {
            helpScreen.SetActive(true);
            loseScreen.SetActive(false);
            winScreen.SetActive(false);
        }
        SettingsOrHelp();
    }

    //Function used for the mute music toggle to mute or unmute the games music
    public void MuteMusic(bool muted)
    {
        aM.PlayClip(selectSound);
        musicObject.GetComponent<AudioSource>().enabled = !muted;
    }

    //Function used for the mute sounds toggle to mute or unmute the games sounds
    public void MuteSounds(bool muted)
    {
        aM.PlayClip(selectSound);
        aM.soundMuted = muted;
    }

    //Function used for the vibration toggle to enable or disable the vibration on mobile devices
    public void StopVibration(bool vibrating)
    {
        aM.PlayClip(selectSound);
        aM.vibrationEnabled = vibrating;
    }

    //Function for the menu buttons which upon being clicked will return the player to the main menu
    private void ReturnToMenu()
    {
        aM.PlayClip(selectSound);
        SceneManager.LoadScene("Menu");
    }

    //This function is called when a button is clicked to restart the level and will reload the current level essentially restarting it
    private void RestartLevel()
    {
        aM.PlayClip(selectSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //This function is called when the next level button is pressed and will pick on of the games levels randomly to proceed into next
    private void NextLevel()
    {
        aM.PlayClip(selectSound);
        SceneManager.LoadScene(scenes[Random.Range(0,scenes.Length)]);
    }

    //When called this function displays either the help aspects of the pause menu or the setting aspects
    private void SettingsOrHelp()
    {
        aM.PlayClip(selectSound);
        if (settingsActive)
        {
            settings.SetActive(true);
            help.SetActive(false);
            settingsActive = false;
            switchButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Help";
        }
        else
        {
            help.SetActive(true);
            settings.SetActive(false);
            settingsActive = true;
            switchButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Settings";
        }
    }

}
