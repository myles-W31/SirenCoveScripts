using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerScript : MonoBehaviour
{
    //Generates the look of the islands and the different siren Icons and calls
    //also generates levels from a pre-rendered list

    public List<GameObject> islands;
    public List<Sprite> islandTextures;

    public List<GameObject> icons;
    public List<Sprite> iconTextures;

    public List<GameObject> sirenCalls;

    public List<GameObject> gameLevels;
    public List<GameObject> rawLevels;

    // Start is called before the first frame update
    void Start()
    {
        islands = new List<GameObject>();
        islands.AddRange(GameObject.FindGameObjectsWithTag("island"));

        icons = new List<GameObject>();
        icons.AddRange(GameObject.FindGameObjectsWithTag("icon"));

        sirenCalls = new List<GameObject>();
        sirenCalls.AddRange(GameObject.FindGameObjectsWithTag("sirenCall"));

        gameLevels = new List<GameObject>();
        gameLevels.AddRange(GameObject.FindGameObjectsWithTag("level"));
        RandomizeLevels();

        rawLevels = new List<GameObject>();
        rawLevels.AddRange(GameObject.FindGameObjectsWithTag("level"));

        GenerateLevels(0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void RandomizeLevels()
    {
        for (int i = 0; i < gameLevels.Count; i++)
        {
            GameObject temp = gameLevels[i];
            int randomIndex = Random.Range(i, gameLevels.Count);
            gameLevels[i] = gameLevels[randomIndex];
            gameLevels[randomIndex] = temp;
        }
    }

    public void GenerateLevels(float destinationCounter)
    {
        foreach (var item in gameLevels)
        {
            item.SetActive(false);
        }
        if (destinationCounter <= gameLevels.Count)
        {
            gameLevels[(int)destinationCounter].SetActive(true);
        }
        else
        {
            int rng = Random.Range(0, gameLevels.Count - 1);
            gameLevels[rng].SetActive(true);
            Debug.Log(destinationCounter + " is not in the range of between 0 and " + gameLevels.Count);
        }
        GenerateIcons();
        GenerateIslands();
    }

    public void GenerateIslands()
    {
        //generate texture for each island
        GenerateTexture(islands, islandTextures);
        //generate rotation for each island
        foreach (var item in islands)
        {
            int random = Random.Range(0, 180);
            item.transform.rotation = Quaternion.identity;
            item.transform.Rotate(0, 0, random);
        }
    }

    public void GenerateIcons()
    {
        GenerateTexture(icons, iconTextures);
    }

    void GenerateTexture(List<GameObject> objects, List<Sprite> textures)
    {
        foreach (var item in objects)
        {
            int random = Random.Range(0, textures.Count);
            item.GetComponent<SpriteRenderer>().sprite = textures[random];
        }
    }

    public void GenerateSirenCalls()
    {
        foreach (var item in sirenCalls)
        {
            item.GetComponent<SirenCallScript>().ResetCall();
        }
    }
}
