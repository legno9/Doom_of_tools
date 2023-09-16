using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Start_Menu : MonoBehaviour
{

    private float mouse_pos_x;
    private float mouse_pos_y;
    public int move_quantity = 5;
    public GameObject background_back;
    public GameObject background_mid;
    public GameObject background_near;
    public GameObject main;
    public GameObject options;
    public Material blur;

    private void Start() {
        
        Application.targetFrameRate = 60;

    }
    private void Update() {
        
        mouse_pos_x = Input.mousePosition.x;
        mouse_pos_y = Input.mousePosition.y;

        background_mid.GetComponent<RectTransform>().position = Parallax(move_quantity);
        background_near.GetComponent<RectTransform>().position = Parallax(move_quantity*4f);
    }

    private Vector2 Parallax(float movement){

        return new Vector2( (mouse_pos_x/Screen.width) * movement + (Screen.width/2), (mouse_pos_y/Screen.height) * movement + (Screen.height /2));

    }

    public void LoadGame(){

        SceneManager.LoadScene("Game");

    }

    public void QuitGame(){

        Application.Quit();

    }

    public void OpenOptions(){

        main.SetActive(false);
        options.SetActive(true);
        background_back.GetComponent<Image>().material = blur;
        background_mid.GetComponent<Image>().material = blur;
        background_near.GetComponent<Image>().material = blur;
    }

    public void BackToMain(){

        main.SetActive(true);
        options.SetActive(false);
        background_back.GetComponent<Image>().material = null;
        background_mid.GetComponent<Image>().material = null;
        background_near.GetComponent<Image>().material = null;

    }

}
