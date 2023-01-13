using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject selectedObject;
    private GameObject selectedSocket;
    public bool isMove;

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
                { // ## Last Plug
                    if (selectedObject==null && !isMove)
                    {
                        if (hit.collider.CompareTag("BluePlug") || hit.collider.CompareTag("YellowPlug") || hit.collider.CompareTag("RedPlug"))
                        {
                            LastPlug _lastPlug = hit.collider.GetComponent<LastPlug>();
                            _lastPlug.SelectionPositon(_lastPlug.availableSocket.GetComponent<Socket>().movePosition, _lastPlug.availableSocket);
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
                            selectedObject.GetComponent<LastPlug>().ChangePositon(_socket.movePosition,hit.collider.gameObject);
                            _socket.full =true;

                            selectedObject = null;
                            selectedSocket =null;
                            isMove = true;
                        }
                        else if (selectedSocket == hit.collider.gameObject)
                        {
                            selectedObject.GetComponent<LastPlug>().ReturnToSocket(hit.collider.gameObject);

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
}
