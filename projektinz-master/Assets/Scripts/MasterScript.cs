using Assets.Scripts.XMLToGameObjectParser;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XMlParser;
using System.Linq;
using LZWPlib;
public class MasterScript : MonoBehaviour
{
    public GameObject UI;
    public List<GameObject> RingUI;
    public List<GameObject> SpaceUI;
    public static MasterScript master;
    public List<GameObject> Puzzles = new List<GameObject>();
    public List<GameObject> SmallObjects = new List<GameObject>();
    public List<Vector3> placements = new List<Vector3>();
    public List<AudioClip> AudioItems = new List<AudioClip>();
    public float puzzleSize;
    private Scene scene;
    public GameObject horizon;
    public GameObject player;
    public GameObject horizonInstance;
    public Camera left, right;
    public enum Mode
    {
        Tunnel,
        OpenSpace
    }
    public Mode environment = Mode.Tunnel;
    
    void loadXml()
    {
        scene = XmlLoader.LoadGameObjectsFromFile("sample2.xml");
    }
    void parseToGameObjects()
    {
        XMLToGameObjectParser.XMLToGameObjects(scene, ref Puzzles, ref placements, ref AudioItems,
                                                ref SmallObjects);
    }

    void initializeGameObjects()
    {
        if (environment == Mode.Tunnel)
        {
            GameObject firstPuzzle = (GameObject)Network.Instantiate(nextPuzzle(), new Vector3(0, 0, 0), new Quaternion(), 1);
            placements.Add(firstPuzzle.transform.position);
            GameObject back = (GameObject)Network.Instantiate(nextPuzzle(), firstPuzzle.transform.position, firstPuzzle.transform.rotation, 1);
            back.transform.Rotate(0, 180, 0);
            back.transform.Translate(0, 0, puzzleSize);
            placements.Add(back.transform.position);
        }
        else
            horizonInstance = (GameObject)Network.Instantiate(horizon, new Vector3(0, 0, 0), new Quaternion(), 1);

    }
    public GameObject nextPuzzle()
    {
        System.Random rnd = new System.Random();
        int index = rnd.Next(Puzzles.Count);
        return Puzzles.ElementAt(index);
    }
    public GameObject tunnel;
    // Use this for initialization
    void Start()
    {
        if (!LzwpManager.Instance.isServer)
            UI.SetActive(false);
        master = this.GetComponent<MasterScript>();
        loadXml();
        
            if (LzwpManager.Instance.isServer)
        {
            
            parseToGameObjects();
            puzzleSize = scene.PuzzleSize;
            if (scene.Type == "Tunnel")
                environment = Mode.Tunnel;
            else
                environment = Mode.OpenSpace;
            initializeGameObjects();
            
            Dropdown dropdown = GameObject.FindGameObjectWithTag("drop").GetComponent<Dropdown>();
            dropdown.ClearOptions();
            List<string> options = new List<string>();
            if (scene.GroupCount > 20)
                scene.GroupCount = 20;
            for (int i = 0; i <= scene.GroupCount; i++)
                options.Add(i.ToString());
            if (environment == Mode.OpenSpace)
            {
                foreach (GameObject a in RingUI)
                    a.SetActive(false);
                foreach (GameObject a in SpaceUI)
                    a.SetActive(true);
            }
            else
            {
                foreach (GameObject a in RingUI)
                    a.SetActive(true);
                foreach (GameObject a in SpaceUI)
                    a.SetActive(false);
            }
            dropdown.AddOptions(options);
            if (environment == Mode.OpenSpace)
            {
                transform.position = new Vector3(0, 1, 0) ;
                left.clearFlags = CameraClearFlags.Skybox;
                right.clearFlags = CameraClearFlags.Skybox;
                left.farClipPlane = 50000;
                right.farClipPlane = 50000;
                FPSConrtoller controller = player.GetComponent<FPSConrtoller>();
                controller.m_GravityMultiplier = 0;
                player.transform.Translate(0, 2, 0);
                foreach(GameObject a in GameObject.FindObjectsOfType(typeof(GameObject)))
                {
                    FlyingObjectScript script = a.GetComponent<FlyingObjectScript>();
                    if (script != null)
                    {
                        a.transform.Translate(new Vector3(0, 2, 0), Space.World);
                        controller.flyingObjects.Add(a);
                        for (int i = 0; i < script.bezierPoints.Count; i++)
                        {
                            script.bezierPoints[i] = new Vector3(script.bezierPoints[i].x, script.bezierPoints[i].y+2, script.bezierPoints[i].z);
                        }
                    }
                }
            }
        }
          

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
                UI.SetActive(!UI.activeSelf);
        if (transform.position.y == 1)
        {
            left.clearFlags = CameraClearFlags.Skybox;
            right.clearFlags = CameraClearFlags.Skybox;
            left.farClipPlane = 50000;
            right.farClipPlane = 50000;
            environment = Mode.OpenSpace;
        }
    }
}
