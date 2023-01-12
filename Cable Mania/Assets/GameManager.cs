using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out RaycastHit hit,100))
            {
                if (hit.collider !=null)
                {
                    if (hit.collider.CompareTag("BluePlug") || hit.collider.CompareTag("YellowPlug") || hit.collider.CompareTag("RedPlug"))
                    {
                        hit.collider.GetComponent<LastPlug>().SelectionPositon(hit.collider.GetComponent<LastPlug>().availableSocket.GetComponent<Socket>().movePosition, hit.collider.GetComponent<LastPlug>().availableSocket);

                        
                    }
                }
            }
        }
    }
}
