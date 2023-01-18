using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionControl : MonoBehaviour
{
    public GameManager _GameManager;
    public int collisionIndex;
    void Start()
    {
        
    }

   
    void Update()
    {
        Collider[] hitColl = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity);

        for (int i = 0; i < hitColl.Length; i++)
        {
            if (hitColl[i].CompareTag("CablePiece"))
            {
                _GameManager.CollisionControl(collisionIndex,false);
            }
            else
            {
                _GameManager.CollisionControl(collisionIndex,true);

            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position,transform.localScale /2);
    }
}
