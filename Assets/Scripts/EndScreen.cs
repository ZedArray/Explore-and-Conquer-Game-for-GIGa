using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] Image img1, img2;

    int dialogueCounter;
    // Start is called before the first frame update
    void Start()
    {
        _text.text = "*Insert very cool cutscene of killing first boss*";
        dialogueCounter = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            dialogueCounter++;
        }

        switch (dialogueCounter)
        {
            case 2:
                _text.text = "Excellent";
                img1.gameObject.SetActive(false);
                img2.gameObject.SetActive(true);
                break;

            case 3:
                _text.text = "You've successfully made us one step closer to our goal";
                break;

            case 4:
                _text.text = "Oh and one thing";
                break;

            case 5:
                _text.text = "You can now use the C4 you just got";
                break;

            case 6:
                _text.text = "(will be added later)";
                break;

            case 7:
                _text.text = "";
                break;

            case 8:
                _text.text = "I guess that's all for this game demo";
                break;

            case 9:
                _text.text = "I do have plans to make this into a full game";
                break;

            case 10:
                _text.text = "I've gone too deep into it and I like how it has gone so far.";
                break;

            case 11:
                _text.text = "";
                break;

            case 12:
                _text.text = "So I guess that's it";
                break;

            case 13:
                _text.text = "Thank you GIGa Lab for giving me this opportunity";
                break;

            case 14:
                _text.text = "Through this, I have actually made a game instead of just talking about making a game";
                break;

            case 15:
                _text.text = "So thank you, GIGa";
                break;

            case 16:
                _text.text = "Even if I'm not accepted into GIGa, I'll still continue to develop games";
                break;

            case 17:
                _text.text = "It is hard, but it is also kinda fun";
                break;

            case 18:
                _text.text = "And thank you for playing this game to anyone I will be sending this game to";
                break;

            case 19:
                _text.text = "Y'all are great. Hope y'all enjoyed the game";
                break;

            case 20:
                _text.text = "Bye bye now";
                break;

            case 21:
                PlayerPrefs.SetInt("SavedScene", 0);
                SceneManager.LoadScene(0);
                break;
        }
    }
}
