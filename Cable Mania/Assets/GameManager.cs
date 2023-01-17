using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject selectedObject;
    private GameObject selectedSocket;
    public bool isMove;
    [Header("----LEVEL SETTINGS")]
    public GameObject[] collisionControlObjects;
    public GameObject[] Plugs;
    public int Targetsys;
    public bool[] collisionSituation;
    private int completionsys;

    private LastPlug _lastPlug;
    void Start()
    {
        
    }

    public void CheckPlug()
    {
        foreach (var item in Plugs)
        {
            if (item.GetComponent<LastPlug>().availableSocket.name==item.GetComponent<LastPlug>().socketColor)
            {
                completionsys++;
            } 
        }

        if (completionsys==Targetsys)
        {
            Debug.Log("All scokets in place");

            foreach (var item in collisionControlObjects)
            {
                item.SetActive(true);
            }

        }
        else
        {
            Debug.Log("not completed");
        }

        completionsys =0;
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out RaycastHit hit,100))
            {
                if (hit.collider !=null)
                { // ## Last Plug
                    if (selectedObject==null && !isMove)
                    {
                        if (hit.collider.CompareTag("BluePlug") || hit.collider.CompareTag("YellowPlug") || hit.collider.CompareTag("RedPlug"))
                        {
                             _lastPlug = hit.collider.GetComponent<LastPlug>();
                             _lastPlug.Move("Selection", _lastPlug.availableSocket, _lastPlug.availableSocket.GetComponent<Socket>().movePosition);

                             selectedObject=hit.collider.gameObject;
                            selectedSocket=_lastPlug.availableSocket;
                            isMove = true;

                        }
                    }
                    // ## Last Plug

                    // ## Socket

                    if (hit.collider.CompareTag("Socket"))
                    {
                        if (selectedObject!=null && !hit.collider.GetComponent<Socket>().full && selectedSocket!=hit.collider.gameObject)
                        {
                            selectedSocket.GetComponent<Socket>().full = false;
                            Socket _socket = hit.collider.GetComponent<Socket>();


                            _lastPlug.Move("ChangePosition", hit.collider.gameObject, _socket.movePosition);
                            _socket.full =true;

                            selectedObject = null;
                            selectedSocket =null;
                            isMove = true;
                        }
                        else if (selectedSocket == hit.collider.gameObject)
                        {
                            _lastPlug.Move("ReturnToSocket", hit.collider.gameObject);

                            selectedObject = null;
                            selectedSocket = null;
                            isMove = true;
                        }
                          
                        
                    }

                    // ## Socket
                }
            }
        }
    }

    public void CollisionControl(int collisonIndex,bool situation)
    {
        collisionSituation[collisonIndex]= situation;

        if (collisionSituation[0] && collisionSituation[1])
        {
            Debug.Log("Win");
        }
        else
        {
            Debug.Log("Collision Detected");

        }
    }
}
