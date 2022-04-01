using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Canvas transition;
    Animator transitionAnim;
    public GameObject player;
    public GameObject destination;

    // Start is called before the first frame update
    void Start()
    {
        transitionAnim = transition.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (destination.GetComponent<DestinationScript>().GetArrivalAmount() >=5)
        {
            transitionAnim.SetBool("win", true);
        }
        if (player.GetComponent<PlayerScript>().getHealth() <= 0)
        {
            transitionAnim.SetBool("lose", true);
        }
    }
}
