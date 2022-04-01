using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirenCallScript : MonoBehaviour
{
    public GameObject player;
    float radiusOfInfluence;

    public GameObject radiusSprite;

    bool playerInInfluence;

    int outInfluenceButton;
    int outofInfluenceAmount;

    int xLocationInWorld;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ChangeRadiusOfInfluence();
        if (transform.position.x >0)
        {
            xLocationInWorld = 1;
        }
        else
        {
            xLocationInWorld = -1;
        }
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 distance = transform.position - player.transform.position;
        if (distance.magnitude < radiusOfInfluence && outInfluenceButton < outofInfluenceAmount)
        {
            playerInInfluence = true;
        }
        if (playerInInfluence)
        {
            anim.SetBool("reset", false);
            if (distance.magnitude < 10)
            {
                anim.SetBool("caught", true);
                player.GetComponent<PlayerScript>().ForceMove(new Vector2(0, transform.position.y));
                playerInInfluence = false;
                outInfluenceButton = 100;
                player.GetComponent<PlayerScript>().ReduceHealth();
            }
            else
            {
                player.GetComponent<PlayerScript>().TogglePlayerControl(false);
                player.GetComponent<PlayerScript>().TurnAndMoveToward(gameObject);
            }

            if (distance.magnitude < 20)
            {
                anim.SetBool("closer", true);
            }

            switch (xLocationInWorld)
            {
                case 1:
                    if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        outInfluenceButton++;
                    }
                    break;
                case -1:
                    if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        outInfluenceButton++;
                    }
                    break;
                default:
                    break;
            }
            //Debug.Log("under influence");

            if (outInfluenceButton >= outofInfluenceAmount)
            {
                anim.SetBool("escaped", true);
                player.GetComponent<PlayerScript>().TogglePlayerControl(true);
                //radiusSprite.SetActive(false);
                playerInInfluence = false;
            }
        }
    }

    void ChangeRadiusOfInfluence()
    {
        radiusOfInfluence = Random.Range(30, 50);
        radiusSprite.transform.localScale = new Vector3(radiusOfInfluence * 2, radiusOfInfluence * 2, 0);
        outofInfluenceAmount = (int)radiusOfInfluence / 3;
    }

    public void ResetCall()
    {
        ChangeRadiusOfInfluence();
        playerInInfluence = false;
        //radiusSprite.SetActive(true);
        outInfluenceButton = 0;

        anim.SetBool("reset", true);
        anim.SetBool("closer", false);
        anim.SetBool("caught", false);
        anim.SetBool("escaped", false);
    }

    public bool IsPlayerInfluenced()
    {
        return playerInInfluence; //playeroutofinfluence determines wether or not the player has broken through the siren call influence yet
    }

    public int FindXLocation()
    {
        return xLocationInWorld;
    }
}
