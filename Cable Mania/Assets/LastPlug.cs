using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastPlug : MonoBehaviour
{ 
    public GameObject availableSocket;
    public string socketColor;
    [SerializeField] private GameManager _gameManager;

    private bool changePos,chosen,socketSit;

    private GameObject movePosition;
    private GameObject socketKnd;

    public void Move(string process, GameObject socket, GameObject gdlObject = null)
    {
        switch (process)
        {
            case "Selection":
                movePosition = gdlObject;
                chosen = true;
                break;
            case "ChangePosition":
                socketKnd = socket;
                movePosition = gdlObject;
                changePos = true;
                break;
            case "ReturnToSocket":
                socketKnd = socket;
                socketSit = true;
                break;
        }
    }
    
    void Update()
    {
        if (chosen)
        {
            transform.position = Vector3.Lerp(transform.position,movePosition.transform.position,.040f);
            if (Vector3.Distance(transform.position,movePosition.transform.position)<.010f)
            {
                chosen = false;
            }
        }

        if (changePos)
        {
            transform.position = Vector3.Lerp(transform.position, movePosition.transform.position, .040f);
            if (Vector3.Distance(transform.position, movePosition.transform.position) < .010f)
            {
               changePos = false;
               socketSit=true;
            }
        }

        if (socketSit)
        {
            transform.position = Vector3.Lerp(transform.position, socketKnd.transform.position, .040f);
            if (Vector3.Distance(transform.position, socketKnd.transform.position) < .010f)
            {
                
                socketSit = false;
                _gameManager.isMove = false;
                availableSocket = socketKnd;
                _gameManager.CheckPlug();
            }
        }
    }
}
