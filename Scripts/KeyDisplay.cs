using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class KeyDisplay : MonoBehaviour
{

    public GameObject key;
    public Vector3Int doorPosition;
    public bool isActive = true;

    private Transform lb;

    void Start() {
        lb = GameObject.Find("Lightbringer").GetComponent<Transform>();
    }

    public Tile opendoor;

    public AudioSource pickup;

    [SerializeField]
    public Tilemap map;

    bool IsOnTile(Vector3 one, Vector3 two) {
        return one.x == two.x && one.y == two.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive && IsOnTile(key.transform.position, lb.position)) {
            isActive = false;
            pickup.Play();
            map.SetTile(doorPosition, opendoor);
            key.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
