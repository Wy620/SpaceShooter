/**** 
 * Created by: Yao Wang
 * Date Created: 4/6/2022
 * 
 * Last Edited by: Yao Wang
 * Last Edited: 04/0//2022
 * 
 * Description: Create a pool of object for reuse
****/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    /**Variables**/
    static public ObjectPool POOL;

    #region POOL Singleton
    void CheckPoolIsInScene()
    {
        if(POOL == null)
        {
            POOL = this;
        }
        else
        {
            Debug.LogError("POOL.Awake() - Attempted to assign a second Object.POOL");
        }
    } //end CheckPoolIsInScene()
    #endregion

    private Queue<GameObject> projectiles = new Queue<GameObject>(); //new queue of projectiles

    [Header("POOL Settings")]
    public GameObject projectilePrefab; //projectile prefab
    public int poolStartSize = 5; //starting size of pool

    /**Methods**/
    private void Awake()
    {
        CheckPoolIsInScene();
    } //end Awake()

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < poolStartSize; i++)
        {
            GameObject projectileGO = Instantiate(projectilePrefab); //create prefab instance
            projectiles.Enqueue(projectileGO); //add to queue
            projectileGO.SetActive(false); //hide projectile
        } //end
    }//end Start()

    public GameObject GetObject()
    {
        //If there are objects in the pool
        if (projectiles.Count > 0)
        {
            GameObject gObject = projectiles.Dequeue(); //remove from queue
            gObject.SetActive(true); //enable
            return gObject; // return object
        }
        else
        {
            Debug.LogWarning("Out of objects, reloading...");
            return null; //returns null
        }//end
    }//end GetObject()

    public void ReturnObject(GameObject gObject)
    {
        projectiles.Enqueue(gObject); //add back to queue
        gObject.SetActive(false); //disable
    } //end ReturnObject()

    // Update is called once per frame
    void Update()
    {
        
    }
}
