/**** 
 * Created by: Akram Taghavi-Burris
 * Date Created: March 16, 2022
 * 
 * Last Edited by: Yao Wang
 * Last Edited: 4/11/2022
 * 
 * Description: Hero
****/

/*** Using Namespaces ***/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase] //forces selection of parent object
public class Hero : MonoBehaviour
{
    /*** VARIABLES ***/

    #region PlayerShip Singleton
    static public Hero SHIP; //refence GameManager
   
    //Check to make sure only one gm of the GameManager is in the scene
    void CheckSHIPIsInScene()
    {

        //Check if instnace is null
        if (SHIP == null)
        {
            SHIP = this; //set SHIP to this game object
        }
        else //else if SHIP is not null send an error
        {
            Debug.LogError("Hero.Awake() - Attempeeted to assign second Hero.SHIP");
        }
    }//end CheckGameManagerIsInScene()
    #endregion

    GameManager gm; //reference to game manager

    ObjectPool pool; //reference to Object Pool

    [Header("Ship Movement")]
    public float speed = 10;
    public float rollMult = -45;
    public float pitchMult = 30;



    [Space(10)]

    [Header("Projectile Settings")]
    public float projectileSpeed = 40;
    public AudioClip projectileSound; //sound clip of projectile
    private AudioSource audioSource; //audio source component


    private GameObject lastTriggerGo; //reference to the last triggering game object
   
    [SerializeField] //show in inspector
    private float _shieldLevel = 1; //level for shields
    public int maxShield = 4; //maximum shield level
    
    //method that acts as a field (property), if the property falls below zero the game object is desotryed
    public float shieldLevel
    {
        get { return (_shieldLevel); }
        set
        {
            _shieldLevel = Mathf.Min(value, maxShield); //Min returns the smallest of the values, therby making max sheilds 4

            //if the sheild is going to be set to less than zero
            if (value < 0)
            {
                Destroy(this.gameObject);
                Debug.Log(gm.name);
                gm.LostLife();
                
            }

        }
    }

    /*** MEHTODS ***/

    //Awake is called when the game loads (before Start).  Awake only once during the lifetime of the script instance.
    void Awake()
    {
        CheckSHIPIsInScene(); //check for Hero SHIP
    }//end Awake()


    //Start is called once before the update
    private void Start()
    {
        gm = GameManager.GM; //find the game manager
        pool = ObjectPool.POOL; //find the game manager
        audioSource = GetComponent<AudioSource>();

    }//end Start()


   
    // Update is called once per frame (page 551)
        void Update()
    {
        //player input
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        //Change the transform based on the axis
        Vector3 pos = transform.position;

        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;

        transform.position = pos;

        //Rotate the shipt to make it feel more dynamic
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

        //Check for spacebar (fire)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireProjectile();
        }

    }//end Update()



    //Taking Damage
    private void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        //tranform root returens the topmost transfrom in the hierarchy (i.e. parent)

        GameObject go = rootT.gameObject; //game object of parent transform

        if (go == lastTriggerGo) return; //don't do anything if it's the same object we last collided with

        lastTriggerGo = go; //set the triger to the last trigger

        if (go.tag == "Enemy")
        {
            Debug.Log("Triggered by Enemy " + other.gameObject.name);
            shieldLevel--;
            Destroy(go);
        }
        else
        {
            Debug.Log("Triggered by non-Enemy " + other.gameObject.name);
        }
    }//end OnTriggerEnter()

    void FireProjectile()
    {
        GameObject projGo = pool.GetObject(); //Get object from pool

        //if there is a projectile object
        if (projGo != null)
        {
            /**Play projectile sound**/
            if(audioSource != null)
            {
                audioSource.PlayOneShot(projectileSound);
            }

            projGo.transform.position = transform.position;
            Rigidbody RB = projGo.GetComponent<Rigidbody>();
            RB.velocity = Vector3.up * projectileSpeed;
        }//end if-statement
    } //end FireProjectile()

}
