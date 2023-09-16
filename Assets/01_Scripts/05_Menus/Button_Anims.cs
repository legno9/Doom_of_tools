using UnityEngine;
using UnityEngine.UI;

public class Button_Anims : MonoBehaviour
{   
    private Animator button_anim;
    public bool active;

    private void Awake() {

        button_anim = transform.GetComponent<Animator>();
        transform.GetComponent<Button>().interactable = active;
        SetButtonActive(active);

    }

    public void SetButtonActive(bool active_) {

        if (active_ == false){

            active = false;
            button_anim.Play("Disabled");
            

        }else{
            
            active = true;
            button_anim.Play("Normal");
        }

        transform.GetComponent<Button>().interactable = active;
    }

    public void PointerClick(){

        if (active){
            button_anim.SetTrigger("Pressed");
            Audio_Manager.instance.Play("Click");
        }
    }

    public void PointerEnter(){
       
        if (active){
            button_anim.Play("Hovered");
            Audio_Manager.instance.Play("Hover");
        }
        
    }

    public void PointerExit(){
        
        if (active){
            button_anim.Play("Unhovered");
        }
        
    }

    public void play(){

    }
}
