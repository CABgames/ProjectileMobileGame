/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///GameManager.cs
///Developed by Charlie Bullock
///This class manages aspects of the game from the main menu, primarilly ui related functionality
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Advertisements;

public class GameManager : MonoBehaviour
{
    //Variables
    #region Variables
    [SerializeField]
    private Button exitButton;
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button twitterButton;
    private AudioManager aM;
    [SerializeField]
    private AudioClip sound;
    [SerializeField]
    private TextMeshProUGUI scores;
    [SerializeField]
    private Button tweetButton;
    [SerializeField]
    private string[] scenes;
    #endregion Variables

    //Inside this start function the correct audio manager is assigned, button listeners are added for numerous aspects of the main menu and the text for scores is set up
    void Start()
    {
        aM = GameObject.FindObjectOfType<AudioManager>();
        exitButton.onClick.AddListener(Exit);
        playButton.onClick.AddListener(Play);
        tweetButton.onClick.AddListener(Twitter);
        twitterButton.onClick.AddListener(Twitter); 
        scores.text =
        "High score for level " + scenes[0] + " is: " + PlayerPrefs.GetFloat(scenes[0] + "Time") + " seconds," + "\n" +
        "high score for level " + scenes[1] + " is: " + PlayerPrefs.GetFloat(scenes[1] + "Time") + " seconds," + "\n" +
        "high score for level " + scenes[2] + " is: " + PlayerPrefs.GetFloat(scenes[2] + "Time") + " seconds," + "\n" +
        "high score for level " + scenes[3] + " is: " + PlayerPrefs.GetFloat(scenes[3] + "Time") + " seconds," + "\n" +
        "and high score for level " + scenes[4] + " is: " + PlayerPrefs.GetFloat(scenes[4] + "Time") + " seconds!";
    }

    //This function is called when the exit button is pressed and exits the application
    private void Exit()
    {
        aM.PlayClip(sound);
        Application.Quit();
    }

    //This function is called when the play button is pressed and loads a random level to play
    private void Play()
    {
        aM.PlayClip(sound);
        SceneManager.LoadScene(scenes[Random.Range(0, scenes.Length)]);
    }

    //In this function a potentil tweet for players to post is formed contining there score information over the course of the levels
    private void Twitter()
    {
        //Show the advert once
        Advertisement.Initialize("3927959", true);
        //Diplay video ad
        Advertisement.Show("video");
        aM.PlayClip(sound);
        Application.OpenURL("http://twitter.com/intent/tweet" +
        "?text=" + WWW.EscapeURL(
        "My score on Conquest Of Kingdoms " + scenes[0] + " level is: " + PlayerPrefs.GetFloat(scenes[0] + "Time") + " seconds," + "\n" +
        "my score for level " + scenes[1] + " is: " + PlayerPrefs.GetFloat(scenes[1] + "Time") + " seconds," + "\n" +
        "my score for level " + scenes[2] + " is: " + PlayerPrefs.GetFloat(scenes[2] + "Time") + " seconds," + "\n" +
        "my score for level " + scenes[3] + " is: " + PlayerPrefs.GetFloat(scenes[3] + "Time") + " seconds," + "\n" +
        "and my score for level " + scenes[4] + " is: " + PlayerPrefs.GetFloat(scenes[4] + "Time") + " seconds!"));
    }
}
