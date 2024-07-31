using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;

public class BombDisplay : MonoBehaviour
{

    public GameObject bomb;
    public int movesLeft;
    public TextMeshPro text;

    private Vector3 pos;
    private int timer;

    public Tile redground;

    public AudioSource tick;
    public AudioSource explode;

    public ParticleSystem ps;

    [SerializeField]
    public Tilemap map;

    // Start is called before the first frame update
    void Start()
    {
        pos = bomb.transform.position;
        text.text = movesLeft+"";
    }

    // Update is called once per frame
    void Update()
    {
        if(movesLeft>-1) {
            if(pos != bomb.transform.position && ((int)(bomb.transform.position.x-0.5f) == Mathf.Round((bomb.transform.position.x-0.5f)*100f)/100) && ((int)(bomb.transform.position.y-0.5f) == Mathf.Round((bomb.transform.position.y-0.5f)*100f)/100f)) {
                movesLeft--;
                if(movesLeft != 0) {
                    tick.Play();
                }
            }
            text.text = movesLeft+"";
            pos = bomb.transform.position;
            if(movesLeft == 0) {
                Vector3Int bposition = new Vector3Int((int)(bomb.transform.position.x-0.5), (int)(bomb.transform.position.y-0.5), 0);
                if(map.GetTile<Tile>(bposition+(new Vector3Int(0,1,0))).name == "crack") {
                    map.SetTile(bposition+(new Vector3Int(0,1,0)), redground);
                    map.SetTile(bposition+(new Vector3Int(0,2,0)), redground);
                }
                ps.Play();
                movesLeft = -2;
            }
        } else if (movesLeft < -1) {
            timer++;
            if(timer == 7) {
                GameObject[] bombs = GameObject.FindGameObjectsWithTag("rock");
                for(int i = 0; i < bombs.Length; i++) {
                    if((bombs[i].transform.position - bomb.transform.position).magnitude <= 1.5)
                        if(bomb == bombs[i]) {
                            explode.Play();
                            bomb.GetComponent<SpriteRenderer>().enabled = false;
                            text.text = "";
                            Destroy(bomb, 0.5f);
                        } else {
                            if(bombs[i].name.Substring(0,4) == "bomb") {
                                bombs[i].GetComponent<BombDisplay>().movesLeft = 0;
                            }
                        }
                }
                movesLeft = -1;
            }
        }
        
    }
}
