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
    public void SelectionPositon(GameObject gdlObject, GameObject socket)
    {
        movePosition =gdlObject;
        chosen = true;
    }

    public void ChangePositon(GameObject gdlObject, GameObject socket)
    {
        socketKnd = socket;
        movePosition = gdlObject;
        changePos = true;
    }

    public void ReturnToSocket( GameObject socket)
    {
        socketKnd = socket;
        socketSit = true;
    }
    void Start()
    {
        
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
