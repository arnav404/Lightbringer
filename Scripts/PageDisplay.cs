using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageDisplay : MonoBehaviour
{

    public GameObject page;
    public SpriteRenderer sr;
    public bool isActive = true;

    private Transform lb;

    public AudioSource pickup;

    bool IsOnTile(Vector3 one, Vector3 two) {
        return one.x == two.x && one.y == two.y;
    }

    // Start is called before the first frame update
    void Start()
    {
        lb = GameObject.Find("Lightbringer").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(isActive && IsOnTile(page.transform.position, lb.position)) {
            isActive = false;
            pickup.Play();
            GameObject lbr = GameObject.Find("Lightbringer");
            lbr.GetComponent<PlayerMovement>().collectedTulip = true;
            sr.enabled = false;
        }

    }
}
