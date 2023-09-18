using UnityEngine;

public class Card_Reference : MonoBehaviour
{
    public Card_Mono card_script;

    public void CallMoveToHand(){

        StartCoroutine(card_script.MoveToHand());

    }

    public void CallMoveToCharacter(){

        if (Mouse_Manager.Instance.card_on_use is not null){
            StartCoroutine(card_script.MoveToCharacter());
        }
        
    }

    public void CardOnHand(){ //On return
        
        card_script.on_hand = true;
        Cards_Manager.Instance.attack_preview_availaible = true;
        Mouse_Manager.Instance.card_on_use = null;
        Cards_Manager.Instance.card_being_clicked = null;
    }

    
}
