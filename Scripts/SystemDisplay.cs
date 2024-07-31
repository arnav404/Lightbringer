using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class SystemDisplay : MonoBehaviour
{

    public SystemData panelData;
    public GameObject[] panels;
    public GameObject panel;
    public SpriteRenderer sr;
    
    public Sprite onSprite;
    public Sprite offSprite;

    public List<Transform> possibles;

    public Tile redground;
    public Tile wall;
    public Tile black;

    public AudioSource pressed;

    [SerializeField]
    public Tilemap map;

    public TextMeshPro text;

    public bool on = false;

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

        if(!on) {

            int count = 0;

            //Loop through panels
            for(int j = 0; j < panels.Length; j++) {

                //Loop through possibles
                for(int i = 0; i < possibles.Count; i++) {
                    //If something is there
                    if(possibles[i] != null) {
                        if(IsOnTile(panels[j].transform.position, possibles[i].position)) {
                            count++;
                            break;
                        }
                    }
                    
                }

                if(count==panels.Length) {
                    pressed.Play();
                    on = true;
                    if(panelData.vertical) {
                        map.SetTile(panelData.floorPosition, redground);
                        map.SetTile(panelData.wallPosition, redground);
                        sr.sprite = onSprite;
                    } else {
                        map.SetTile(panelData.floorPosition, redground);
                        map.SetTile(panelData.wallPosition, wall);
                        sr.sprite = onSprite;
                    }
                }

                bool check = false;

                for(int i = 0; i < possibles.Count; i++) {
                    //If something is there
                    if(possibles[i] != null) {
                        if(IsOnTile(panel.transform.position, possibles[i].position)) {
                            check = true;
                        }
                    }
                    
                }

                if(check) {
                    sr.sprite = onSprite;
                } else {
                    sr.sprite = offSprite;
                }

            }
            text.text = ""+(panels.Length-count);

        }

    }
}
