using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
    [SerializeField] private TextMeshProUGUI[] UItexts;


    private int completionsys;
    private int collisionControlSys;
    private LastPlug _lastPlug;

    [Header("----OTHER OBJECTS")]
    [SerializeField] private GameObject[] Lights;
    [SerializeField] private AudioSource PlugSound;


    [Header("----UI OBJECTS")]
    [SerializeField] private GameObject controlPanel;
    [SerializeField] private TextMeshProUGUI controlText;
    [SerializeField] private GameObject[] GnlPanels;
    [SerializeField] private TextMeshProUGUI RightofMoveText;


    void Start()
    {
        RightofMoveText.text ="MOVES : " + RightofMove.ToString();
        for (int i = 0; i < Targetsys-1; i++)
        {
            collisionSituation.Add(false);
        }

        UItexts[3].text = PlayerPrefs.GetInt("Money").ToString();
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
                Lost();
            }

            
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
                              PlugSound.Play();
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
                //Lights[1].SetActive(false);
                //Lights[2].SetActive(true);
                //controlText.text = "WIN";
                
                Win();

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
                    Lost();
                }
            }
        }

        collisionControlSys = 0;
    }

    void CloseThePanel()
    {
        controlPanel.SetActive(false);
    }

    public void PlayPlugSound()
    {
        PlugSound.Play();
    }

    public void ButtonOperations(string Valuee)
    {
        switch (Valuee)
        {
            case "Pause": 
                GnlPanels[0].SetActive(true);
                Time.timeScale = 0;
                break;
            case "Resume":
                GnlPanels[0].SetActive(false);
                Time.timeScale = 1;
                break;
            case "TryAgain":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                Time.timeScale = 1;
                break;
            case "NextLevel":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                Time.timeScale = 1;
                break;
            case "Settings":
                // optional
            break;
            case "Exit":
                Application.Quit();
                break;
                

        }
    }

    void Win()
    {
        Lights[1].SetActive(false);
        Lights[2].SetActive(true);
        PlayerPrefs.SetInt("Level",PlayerPrefs.GetInt("Level")+1);
        UItexts[0].text="LEVEL : " +SceneManager.GetActiveScene().name;
        controlText.text = "YOU WIN";

        int randomMoney = Random.Range(5, 20);
        PlayerPrefs.SetInt("Money",PlayerPrefs.GetInt("Money")+randomMoney);
        UItexts[2].text = "MONEY : " + randomMoney;
        GnlPanels[1].SetActive(true);
        Time.timeScale = 0;

    }

    void Lost()
    {
        UItexts[1].text = "LEVEL : " + SceneManager.GetActiveScene().name;
        GnlPanels[2].SetActive(true);
        Time.timeScale = 0;
    }
}
