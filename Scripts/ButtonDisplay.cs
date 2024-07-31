using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonDisplay : MonoBehaviour
{
    public Progression progression;
    public Button btn;
    public Text text;
    public int level;
    public Image tulip;

    void Start() {
        if(level > progression.highestLevelCompleted) {
            btn.interactable = false;
        }
        if(progression.tulips.Contains(level)) {
            tulip.color = new Color(1f,1f,1f,1f);
        } else {
            tulip.color = new Color(0f,0f,0f,0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        text.text = level+"";
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + level);
    }
}
