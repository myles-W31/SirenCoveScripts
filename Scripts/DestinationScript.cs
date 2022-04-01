using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationScript : MonoBehaviour
{

    public GameObject manager;

    public GameObject player;
    Collider2D playerCollider;
    public GameObject mainCamera;

    BoxCollider2D destinationArea;
    public Vector2 teleportLocation; //where the player will be teleported

    float arrivalCounter;
    bool arrival;

    public AudioClip arrivalSound;
    public AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        playerCollider = player.GetComponent<Collider2D>();
        destinationArea = GetComponent<BoxCollider2D>();
        arrival = false;
        source = manager.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(arrivalCounter);
        if (destinationArea.bounds.Intersects(playerCollider.bounds))
        {
            player.GetComponent<PlayerScript>().ForceMove(teleportLocation);

            manager.GetComponent<ManagerScript>().GenerateLevels(arrivalCounter+1);
            manager.GetComponent<ManagerScript>().GenerateSirenCalls();
            manager.GetComponent<UIScript>().ResetSirenCallList();

            mainCamera.GetComponent<CameraScript>().ForceMove(teleportLocation);

            arrival = true;
        }
        if (arrival)
        {
            arrivalCounter++;
            source.PlayOneShot(arrivalSound);
            arrival = false;
        }
    }
    public float GetArrivalAmount()
    {
        return arrivalCounter;
    }
    public bool GetArrival()
    {
        return arrival;
    }
}
