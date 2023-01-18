using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private GameObject selectedObject;
    private GameObject selectedSocket;
    public bool isMove;
    [Header("----LEVEL SETTINGS")]
    [SerializeField] private GameObject[] collisionControlObjects;
    [SerializeField] private GameObject[] Plugs;
    [SerializeField] private int Targetsys;
    [SerializeField] private List<bool> collisionSituation;
    [SerializeField] private int RightofMove;


    private int completionsys;
    private int collisionControlSys;
    private LastPlug _lastPlug;

    [Header("----OTHER OBJECTS")]
    [SerializeField] private GameObject[] Lights;


    [Header("----UI OBJECTS")]
    [SerializeField] private GameObject controlPanel;
    [SerializeField] private TextMeshProUGUI controlText;
    [SerializeField] private TextMeshProUGUI RightofMoveText;


    void Start()
    {
        RightofMoveText.text ="MOVES : " + RightofMove.ToString();
        for (int i = 0; i < Targetsys-1; i++)
        {
            collisionSituation.Add(false);
        }
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
            //Debug.Log("All scokets in place");

            foreach (var itemm in collisionControlObjects)
            {
                itemm.SetActive(true);
            }

            StartCoroutine(CollisionVrm());
        }
        else
        {
            if (RightofMove<=0)
            {
                Debug.Log("Right of move is over");
            }

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
                            RightofMove--;
                            RightofMoveText.text = "MOVES : " + RightofMove.ToString();

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
    }

    IEnumerator CollisionVrm()
    {
        Lights[0].SetActive(false);
        Lights[1].SetActive(true);

        controlPanel.SetActive(true);
        controlText.text = "Being Checked...";

        
        yield return new WaitForSeconds(4f);

        foreach (var item in collisionSituation)
        {
            if (item)
            {
                collisionControlSys++;
            }

            if (collisionControlSys==collisionSituation.Count)
            {
                Lights[1].SetActive(false);
                Lights[2].SetActive(true);
                controlText.text = "WIN";
                // win panel

            }
            else
            {
                Lights[1].SetActive(false);
                Lights[0].SetActive(true);
                controlText.text = "Collision Detected";
                Invoke("CloseThePanel",2f);
                foreach (var itemm in collisionControlObjects)
                {
                    itemm.SetActive(false);
                }

                if (RightofMove<=0)
                {
                    Debug.Log("Right of move is over");
                }
            }
        }

        collisionControlSys = 0;
    }

    void CloseThePanel()
    {
        controlPanel.SetActive(false);
    }
}
