using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	public static PlayerController instance; 
	//creates an instance of the script other scripts can access
	//all variables used within the code
	public GameObject playerPrefab;
	float xInput = 0, yInput = 0;
	public float speed = 5;
	bool mouseLeft, canShoot;
	float lastShot = 0;
	public float timeBetweenShots = 0.25f;
	Vector3 mousePos, mouseVector;
	public Transform gunSprite, gunTip;
	public SpriteRenderer gunRend;
	int playerSortingOrder = 20;
	private Rigidbody2D rb;

	public GameObject bulletPrefab;
	CameraController Cam;


    private void Awake()
    {
        instance = this; 
		//creates the current script to be the instance that scripts can access
    }

    void Start()
	{
		GetMouseInput(); 
		//calls this function
		Cam = FindObjectOfType<CameraController>(); 
		//sets camera to the object that holds the scripts called CameraController
	}
	void Update()
	{

		GetInput(); 
		//capture wasd and mouse
		Movement(); 
		//move the player
		Animation(); 
		//rotate the gun
		Shooting(); 
		//handle shooting
		
	}
	void GetInput()
	{
		xInput = Input.GetAxis("Horizontal");
		yInput = Input.GetAxis("Vertical"); //capture wasd and arrow controls
		GetMouseInput();
	}
	void GetMouseInput()
	{
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
		//position of cursor in world
		mousePos.z = transform.position.z;
		//keep the z position consistant, since we're in 2d
		mouseVector = (mousePos - transform.position).normalized;
		//normalized vector from player pointing to cursor
		mouseLeft = Input.GetMouseButton(0); 
		//check left mouse button
	}
	void Movement()
	{
		Vector3 tempPos = transform.position;
		tempPos += new Vector3(xInput, yInput, 0) * speed * Time.deltaTime; //move the player based on input captures
		transform.position = tempPos;
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Void")
        {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //loads the next scene when trigger is activated
		}
		if (collision.gameObject.tag == "endVoid")
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
    }
    void Animation()
	{
		float gunAngle = -1 * Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg; //find angle in degrees from player to cursor
		gunSprite.rotation = Quaternion.AngleAxis(gunAngle, Vector3.back); //rotate gun sprite around that angle
		gunRend.sortingOrder = playerSortingOrder - 1; //put the gun sprite bellow the player sprite
		if (gunAngle > 0)
		{ 
			//put the gun on top of player if it's at the correct angle
			gunRend.sortingOrder = playerSortingOrder + 1;
		}
	}
	void Shooting()
	{
		canShoot = (lastShot + timeBetweenShots < Time.time);
		if (mouseLeft && canShoot)
		{
			//shoot if the mouse button is held and its been enough time since last shot
			Vector3 spawnPos = gunTip.position; 
			//position of the tip of the gun, a transform that is a child of rotating gun
			Quaternion spawnRot = Quaternion.identity; 
			//no rotation, bullets here are round
			Bullet bul = Instantiate(bulletPrefab, spawnPos, spawnRot).GetComponent<Bullet>();
			//spawn bullet and capture it's script
			bul.Setup(mouseVector); 
			//give the bullet a direction to fly
			lastShot = Time.time; 
			//used to check next time this is called
			Cam.Shake((transform.position - gunTip.position).normalized, 1.5f, 0.05f); 
			//call camera shake for recoil
		}
	}/*
  public IEnumerator Knockback(float knockbackDuration, float knockbackPower, Transform obj)
    {
		float timer = 0;

		while (knockbackDuration > timer)
        {
			timer += Time.deltaTime;
			Vector2 direction = (obj.transform.position = this.transform.position).normalized;
			rb.AddForce(-direction * knockbackPower);

        }
		yield return 0;
    }
    */
        
    

}
