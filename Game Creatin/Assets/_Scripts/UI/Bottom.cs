using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bottom : MonoBehaviour
{
    [SerializeField]
    private EnemyManager _manger;
    public void Restart()
    {
        StaticLevelManager.IsGameFlove = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
    public void StartGame()
    {
        if (!StaticLevelManager.IsGameFlove)
        {
            if (_manger.CheckHero())
            {
                StaticLevelManager.IsGameFlove = true;
                _manger.StartGame();
            }
            else
            {
                Debug.Log("not all heroes are on the map");
            }
        }
        else
        {
            Debug.Log("the game is running");
        }
    }
}
