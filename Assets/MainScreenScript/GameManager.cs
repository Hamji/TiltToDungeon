using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {

    public Texture2D StartButton;
    public Texture2D OptionButton;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadGame()
    {
        SceneManager.LoadScene("GameStart");
    }

    void OnGUI()
    {
        // GUI 큰 틀 만듬
        //GUI.Box(new Rect(0, 0, 2560, 1440), MainFrame);

        
      

        if (GUI.Button(new Rect(20, 740, 1230, 660), StartButton))
        {
            Debug.Log( "GameStart_Image");
            LoadGame();
        }

        if(GUI.Button(new Rect(1300,740,1230,660),OptionButton))
        {
            Debug.Log("Option");
        }

    }

}
