using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public GameObject player;
    public GameObject buttonPrompt;

    public Sprite leftPrompt;
    public Sprite rightPrompt;
    int promptDirection;
    public Animator buttonPromptAnim;

    public List<GameObject> sirenCall;
    GameObject activeSirenCall;

    public Image healthSpriteOne;
    public Image healthSpriteTwo;
    public Image healthSpriteThree;

    public Scrollbar progressBar;
    float progressBarValue;
    float distanceToEnd;
    public GameObject destination;
    float startDistanceToEnd;

    // Start is called before the first frame update
    void Start()
    {
        sirenCall = new List<GameObject>();
        ResetSirenCallList();
        //buttonPrompt.SetActive(false);
        buttonPromptAnim = buttonPrompt.GetComponent<Animator>();
        startDistanceToEnd = ((player.transform.position - destination.transform.position).magnitude) / 100;
    }

    // Update is called once per frame
    void Update()
    {
        //----------button prompt----------//
        if (player.GetComponent<PlayerScript>().findPlayerControl() == false)
        {
            buttonPrompt.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 7, buttonPrompt.transform.position.z);
            buttonPromptAnim.SetBool("end", false);
            if (activeSirenCall == null)
            {
                FindActiveSirenCall();
            }
            ActivateButtonPrompt();
        }
        if (player.GetComponent<PlayerScript>().findPlayerControl())
        {
            activeSirenCall = null;
            buttonPromptAnim.SetBool("start", false);
            buttonPromptAnim.SetBool("end", true);
            //buttonPrompt.SetActive(false);
        }
        
        //---------------------------------//

        //----------player health----------//
        //Debug.Log(player.GetComponent<PlayerScript>().getHealth());
        switch (player.GetComponent<PlayerScript>().getHealth())
        {
            case 2:
                healthSpriteOne.GetComponent<Animator>().SetBool("start", true);
                break;
            case 1:
                healthSpriteTwo.GetComponent<Animator>().SetBool("start", true);
                break;
            case 0:
                healthSpriteThree.GetComponent<Animator>().SetBool("start", true);
                break;
            default:
                break;
        }
        //---------------------------------//

        //----------progress bar----------//
        distanceToEnd = ((player.transform.position - destination.transform.position).magnitude)/100;
        //Debug.Log(distanceToEnd);
        float scaledEnd = Mathf.InverseLerp(0,startDistanceToEnd*5,startDistanceToEnd-distanceToEnd);
        //Debug.Log(scaledEnd);
        scaledEnd += (destination.GetComponent<DestinationScript>().GetArrivalAmount()/5);
        progressBar.value = scaledEnd;
        //--------------------------------//

        //----------pause screen----------//
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GetComponent<CanvasManager>().ActivatePopUpCanvas(GetComponent<CanvasManager>().regCanvas, GetComponent<CanvasManager>().popUpCanvas);

        }
        //--------------------------------//
    }

    void FindActiveSirenCall()
    {
        //Debug.Log(player.GetComponent<PlayerScript>().findPlayerControl());
        foreach (var item in sirenCall)
        {
            //Debug.Log(item.name + item.GetComponent<SirenCallScript>().IsPlayerInfluenced());
            if (item.GetComponent<SirenCallScript>().IsPlayerInfluenced())
            {
                activeSirenCall = item;
            }
        }
    }

    void ActivateButtonPrompt()
    {
        promptDirection = activeSirenCall.GetComponent<SirenCallScript>().FindXLocation();
        if (promptDirection > 0)
        {
            buttonPrompt.GetComponent<SpriteRenderer>().sprite = leftPrompt;
        }
        else
        {
            buttonPrompt.GetComponent<SpriteRenderer>().sprite = rightPrompt;
        }

        //buttonPrompt.SetActive(true);
        buttonPromptAnim.SetBool("start", true);
    }

    public void ResetSirenCallList()
    {
        sirenCall.Clear();
        sirenCall.AddRange(GameObject.FindGameObjectsWithTag("sirenCall"));
    }
}
