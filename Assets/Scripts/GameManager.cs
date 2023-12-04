using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    // list of what enemies on which rows
    [SerializeField] 
    private List<GameObject> enemyTypeOnRow;
    // amount of enemies per row
    [SerializeField] 
    private int amountPerRow;

    // list where we put the actual enemies
    private List<Enemy> enemies;


    // rect for gizmos
    [SerializeField] 
    private Rect enemyZone;

    /* ---------- moving swarm variables ---------- */
    [SerializeField] 
    private float interval = 1f;
    private float timer;

    [SerializeField] 
    private float moveAmount = 1f;
    private int waveDirection;

    /* ---------- win and lose screen variables ---------- */
    [SerializeField] 
    private GameObject winScreen;
    [SerializeField] 
    private GameObject lostScreen;

    // score
    private int score;

    // 
    [SerializeField]
    private UnityEvent onScoreUpdate;


    private void Start()
    {
        // set win and lose screen inactive
        winScreen.SetActive(false);
        lostScreen.SetActive(false);

        // call function create to create the enemies
        Create();
    }

    private void Update()
    {
        // add to timer
        timer += Time.deltaTime;

        // if timer reached goal update wave direction and apply wave direction
        if(timer >= interval)
        {
            // checks if the wave has to move to the right/left and down and updates the wave direction variable
            UpdateWaveDirection();
            // checks the wave direction variable and moves depending on
            ApplyWaveDirection();

            // set timer back to zero
            timer = 0;
        }
    }

    // draw gizmos
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(enemyZone.center, enemyZone.size);
    }

    // function create, creates all enemies
    private void Create()
    {
        // make new list of type <Enemy>
        enemies = new List<Enemy>();

        // set start position to top left of rect gizmos
		float startX = enemyZone.x;
        float startY = enemyZone.y + enemyZone.height;
        
        // nested for loop, for actually creating enemies
        for(int i = 0; i < enemyTypeOnRow.Count; i++)
        {
            for(int j = 0; j < amountPerRow; j++)
            {
                // creates enemy
                GameObject enemy = Instantiate(enemyTypeOnRow[i], new Vector3(startX + j,startY - i, 0), transform.rotation);
                // adds enemies enemy script to list
                enemies.Add(enemy.GetComponent<Enemy>());
            }
        }
    }

    // function to remove enemy. Takes as parameter the enemy to destroy
    public void RemoveEnemy(Enemy enemy)
    {
        // remove the enemy given in parameter out of the enemies list
        enemies.Remove(enemy);
        // destroy enemy given in parameter
        Destroy(enemy.gameObject);

        // if no enemies are left, activate the winscreen
        if(enemies.Count == 0)
        {
            winScreen.SetActive(true);
        }
    }

    /* ---------------- move enemies ---------------- */
    private void MoveEnemies(float x, float y, float z)
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.transform.Translate(x, y, z);  
        }
    }

    private void ApplyWaveDirection()
    {
        if (waveDirection == 0)
        {
            MoveEnemies(moveAmount, 0, 0);
        }
        else if (waveDirection == 1)
        {
            MoveEnemies(0, -moveAmount, 0);
        }
        else if (waveDirection == 2)
        {
            MoveEnemies(-moveAmount, 0, 0);
        }
        else if (waveDirection == 3)
        {
            MoveEnemies(0, -moveAmount, 0);
        }
    }

    private bool EnemyIsTouchingRightSide()
    {
        foreach(Enemy enemy in enemies)
        {
            if(enemy.transform.position.x >= enemyZone.x + enemyZone.width)
            {
                return true;
            }
        }
        return false;
    }

    private bool EnemyIsTouchingLeftSide()
    {
        foreach(Enemy enemy in enemies)
        {
            if(enemy.transform.position.x <= enemyZone.x)
            {
                return true;
            }
        }
        return false;
    }

    private void UpdateWaveDirection()
    {
        if(waveDirection == 0)
        {
            if(EnemyIsTouchingRightSide())
            {
                waveDirection = 1;
            }
        }
        else if(waveDirection == 1)
        {
            waveDirection = 2;
        }
        else if(waveDirection == 2)
        {
            if(EnemyIsTouchingLeftSide())
            {
                waveDirection = 3;
            }
        }
        else if(waveDirection == 3)
        {
            waveDirection = 0;
        }
    }

    /* ---------------- ---------------- ---------------- */

    // reset game
    public void ResetGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // lost screen
    public void ReportPlayerDeath()
    {
        lostScreen.SetActive(true);
    }

    // add and update score
    public void AddScore(int amount)
    {
        score += amount;
        onScoreUpdate.Invoke();
    }

    public int GetScore()
    {
        return score;
    }
}