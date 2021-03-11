/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///LevelManager.cs
///Developed by Charlie Bullock
///This class is responsible for managing the levels state (paused etc), win conditions and the main camera too
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //Variables
    #region variables
    public int gameState = 0;
    private GameObject[] enemies;
    private GameObject player;
    [SerializeField]
    private GameObject pauseButton;
    [SerializeField]
    private Image currentProjectileImage;
    [SerializeField]
    private GameObject[] shotsLeft;
    [SerializeField]
    private GameObject camera;
    [SerializeField]
    private GameObject remainingShots;
    private Vector3 startingCameraPosition;
    private LaunchProjectile lP;
    private Vector3 projectilePosition;
    [SerializeField]
    private float cameraMinimumX;
    [SerializeField]
    private float cameraMinimumY;
    [SerializeField]
    private float cameraMaximumX;
    [SerializeField]
    private float cameraMaximumY;
    [SerializeField]
    private Sprite[] projectiles;
    [SerializeField]
    private AudioClip loseSound;
    [SerializeField]
    private AudioClip winSound;
    [SerializeField]
    private AudioClip selectSound;
    [SerializeField]
    private GameObject timerObject;
    [SerializeField]
    private TextMeshProUGUI timerText;
    private AudioManager aM;
    private UiManager uM;
    private Vector3 touchstart;
    private float cameraDistance;
    private bool projectileShaking = false;
    public float timer;
    #endregion variables
    //Start function gets the enemy players and player along with setting up camera related stuff
    void Start()
    {
        timer = 500;
        aM = GameObject.FindObjectOfType<AudioManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        lP = GameObject.FindObjectOfType<LaunchProjectile>();
        uM = GameObject.FindObjectOfType<UiManager>();
        if (camera != null)
        {
            startingCameraPosition = camera.transform.position;
            pauseButton.GetComponent<Button>().onClick.AddListener(PauseUnpause);
        }
        else
        {
            Debug.LogError("Camera reference missing");
        }
    }

    //Update function manages the functionality of the levels different states
    void Update()
    {
        switch (gameState)
        {
            //Playing
            case 0:
                //Game timer
                timer -= Time.deltaTime;
                timerText.text = timer.ToString();
                //Save the current time as this levels highscore
                if (timer > PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "Time"))
                {
                    PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "Time", timer);
                }
                CursorPauseCheck();
                //Ensure orthographic size set back to 3 if not already
                if (camera.GetComponent<Camera>().orthographicSize != 3)
                {
                    camera.GetComponent<Camera>().orthographicSize = 3; 
                }
                //Projectile position set
                projectilePosition = lP.ProjectilePosition();
                //If mouse clicked or touched within 1.2f of the cannon then set touch start to the input position
                if (Input.GetMouseButtonDown(0) && GetDistance() > 1.2)
                {
                    touchstart = camera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
                }
                //If projectile position not 0 on all axis then set it to the camera position to the projectile position on X and Y axis
                if (projectilePosition != new Vector3 (0,0,0))
                {

                    camera.transform.position = new Vector3 (projectilePosition.x,projectilePosition.y,camera.transform.position.z);
                }          
                //Else if the mouse pressed and camera position within constraints then move the camera when the screen pressed
                else if (Input.GetMouseButton(0) && camera.transform.position.x > cameraMinimumX && camera.transform.position.y > cameraMinimumY && cameraMaximumX > camera.transform.position.x && cameraMaximumY > camera.transform.position.y)
                {

                    Vector3 direction = touchstart - camera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
                    camera.transform.position += direction;
                }
                //Else if the came position not at its starting position and projectileShaking now false then move the position back 
                else if (camera.transform.position != startingCameraPosition && projectileShaking == false)
                {

                    camera.transform.position = Vector3.MoveTowards(camera.transform.position, startingCameraPosition, 5 * Time.deltaTime);
                }
                CheckIfEmpty();
                break;
            //Paused
            case 1:
                //If cursore confined unlock it
                if (Cursor.lockState == CursorLockMode.Confined)
                {
                    Cursor.lockState = CursorLockMode.None;
                }
                //If scape is pressed change state
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    uM.SettingsAndHelp();
                    remainingShots.SetActive(true);
                    gameState = 0;
                    touchstart = Vector3.zero;
                }
                break;
            //Won
            case 2:
                pauseButton.SetActive(false);
                timerObject.SetActive(false);
                remainingShots.SetActive(false);
                player.transform.GetChild(0).gameObject.SetActive(false);
                uM.WonOrLost(false);
                break;
            //Lost
            case 3:
                pauseButton.SetActive(false);
                timerObject.SetActive(false);
                remainingShots.SetActive(false);
                player.transform.GetChild(0).gameObject.SetActive(false);
                uM.WonOrLost(true);
                break;
            //Firing
            case 4:
                //Zoom camera out when firing to see line renderer correctly
                cameraDistance = 3 + ((GetDistance() - 1.2f) * 0.55f);
                if (cameraDistance > 3f && cameraDistance < 10)
                {
                    camera.GetComponent<Camera>().orthographicSize = cameraDistance;
                }
                CursorPauseCheck();
                break;
            default:
                Debug.LogWarning("Wrong value!");
                break;
        }
    }

    //When called this coroutine function shakes the main camera on it's X and Y axis and destroys gameobject given in the projectile parameter
    public IEnumerator Shake(float time, float intensity,GameObject projectile)
    {
        projectileShaking = true;
        Vector3 startingPosition = camera.transform.position = new Vector3 (projectilePosition.x,projectilePosition.y,camera.transform.position.z);

        float timeElapsed = 0.0f;
        while (timeElapsed < time)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * intensity;
            float y = UnityEngine.Random.Range(-1f, 1f) * intensity;

            camera.transform.localPosition = new Vector3(projectile.transform.position.x + x, projectile.transform.position.y  + y, startingPosition.z);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        camera.transform.localPosition = startingPosition;
        Destroy(projectile);
        projectileShaking = false;
    }

    //Function gets the distance from the input/touch position and the player position
    public float GetDistance()
    {
        return Vector3.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), player.transform.position);
    }

    //This function checks if cusor locks or not and will change state accordingly
    private void CursorPauseCheck()
    {
        if (player.activeSelf == false || Cursor.lockState == CursorLockMode.None)
        {
            remainingShots.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            gameState = 0;
            touchstart = Vector3.zero;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uM.SettingsAndHelp();
            remainingShots.SetActive(false);
            gameState = 1;
        }
    }

    //This function hides the gameplay related to ui when the game is paused so that it doesn't become a distsctio
    private void PauseUnpause()
    {
        aM.PlayClip(selectSound);
        uM.SettingsAndHelp();
        if (gameState == 1)
        {
            remainingShots.SetActive(true);
            gameState = 0;
            touchstart = Vector3.zero;
        }
        else
        {
            remainingShots.SetActive(false);
            gameState = 1;
        }
    }
    //Function called when no more projectiles to launch to set to gameover state
    public void GameLost()
    {
        aM.PlayClip(loseSound);
        gameState = 3;
    }
    //Function called to set the game state to 0 as soon as projectile is being fired or 4 when not being fired
    public void NowFiring(bool yes)
    {
        if (yes)
        {
            gameState = 0;
        }
        else
        {
            gameState = 4;
        }
    }
    //This function checks how many shots remain
    public void RemainingShots(int remainingShot)
    {
        timer -= 25;
        for (int i = 0;i < shotsLeft.Length;i++)
        {
            if (i < remainingShot)
            {
                shotsLeft[i].SetActive(true);

                currentProjectileImage.sprite = projectiles[Math.Abs(shotsLeft.Length - remainingShot)];
            }
            else
            {
                shotsLeft[i].SetActive(false);
            }
        }
    }

    //This function loops through the enemy array in order to see if any remain
    private void CheckIfEmpty()
    {
        for (int i = 0;i < enemies.Length;i++)
        {
            if (enemies[i] != null)
            {
                return;
            }   
        }
        aM.PlayClip(winSound);
        gameState = 2;
    }
}
