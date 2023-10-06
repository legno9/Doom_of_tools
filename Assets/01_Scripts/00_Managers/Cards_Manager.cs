using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Cards_Manager : MonoBehaviour
{

    private static Cards_Manager manager_instance;
    public static Cards_Manager Instance {get { return manager_instance; } }
    private List<Card_Basics> draw_deck = new();
    private Dictionary<Card_Mono, Card_Basics> hand_deck = new();
    public List<Card_Basics> discard_deck = new();
    public Transform canvas;
    private int initial_deck_size;
    private int draw_quantity = 5; //If its more modify hand_cards layout
    public Transform hand_cards;
    public Card_Mono card_prefab;
    public TMP_Text draw_deck_number;
    public TMP_Text discard_deck_number;
    public TMP_Text mana;
    public bool attack_preview_availaible = true;
    public bool drawing = false;
    public Card_Mono card_being_clicked;
    public int start_mana = 4;
    private int left_mana;
    public Button_Anims end_turn_button;
    private List<Card_Basics> cards_to_use;



    private void Awake() { 
        
        if (manager_instance == null){

            manager_instance = this;
        }else{

            Destroy(this.gameObject);}
    }

    private void Start() {
        
        if (PlayerPrefs.GetInt ("TutorialPlayed") == 0){
            cards_to_use = Cards_Data.tutorial_list;

        }else{
            cards_to_use = Cards_Data.cards_list;
        }
        initial_deck_size = (cards_to_use.Count * 2) - 2;
        CreateDeck();
        StartCoroutine(DrawCards());
        
    
    }

    private void CreateDeck(){
        
        while (draw_deck.Count < initial_deck_size){

            for (int i = 1; i < cards_to_use.Count; i++){

                draw_deck.Add(cards_to_use[i]); 
            }

        }

        Shuffle();
        

    }

    public IEnumerator DrawCards(){

        RefillMana();
        int cards_drawed = 0;
        end_turn_button.SetButtonActive( false);
        drawing = true;

        while ( cards_drawed < draw_quantity ){

            if (draw_deck.Count > 0){

                Card_Mono card_drawed =  Instantiate(card_prefab, draw_deck_number.transform.position + new Vector3 (0.5f,4.5f,0), Quaternion.Euler(0,0,0), canvas);
                card_drawed.this_id = draw_deck[0].id;
                
                card_drawed.animator.Play("Card_Drawed");
                card_drawed.card_sibling_index = draw_quantity - 1 - cards_drawed;

                hand_deck.Add (card_drawed, draw_deck[0]);
                draw_deck.RemoveAt(0);
                draw_deck_number.text = draw_deck.Count.ToString();

                cards_drawed += 1;
                Audio_Manager.instance.Play("Card_Out");


            }else if (discard_deck.Count != 0){ //Refill
                
                RefillDrawDeck();

            }

            else{
                
                if (discard_deck.Count == 0){
                    drawing = false;
                    end_turn_button.SetButtonActive( true);
                    yield break;
                }
            }

            yield return new WaitForSeconds(0.9f);
        }
        
        drawing = false;
        end_turn_button.SetButtonActive( true);
    }

    public void DiscardCard( Card_Mono card_used){

        StartCoroutine(card_used.MoveToDiscard());
        discard_deck.Add(hand_deck [card_used]);
        hand_deck.Remove(card_used);
        SetCardsIndex(); 
    }

    public void DiscardHand(){

        List<Card_Mono> cards_to_discard = new List<Card_Mono> (hand_deck.Keys);

        foreach (Card_Mono c in cards_to_discard){

            c.transform.SetParent(canvas);
            DiscardCard(c);
            
        }
    }

    
    private void SetCardsIndex(){
        
        int index = 0;

        foreach (Card_Mono card in hand_deck.Keys){

            card.card_sibling_index = index;
            index += 1;    
        }
    }

    public bool IsEnoughMana(int mana_used){

        string end = "/" + start_mana.ToString();

        if (left_mana - mana_used < 0){
            
            Audio_Manager.instance.Play("Error");
            mana.GetComponentInParent<Animator>().SetTrigger("Error");
            return false;

        }else{

            left_mana -= mana_used;
            mana.text = left_mana.ToString() + end;

            return true;
        }

    }
    public void RefillMana(){

        string end = "/" + start_mana.ToString();

        mana.color = Color.white;
        left_mana = start_mana;
        mana.text = left_mana.ToString() + end;

    }

    public void RefillDrawDeck(){

        int cards_in_discard = discard_deck.Count;

        for (int c = 0; c < cards_in_discard ; c++)
        {

            draw_deck.Add(discard_deck[0]);
            draw_deck_number.text = draw_deck.Count.ToString();
            discard_deck.Remove(discard_deck[0]);
            //Animation
            discard_deck_number.text = discard_deck.Count.ToString();

            
        }
        Audio_Manager.instance.Play("Card_Shuffle");
        Shuffle();
    }

    public void RemoveCardsCharacterDead( Characters_Basic character_dead){

        List<Card_Basics> basics_to_check = new List<Card_Basics> (draw_deck);

        for (int c = 0; c < basics_to_check.Count; c++){
            
            if (basics_to_check[c].character == character_dead.name_){

                draw_deck.Remove(basics_to_check[c]);
                draw_deck_number.text = draw_deck.Count.ToString();

            }
        }

        basics_to_check.Clear();
        basics_to_check.AddRange(discard_deck);

        for (int c = 0; c < basics_to_check.Count; c++){
            
            if (basics_to_check[c].character == character_dead.name_){

                discard_deck.Remove(basics_to_check[c]);
                discard_deck_number.text = discard_deck.Count.ToString();

            }
        }

        basics_to_check.Clear();
        basics_to_check.AddRange(hand_deck.Values);
        List<Card_Mono> monos_to_check = new List<Card_Mono> (hand_deck.Keys);

        for (int c = 0; c < basics_to_check.Count; c++){
            
            if (basics_to_check[c].character == character_dead.name_){

                hand_deck.Remove(monos_to_check[c]);
                StartCoroutine(monos_to_check[c].DissolveCard());
            }
        }
    }

    private void Shuffle(){
        
        for (var i = 0; i < draw_deck.Count - 1; ++i)
        {
            int r = Random.Range(i, draw_deck.Count);
            Card_Basics tmp = draw_deck[i];
            draw_deck[i] = draw_deck[r];
            draw_deck[r] = tmp;
        }
    }
}
