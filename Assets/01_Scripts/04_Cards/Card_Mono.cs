using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card_Mono: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    
    private Card_Basics card_data;
    public int this_id;

    private int id;
    private Names.attack_selector attack;
    private Names.character character;
    private int cost;
    private int damage;
    private string description;
    public string card_name;
    public int card_sibling_index;
    public bool on_hand;
    public bool on_character = false;
   
    public Animator animator;
    public GameObject visuals;

    public TMP_Text name_text;
    public TMP_Text cost_text;
    public TMP_Text description_text;
    public TMP_Text character_text;
    private Cards_Manager cards_manager = Cards_Manager.Instance;

    private void Start() {
        
        card_data = Cards_Data.cards_list[this_id];

        id = card_data.id;
        attack = card_data.attack;
        card_name = card_data.card_name;
        character = card_data.character;
        cost = card_data.cost;
        description = card_data.description;

        name_text.text = card_name;
        cost_text.text = cost.ToString();
        description_text.text = description.ToString();   
        character_text.text = character.ToString();      
    }

    public IEnumerator MoveToCharacter (){ 

        Vector3 start_position = transform.position;
        Vector3 character_position = Mouse_Manager.Instance.GetCharacterTransform(character).transform.position;
        Vector3 end_position = Camera.main.WorldToScreenPoint(character_position);

        float frames_taken = 0;
        float frames_needed = 20;
        
        while (frames_taken < frames_needed){
            
            transform.position = Vector3.Lerp(start_position, end_position, Mathf.SmoothStep(0, 1, (frames_taken/frames_needed)));

            frames_taken += 1;

            yield return new WaitForEndOfFrame();
        }

        Attacks_Manager.Instance.ClearPreview();
        Attacks_Manager.Instance.GetAllyCharacterAttacks(character, attack);
        Attacks_Manager.Instance.SetAllyAttack();
        on_character = true;    
    }

    public IEnumerator MoveToHand (){

        Vector3 start_position = transform.position;
        Vector3 end_position = new Vector3 ( Screen.width/2, Screen.height/10 ,0 ); //Middle 

        float frames_taken = 0;
        float frames_needed = 20;

        while (frames_taken < frames_needed){

            transform.position = Vector3.Slerp(start_position, end_position, Mathf.SmoothStep(0, 1, (frames_taken/frames_needed)));
            Vector3 next_position = Vector3.Slerp(start_position, end_position, Mathf.SmoothStep(0, 1, ((frames_taken +1)/frames_needed)));

            Vector3 direction = (next_position - transform.position).normalized;
            visuals.transform.up = -direction;
            frames_taken += 1;

            yield return new WaitForEndOfFrame();
        }
        
        transform.position = end_position;
        visuals.transform.up = Vector3.zero;
        transform.SetParent(cards_manager.hand_cards);
        
        transform.SetSiblingIndex(card_sibling_index);
        on_hand = true;
    }

    public IEnumerator MoveToDiscard (){ 

        Vector3 start_position = transform.position;
       
        TMP_Text discard = cards_manager.discard_deck_number;
        Vector3 end_position = discard.transform.position;

        animator.enabled = false;
        visuals.GetComponent<Image>().enabled = true;
        visuals.GetComponent<RectTransform>().localScale = new Vector3 ( 0.15f,0.15f,0.15f);

        float frames_taken = 0;
        float frames_needed = 15;
        
        while (frames_taken < frames_needed){
            
            transform.position = Vector3.Slerp(start_position, end_position, Mathf.SmoothStep(0, 1, (frames_taken/frames_needed)));
            Vector3 next_position = Vector3.Slerp(start_position, end_position, Mathf.SmoothStep(0, 1, ((frames_taken +1)/frames_needed)));

            Vector3 direction = (next_position - transform.position).normalized;
            visuals.transform.up = direction;
            frames_taken += 1;

            yield return new WaitForEndOfFrame();
        }

        cards_manager.attack_preview_availaible = true;
        cards_manager.card_being_clicked = null;
        discard.text = cards_manager.discard_deck.Count.ToString();
        Destroy(this.gameObject);
    }

    public void OnPointerEnter(PointerEventData pointerEventData){
        
        if (on_hand){       
            
            animator.Play("Card_Hovered");
            animator.SetBool("Hovered",true);
            visuals.GetComponent<Canvas>().sortingOrder = 1;

            if (cards_manager.attack_preview_availaible){

                Attacks_Manager.Instance.GetAllyCharacterAttacks(character, attack);
            }
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData){
        
        if (on_hand){

            animator.SetBool("Hovered",false);
            visuals.GetComponent<Canvas>().sortingOrder = 0;
            
            if (cards_manager.attack_preview_availaible){

                Attacks_Manager.Instance.ClearPreview();
            }
            
        }  
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {   
        if ( on_hand && pointerEventData.button == PointerEventData.InputButton.Left){
            
            animator.Play("Card_Clicked");

            if (cards_manager.card_being_clicked is not null){ //If multiple cards are clicked at the same time

                StartCoroutine(cards_manager.card_being_clicked.ReturnToHand());

            }

            cards_manager.card_being_clicked = this;
            cards_manager.attack_preview_availaible = false;
            
            if (cards_manager.IsEnoughMana(cost)){
                
                StartCoroutine(CardClicked());

                if (Mouse_Manager.Instance.card_on_use is not null){ //If there is already one 

                    StartCoroutine(Mouse_Manager.Instance.card_on_use.ReturnToHand());

                }

            }else{

                cards_manager.attack_preview_availaible = true;

            }
        } 
    }

    public IEnumerator CardClicked(){

        yield return new WaitForSeconds(0.4f); //Wait for clicked anim
        yield return new WaitUntil(() => Mouse_Manager.Instance.card_on_use == null);

        Mouse_Manager.Instance.card_on_use = this;
        cards_manager.attack_preview_availaible = false;

        on_hand = false;
        transform.SetParent(cards_manager.canvas);
        animator.Play("Card_Selected");
        animator.SetBool("Selected", true);   
            
    }

    public IEnumerator ReturnToHand()
    {   
        yield return new WaitUntil(() => on_character == true);

        on_character = false;
        Mouse_Manager.Instance.ClearAndHide();
        transform.SetParent(cards_manager.hand_cards);
        animator.SetBool("Selected", false);
        transform.SetSiblingIndex(card_sibling_index);
        visuals.GetComponent<Canvas>().sortingOrder = 0;
        cards_manager.IsEnoughMana(-cost);
        
    }

}
