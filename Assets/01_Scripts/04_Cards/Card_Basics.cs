using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Basics
{
    public int id;
    public int cost;
    public string description;
    public string card_name;
    public Names.character character;
    public Names.attack_selector attack;

    public Card_Basics (int id_, Names.character character_, Names.attack_selector attack_, string name_,  int cost_, string description_  ){

        id = id_;
        attack = attack_;
        card_name = name_;
        cost = cost_;
        description = description_;
        character = character_;

    }

}
