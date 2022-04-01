using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    //controls the appearance of canvases (mainly for sceneMenus but also for game menu canvases)
    public List<Canvas> canvases;

    public Canvas regCanvas;
    public Canvas popUpCanvas; //for the settings and "back to menu" canvas in the game screen and the "are you sure" canvas for if the player quits the game

    //start menu specific
    public Canvas instructionsCanvas;
    public Canvas creditsCanvas;

    //transition screens specific
    public string sceneToTransitionTo;

    AudioSource source;
    public AudioClip beginningSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        canvases = new List<Canvas>();
        List<GameObject> temp = new List<GameObject>();
        temp.AddRange(GameObject.FindGameObjectsWithTag("canvas"));
        foreach (var item in temp)
        {
            canvases.Add(item.GetComponent<Canvas>());
        }

        ActivateCanvas(regCanvas);
        ToggleWaves(true);
        sceneToTransitionTo = "";

        if (beginningSoundEffect)
        {
            source.PlayOneShot(beginningSoundEffect);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySound(AudioClip clip)
    {
        AudioSource source = GetComponent<AudioSource>();
        if (source.isPlaying == false)
        {
            source.PlayOneShot(clip);
        }
    }

    public void ToggleWaves(bool waveBool)
    {
        List<GameObject> temp = new List<GameObject>();
        temp.AddRange(GameObject.FindGameObjectsWithTag("wave"));
        foreach (var item in temp)
        {
            item.GetComponent<Animator>().SetBool("start", waveBool);
        }

    }

    public void ActivateCanvas(Canvas canvas)
    {
        foreach (var item in canvases)
        {
            if (item.gameObject.GetComponent<Animator>())
            {
                //Debug.Log(item.name + " " + item.gameObject.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0));
                item.gameObject.GetComponent<Animator>().SetBool("start", false);
            }
            else
            {
                item.enabled = false;
            }
        }
        canvas.enabled = true;
        if (canvas.gameObject.GetComponent<Animator>())
        {
            canvas.gameObject.GetComponent<Animator>().SetBool("start",true);
        }
    }
    public void ActivatePopUpCanvas(Canvas canvas, Canvas popUp)
    {
        foreach (var item in canvases)
        {
            if (item.gameObject.GetComponent<Animator>())
            {
                Debug.Log(item.name + " " + item.gameObject.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0));
                item.gameObject.GetComponent<Animator>().SetBool("start", false);
            }
            else
            {
                item.enabled = false;
            }
        }
        canvas.enabled = true;
        popUp.enabled = true;
        if (popUp.gameObject.GetComponent<Animator>())
        {
            popUp.gameObject.GetComponent<Animator>().SetBool("start", true);
        }
    }

    public void ChangeUnityScene(string unityScene)
    {
        SceneManager.LoadScene(unityScene);
    }
    public void ChangeSceneViaAnimation(string unityScene)
    {
        Debug.Log(this.name);
        sceneToTransitionTo = unityScene;
        regCanvas.GetComponent<Animator>().SetBool("start", false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
             Application.Quit();
#endif
    }
}
