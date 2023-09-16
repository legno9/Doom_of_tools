using System.Collections.Generic;

public static class Cards_Data{
    public static List<Card_Basics> cards_list = new List<Card_Basics>{
        
        new Card_Basics (0, Names.character.None, Names.attack_selector.attack_01,"None", 0, "None" ),
        new Card_Basics (1, Names.character.Peeler, Names.attack_selector.attack_01, "Sweep", 1, "Sweep a line of tiles to deal 1 damage"),
        new Card_Basics (2, Names.character.Peeler, Names.attack_selector.attack_02, "Sharp Shoot", 1, "Deal 2 damage in a single tile"),
        new Card_Basics (3, Names.character.Blender, Names.attack_selector.attack_01, "Canon Ball", 1, "Deal 1 damage to a row of three tiles"),
        new Card_Basics (4, Names.character.Blender, Names.attack_selector.attack_02, "Stomp", 2, "Stomp the ground to deal 1 damage to multiple tiles"),
        
    };
}
