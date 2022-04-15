/**** 
 * Created by: Yao Wang
 * Date Created: 4/6/2022
 * 
 * Last Edited by: 
 * Last Edited: 4/8/2022
 * 
 * Description: projectile
****/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    /**Variables**/
    private BoundsCheck bndCheck; //ref to boundary

    // Start is called before the first frame update
    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bndCheck.offUp)
        {
            gameObject.SetActive(false); //set the projetile to deactivate and return to pool
            bndCheck.offUp = false; //reset the BoundsCheck offUp boolean
        }
    }
}
