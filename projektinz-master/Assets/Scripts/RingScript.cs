using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LZWPlib;
public class RingScript : MonoBehaviour {
    public enum direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public bool right = true;
    public float speed = 10;
    public bool CreateNext = false;
    public direction dir;
    public int group = 0;

	// Use this for initialization
	void Start () {
        transform.tag = "Grupa " + group.ToString();
	}
	
	// Update is called once per frame
	void Update () {
        if (LzwpManager.Instance.isServer)
        {
            if (right)
                transform.Rotate(speed * Time.deltaTime, 0, 0);
            else
                transform.Rotate(-1 * speed * Time.deltaTime, 0, 0);
        }
    }

    void createNextPuzzle()
    {
        
        GameObject positionMarker = new GameObject();
        positionMarker.transform.position = transform.parent.transform.position;
        positionMarker.transform.rotation = transform.parent.transform.rotation;
        bool exists = false;
        if (dir == direction.Down)
        {
            positionMarker.transform.Rotate(0, 180, 0);
        }
        else if (dir == direction.Right)
        {
            positionMarker.transform.Rotate(0, 90, 0);
        }
        else if (dir == direction.Left)
        {
            positionMarker.transform.Rotate(0, -90, 0);
        }
        positionMarker.transform.Translate(0, 0, MasterScript.master.puzzleSize);
        foreach (var place in MasterScript.master.placements)
        {
            if (place == positionMarker.transform.position)
                exists = true;
        }
        if (!exists)
        {

            GameObject newPuzzle = (GameObject)Network.Instantiate(MasterScript.master.nextPuzzle(), positionMarker.transform.position, positionMarker.transform.rotation, 1);
            MasterScript.master.placements.Add(newPuzzle.transform.position);
            newPuzzle.SetActive(true);
        }
        Destroy(positionMarker);
    }
    void OnTriggerEnter(Collider other)
    {
        if (LzwpManager.Instance.isServer)
        {
            if (CreateNext)
            {
                CreateNext = false;
                createNextPuzzle();
            }
        }
    }
}
