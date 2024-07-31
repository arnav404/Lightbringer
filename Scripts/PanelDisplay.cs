using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PanelDisplay : MonoBehaviour
{

    public Vector3Int wallPosition;
    public Vector3Int floorPosition;
    public bool vertical;

    public GameObject panel;
    public SpriteRenderer sr;
    
    public Sprite onSprite;
    public Sprite offSprite;

    public List<Transform> possibles;

    public Tile redground;
    public Tile wall;
    public Tile black;

    [SerializeField]
    public Tilemap map;

    public AudioSource pressed;

    bool IsOnTile(Vector3 one, Vector3 two) {
        return one.x == two.x && one.y == two.y;
    }

    // Start is called before the first frame update
    void Start()
    {
        possibles.Add(GameObject.Find("Lightbringer").GetComponent<Transform>());
        GameObject[] objects = GameObject.FindGameObjectsWithTag("rock");
        for(int i = 0; i < objects.Length; i++) {
            possibles.Add(objects[i].transform);
        }
    }

    // Update is called once per frame
    void Update()
    {

        bool on = false;

        //Loop through possibles
        for(int i = 0; i < possibles.Count; i++) {
            if(possibles[i] != null) {
                //If something is there
                if(IsOnTile(panel.transform.position, possibles[i].position)) {
                    if(sr.sprite == offSprite) {
                        pressed.Play();
                    }
                    if(vertical) {
                        map.SetTile(floorPosition, redground);
                        map.SetTile(wallPosition, redground);
                        sr.sprite = onSprite;
                    } else {
                        map.SetTile(floorPosition, redground);
                        map.SetTile(wallPosition, wall);
                        sr.sprite = onSprite;
                    }
                    on = true;
                    break;
                }
            }
        }
        if(!on) {
            if(vertical) {
                    map.SetTile(floorPosition, black);
                    map.SetTile(wallPosition, wall);
                    sr.sprite = offSprite;
            } else {
                map.SetTile(floorPosition, black);
                map.SetTile(wallPosition, black);
                sr.sprite = offSprite;
            }
        }
    }
}
