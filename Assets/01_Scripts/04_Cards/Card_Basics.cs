
public class Card_Basics
{
    public int id;
    public int cost;
    public string description;
    public Names.attack_names card_name;
    public Names.character character;
    public Names.attack_selector attack;

    public Card_Basics (int id_, Names.character character_, Names.attack_selector attack_, Names.attack_names attack_name,  int cost_, string description_  ){

        id = id_;
        attack = attack_;
        card_name = attack_name;
        cost = cost_;
        description = description_;
        character = character_;

    }

}
