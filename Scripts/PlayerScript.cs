using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //move player forward and left/right when needed

    public float speed;
    Vector2 velocity;
    Vector2 acceleration;
    public float rotationAmount;
    float frictionAmount;
    Vector2 friction;

    //bounds
    float boundsAmount;
    Vector2 bounds;

    //toggle player control
    bool playerControl;
    float controlledSpeed;

    //siren calls
    List<GameObject> sirenCalls;

    //health
    int health;

    //island
    public List<Collider2D> islands; //for physics
    public Collider2D closestIsland;

    //sources and sounds
    public AudioSource effectsSource;
    public AudioSource selfSource;
    public AudioClip impact;
    public AudioClip moveSound;
    public float volume;

    // Start is called before the first frame update
    void Start()
    {
        //speed = speed / 20;
        //rotationAmount = rotationAmount * Time.deltaTime;
        frictionAmount = speed / 75;
        boundsAmount = speed;
        playerControl = true;
        controlledSpeed = speed / 2;

        sirenCalls = new List<GameObject>();
        sirenCalls.AddRange(GameObject.FindGameObjectsWithTag("sirenCall"));

        health = 3;

        //get islands for physics
        //islands = new List<Collider2D>();
        //ResetIslandList();

        //sources
        selfSource = GetComponent<AudioSource>();
        effectsSource = GameObject.FindGameObjectWithTag("manager").GetComponent<AudioSource>();
        selfSource.volume = 0.2f;
    }

    void Update()
    {

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerControl == true)
        {
            acceleration = Vector2.zero;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                acceleration += (Vector2)transform.up;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                acceleration += (Vector2)transform.up * -1;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) //left
            {
                transform.Rotate(0, 0, rotationAmount);
                //acceleration += (Vector2)transform.right * rotationAmount * -1;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) //right
            {
                transform.Rotate(0, 0, -rotationAmount);
                //acceleration += (Vector2)transform.right * rotationAmount;
            }

            //friction
            if (velocity.magnitude >= 0.09)
            {
                friction += velocity * -1;
                friction = Vector2.ClampMagnitude(friction, frictionAmount);
            }
            else
            {
                velocity = Vector2.zero;
            }

            //move sounds
            if (selfSource.isPlaying == false)
            {
                selfSource.PlayOneShot(moveSound);
            }

            //bounds
            if (transform.position.x <= -30 || transform.position.x >= 30 || transform.position.y <= -160)
            {
                bounds += velocity*-1;
                bounds = Vector2.ClampMagnitude(bounds, boundsAmount);
                velocity += bounds;
            }

            acceleration = acceleration.normalized;
            velocity += acceleration;
            velocity += friction;

            velocity = Vector2.ClampMagnitude(velocity, speed);

            transform.position = transform.position + (Vector3)velocity * Time.fixedDeltaTime;
        }
        else
        {
            //selfSource.volume = .01f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerControl)
        {
            effectsSource.PlayOneShot(impact);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        //Debug.Log(angle);//between 110 and 65 it is side by side to the island
        //Debug.Log(Mathf.Abs(angle)); //returns absolute value (in case the angle float is negative somehow)
        if (playerControl)
        {
            Vector2 distance = transform.position - collision.gameObject.transform.position;
            float angle = Vector2.Angle(transform.up, distance);
            bounds += distance;
            bounds = Vector2.ClampMagnitude(bounds, boundsAmount);
            velocity += bounds;
        }
        else
        {
            //Debug.Log("reduce health");
        }
    }

    void FindClosestIsland()
    {
        
        float minDistance = float.MaxValue;
        foreach (var item in islands)
        {
            float distance = (item.gameObject.transform.position - transform.position).magnitude;
            //Debug.Log("checking closest island " + item.name + " distance: " + distance);
            if (distance < minDistance)
            {
                closestIsland = item;
                minDistance = distance;
            }
        }
        //Debug.Log("Closest island: " + closestIsland);
    }

    public void ForceMove(Vector2 location)
    {
        transform.position = location;
    }
    public void TogglePlayerControl(bool control)
    {
        playerControl = control;
    }
    public bool findPlayerControl()
    {
        return playerControl;
    }
    public void TurnAndMoveToward(GameObject area) //turns player and moved them towards areas outside of player control
    {
        transform.position = Vector2.MoveTowards(transform.position, area.transform.position, (speed/3) * Time.deltaTime);

        var offset = -90f;
        Vector2 direction = area.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));


        Debug.DrawLine(transform.position, transform.position + transform.up, Color.red);
    }

    public int getHealth()
    {
        return health;
    }
    public void ReduceHealth()
    {
        health--;
    }

    public void ResetIslandList()
    {
        islands.Clear();
        List<GameObject> temp = new List<GameObject>();
        temp.AddRange(GameObject.FindGameObjectsWithTag("island"));
        foreach (var item in temp)
        {
            islands.Add(item.GetComponent<Collider2D>());
        }
    }
}
