using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    //controls the camera
    public GameObject player;
    Vector3 playerPositionVector;
    Vector3 previousVector;

    bool teleport;

    // Start is called before the first frame update
    void Start()
    {
        playerPositionVector = player.transform.position;
        teleport = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (teleport == false)
        {
            if (player.transform.position.y < 155 && player.transform.position.y > -155)
            {
                playerPositionVector = player.transform.position;

                float yAmount = (playerPositionVector.y - previousVector.y) * 5;

                transform.Translate(Vector3.up * yAmount * Time.fixedDeltaTime);

                previousVector = transform.position;
            }
            else
            {
                Debug.Log("outOfBounds");
            }
        }
    }

    public void ForceMove(Vector2 location)
    {
        teleport = true;
        transform.position = new Vector3(transform.position.x, location.y, transform.position.z);
        teleport = false;
    }
}
