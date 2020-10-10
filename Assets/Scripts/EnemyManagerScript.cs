using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManagerScript : MonoBehaviour
{
    public int enemyCount = -1;

    private bool sceneEnded;

    private DialogManagerScript dialogManager;

    void Start()
    {
        //try { enemyCount = FindObjectsOfType<EnemyScript>().Length; }  catch { }

        dialogManager = GameObject.FindObjectOfType<DialogManagerScript>();

        sceneEnded = false;
    }

    void Update()
    {
        CheckEnemies();
    }

    void CheckEnemies()
    {
        if(!sceneEnded && enemyCount == 0)
        {
            sceneEnded = true;
            dialogManager.ShowDialog(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            },
            new Tuple<string, string>("David", "Floor cleared."));
        }
    }
}
