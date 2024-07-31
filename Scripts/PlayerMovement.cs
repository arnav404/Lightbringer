using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    public Progression progression;

    private bool movementEnabled = true;
    public Sprite[] sprites;
    public SpriteRenderer sr;
    public Transform lb;
    public ParticleSystem ps;

    public Animator transition;

    private string moving = "n";

    private GameObject[] rocks;
    public bool collectedTulip;

    private GameObject rockBeingPushed;
    public AudioSource bouldermove;
    public AudioSource footstep;
    public AudioSource door;
    public AudioSource goal;
    public AudioSource menu;
    public AudioSource bonk;

    public Animator animator;

    private GameObject temp;

    public GameObject textbox;
    public Text text;

    public Tile opendoor;

    [SerializeField]
    public Tilemap map;


    int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        lb.position = progression.spawnPoint;
        rocks = GameObject.FindGameObjectsWithTag("rock");
        temp = new GameObject();
        textbox.SetActive(false);
    }

    bool IsOnTile(Vector3 one, Vector3 two) {
        return one.x == two.x && one.y == two.y;
    }

    
    public void Reload() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu() {
        progression.spawnPoint = new Vector3(0.5f,0.5f,0f);
        menu.Play();
        StartCoroutine(LoadLevel(0));
    }

    public void LoadNextLevel() {
        if(!progression.tulips.Contains(SceneManager.GetActiveScene().buildIndex) && collectedTulip) {
            progression.tulips.Add(SceneManager.GetActiveScene().buildIndex);
        }
        if(SceneManager.GetActiveScene().buildIndex+1 < 7) {
            if(SceneManager.GetActiveScene().buildIndex+1 > progression.highestLevelCompleted) {
                progression.highestLevelCompleted = SceneManager.GetActiveScene().buildIndex+1;
            }
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex+1));
        } else {
            Menu();
        }
    }

    IEnumerator LoadLevel(int levelIndex) {

        if(levelIndex == SceneManager.GetActiveScene().buildIndex+1 || levelIndex == 0) {

            yield return new WaitForSeconds(1);

            sr.enabled = false;
            ps.Play();

        }

        yield return new WaitForSeconds(0.5f);

        transition.SetTrigger("End");

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(levelIndex);
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {

        rocks = GameObject.FindGameObjectsWithTag("rock");

        if(movementEnabled) {

            //Position with respect to the tilemap
            Vector3Int bposition = new Vector3Int((int)(lb.position.x-0.5f), (int)(lb.position.y-0.5f), 0);

            if(Input.GetKeyDown("r")) {
                movementEnabled = false;
                Reload();
            } else if(Input.GetKeyDown("m")) {
                movementEnabled = false;
                Menu();
            }

            Vector3Int direction = new Vector3Int(0,0,0);
            print(bposition.y);
            print(Mathf.Round((lb.position.y-0.5f)*100)/100);
            if((Mathf.Round((lb.position.x-0.5f)*100)/100) == bposition.x && (Mathf.Round((lb.position.y-0.5f)*100)/100) == bposition.y) {
                //Player Movement
                if(Input.GetKey("w")) {
                    if(moving != "w") {
                        rockBeingPushed = temp;
                    }
                    moving = "w";
                } else if(Input.GetKey("a")) {
                    if(moving != "a") {
                        rockBeingPushed = temp;
                    }
                    moving = "a";
                } else if(Input.GetKey("s")) {
                    if(moving != "s") {
                        rockBeingPushed = temp;
                    }
                    moving= "s";
                } else if(Input.GetKey("d")) {
                    if(moving != "d") {
                        rockBeingPushed = temp;
                    }
                    moving = "d";
                } else {
                    moving = "n";
                }

                if(moving=="w") {
                    footstep.Play();
                    sr.sprite = sprites[0];
                    direction = new Vector3Int(0,1,0);
                } else if(moving=="a") {
                    footstep.Play();
                    sr.sprite = sprites[1];
                    direction = new Vector3Int(-1,0,0);
                } else if(moving=="s") {
                    footstep.Play();
                    sr.sprite = sprites[2];
                    direction = new Vector3Int(0,-1,0);
                } else if(moving=="d") {
                    footstep.Play();
                    sr.sprite = sprites[3];
                    direction = new Vector3Int(1,0,0);
                } else {
                    rockBeingPushed = temp;
                    direction = new Vector3Int(0,0,0);
                }

                //Check which tile we are walking to
                if(map.GetTile<Tile>(bposition+direction).name == "redground") {

                    //Boulder Check
                    for(int i = 0; i < rocks.Length; i++) {
                        if(rocks[i] != null) {
                            if(IsOnTile(lb.position+direction, rocks[i].transform.position)) {
                                if(map.GetTile<Tile>(bposition+2*direction).name == "redground") {
                                    for(int j = 0; j < rocks.Length; j++) {
                                        if(IsOnTile(lb.position+2*direction, rocks[j].transform.position)) {
                                            moving = "n";
                                        }
                                    }
                                    if(direction != new Vector3Int(0,0,0)) {
                                        //Play boulder sound
                                    }
                                    rockBeingPushed = rocks[i];
                                } else {
                                    moving = "n";
                                }
                            }
                        }
                        
                    }

                    

                } else if(map.GetTile<Tile>(bposition+direction).name == "door-closed") {
                    print("Door closed");
                    textbox.SetActive(true);
                    text.text = "The door is locked. Gotta find a way to open it.";
                    moving = "n";
                } else if(map.GetTile<Tile>(bposition+direction).name == "unwalkable") {
                    textbox.SetActive(true);
                    text.text = "What is it you wish to hear?\n";
                    movementEnabled = false;
                    moving = "n";
                } else if(map.GetTile<Tile>(bposition+direction).name == "door-open") {
                    index++;
                    progression.spawnPoint = lb.transform.position+(new Vector3Int(0,5,0));
                    lb.transform.position = lb.transform.position+(new Vector3Int(0,5,0));
                    door.Play();
                } else if(map.GetTile<Tile>(bposition+direction).name == "goal") {
                    lb.transform.position = lb.transform.position + direction; 
                    goal.Play();
                    progression.spawnPoint = new Vector3(0.5f,0.5f,0f);
                    movementEnabled = false;
                    LoadNextLevel();
                } else {
                    print("JOE");
                    moving = "n";
                }
                
                if(moving != "n") {
                    footstep.Play();
                    animator.ResetTrigger("notrunning");
                    animator.SetTrigger("running");
                } else {
                    animator.ResetTrigger("running");
                    animator.SetTrigger("notrunning");
                }
                if(rockBeingPushed != temp && rockBeingPushed.GetComponent<BombDisplay>() == null) {
                    bouldermove.Play();
                }
                
            }

            if(moving=="w") {
                sr.sprite = sprites[0];
                direction = new Vector3Int(0,1,0);
            } else if(moving=="a") {
                sr.sprite = sprites[1];
                direction = new Vector3Int(-1,0,0);
            } else if(moving=="s") {
                sr.sprite = sprites[2];
                direction = new Vector3Int(0,-1,0);
            } else if(moving=="d") {
                sr.sprite = sprites[3];
                direction = new Vector3Int(1,0,0);
            } else {
                direction = new Vector3Int(0,0,0);
            }

            //A/B Presses
            if (Input.GetKeyDown("j")) { // A
                textbox.SetActive(false);
            } else if (Input.GetKeyDown("k")) { // B
                
            }

            

            if(moving != "n") {
                lb.position = new Vector3(Mathf.Round((lb.position.x+direction.x/10f)*100f)/100f, Mathf.Round((lb.position.y+direction.y/10f)*100f)/100f,0);
                rockBeingPushed.transform.position = new Vector3(Mathf.Round((rockBeingPushed.transform.position.x+direction.x/10f)*100f)/100f, Mathf.Round((rockBeingPushed.transform.position.y+direction.y/10f)*100f)/100f,0);
            }


        } else {
            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.Slash)) { 
                text.text += "?";
            } else {
                    foreach(KeyCode vKey in System.Enum.GetValues(typeof(KeyCode))){
                    if(Input.GetKeyDown(vKey)){
                    //your code here
                        if (vKey+"" == "Space") {
                            text.text += " ";
                        } else if (vKey+"" == "Backspace") {
                            if(text.text[text.text.Length - 1] != '\n') {
                            text.text = text.text.Remove(text.text.Length - 1, 1);
                            }
                        } else if (vKey+"" == "Slash") {
                            textbox.SetActive(false);
                            movementEnabled = true;
                        } else if((vKey+"").Length == 1) {
                            text.text += vKey;
                        }
                    }

                }
            }

        }

    }
}
