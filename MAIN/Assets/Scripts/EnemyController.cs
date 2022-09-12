using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum EnemyState
{
    idle,
    walk,
    attack,
    dead
} // all the different states that the enemy can be in

public class EnemyController : MonoBehaviour
{
    public EnemyState currentState;
    //public FloatValue maxHealth;
    public float health;
    public string enemyName;
    public float baseAttack;
    public float moveSpeed;

    public Transform target;
    private GameObject boss;
    public float chaseRadius;
    public float attackRadius;

    public static int numberOfEnemies;

    public float knockbackPower = 100;
    public float knockbackDuration = 1;

    private static int scoreValue;




    private void Awake()
    {
      //  health = maxHealth.initialValue;
    }
    
    void Start()
    {
        
        target = GameObject.FindWithTag("Player").transform;
        // sets the target at the location of the player
    }


    private GameObject[] _floorTiles;
    private GameObject _randomValidFloorTile;
    public GameObject voidPrefab;
    
    void Update()
{
        
        if (health <= 0)
        {
            Destroy(gameObject);
            if (numberOfEnemies == 0)
                // to spawn the void after all the enemies are confirmed dead
            {
                _floorTiles = GameObject.FindGameObjectsWithTag("floorTile");
                _randomValidFloorTile = _floorTiles[Random.Range(0, _floorTiles.Length)];
                Instantiate(voidPrefab, new Vector3(_randomValidFloorTile.transform.position.x, _randomValidFloorTile.transform.position.y, 0f), Quaternion.identity);
                //This will take the player to the next level.

            }
        
        }
        

        
    }
    private void OnEnable()
    {
        numberOfEnemies++;
        // will increment the number of enemies each time they are instantiated
    }
    public int enemiesKilled;
    private void OnDisable()
    {
        GameController.scoreValue += 10;
        // adds 10 to the score each time an enemy is killed
        numberOfEnemies--;
        // will decrement the count
    }
    static public int maxEnemies;
    /*
     static public void GameOver()
    {
        maxEnemies = GameController.scoreValue;
        if (maxEnemies <= 0)
        {
            Debug.Log("You didn't kill any enemy");
        }
        else
        {
            PlayFabManager PlayFabManager = GameObject.Find("PlayerFabManager")
                .GetComponent<PlayFabManager>();

            PlayFabManager.SendLeaderboard(maxEnemies);
        }
    }
    */
    void FixedUpdate()
    {
        CheckDistance();
        // will check the distance between the enemy and the player
    }
    void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            if(currentState == EnemyState.idle || currentState == EnemyState.walk)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                ChangeState(EnemyState.walk);
            }
            // will change the enemy state to walk if the player is close enough
            
        }
        if(Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) < attackRadius)
        {
            currentState = EnemyState.attack;
            // if the player is within attack range then the enemy will attack
        }
    }
    private void ChangeState(EnemyState newState)
    {
        if(currentState != newState)
        {
            currentState = newState;
        }
    }

  

    private void OnCollisionEnter2D(Collision2D attack)
    {
        if(attack.gameObject.tag == "Player")
        {
            attack.gameObject.GetComponent<Health>().playHealth -= baseAttack;
            // will decrease the player health each time the enemy attacks them       
        }
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(PlayerController.instance.Knockback(knockbackDuration, knockbackPower, this.transform));
        }
    }
    */

    public void Knock(Rigidbody2D myRigidbody, float knockTime)
    {
        StartCoroutine(KnockCo(myRigidbody, knockTime));
    }
    private IEnumerator KnockCo(Rigidbody2D myRigidbody, float knockTime)
    {
        if (myRigidbody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = EnemyState.idle;
        }
    }
    // void hit(int damage)
    //  {
    //  health -= damage;
    //     if(health <= 0)
    //     {
    //          Destroy(gameObject);
    //        }

    //   }


}
