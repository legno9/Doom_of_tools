using System.Collections.Generic;

public static class Cards_Data{
    public static List<Card_Basics> cards_list = new List<Card_Basics>{
        
        new Card_Basics (0, Names.character.None, Names.attack_selector.attack_01, Names.attack_names.none, 0, "None" ),
        new Card_Basics (1, Names.character.Peeler, Names.attack_selector.attack_01, Names.attack_names.Sweep, 1, "Sweep three tiles vertically to inflict 1 damage"),
        new Card_Basics (2, Names.character.Peeler, Names.attack_selector.attack_02, Names.attack_names.DeathMark, 2, "Apply DeathMark to inflict 3 damage to an adjacent tile"),
        new Card_Basics (3, Names.character.Blender, Names.attack_selector.attack_01, Names.attack_names.Headbutt, 1, "Headbutt to inflict 1 damage to an adjacent tile"),
        new Card_Basics (4, Names.character.Blender, Names.attack_selector.attack_02, Names.attack_names.Stomp, 1, "Stomp the ground to deal 1 damage to surrounding tiles"),
        new Card_Basics (5, Names.character.Spoon, Names.attack_selector.attack_01, Names.attack_names.Sharpshooter, 1, "Shoot food to inflict 2 damage in a single tile"),
        new Card_Basics (6, Names.character.Spoon, Names.attack_selector.attack_02, Names.attack_names.Meatball, 1, "Throw a meatball to inflict 1 damage to a row of three tiles"),
        new Card_Basics (7, Names.character.Pot, Names.attack_selector.attack_01, Names.attack_names.Splash, 1, "Splash to inflict 1 damage to a row of three tiles"),
        new Card_Basics (8, Names.character.Pot, Names.attack_selector.attack_02, Names.attack_names.Overflow, 2, "Overflow to inflict 1 damage to multiple surrounding tiles"),
        
    };

    public static List<Card_Basics> tutorial_list = new List<Card_Basics>{
        
        new Card_Basics (0, Names.character.None, Names.attack_selector.attack_01,Names.attack_names.none, 0, "None" ),
        new Card_Basics (1, Names.character.Peeler, Names.attack_selector.attack_01, Names.attack_names.Sweep, 1, "Sweep three tiles vertically to inflict 1 damage"),
        new Card_Basics (2, Names.character.Peeler, Names.attack_selector.attack_02, Names.attack_names.DeathMark, 2, "Inflicts 3 damage to an adjacent tile"),
        
    };

    public static List<Card_Basics> aards_list = new List<Card_Basics>{
        
        new Card_Basics (0, Names.character.None, Names.attack_selector.attack_01, Names.attack_names.none, 0, "None" ),
        new Card_Basics (1, Names.character.Peeler, Names.attack_selector.attack_01, Names.attack_names.Sweep, 1, "Sweep three tiles vertically to inflict 1 damage"),
        new Card_Basics (2, Names.character.Peeler, Names.attack_selector.attack_02, Names.attack_names.DeathMark, 2, "Apply DeathMark to inflict 3 damage to an adjacent tile"),
        new Card_Basics (3, Names.character.Blender, Names.attack_selector.attack_01, Names.attack_names.Headbutt, 1, "Headbutt to inflict 1 damage to an adjacent tile"),
        new Card_Basics (4, Names.character.Blender, Names.attack_selector.attack_02, Names.attack_names.Stomp, 1, "Stomp the ground to deal 1 damage to surrounding tiles"),
        new Card_Basics (5, Names.character.Microwave, Names.attack_selector.attack_01, Names.attack_names.Overload, 2, "Overload to inflict 2 damage to surrounding tiles"),
        new Card_Basics (6, Names.character.Microwave, Names.attack_selector.attack_02, Names.attack_names.Hypershot, 1, "Charge radiation to inflict 2 damage in two vertical squares downward "),
        new Card_Basics (7, Names.character.Pot, Names.attack_selector.attack_01, Names.attack_names.Splash, 1, "Splash to inflict 1 damage to a row of three tiles"),
        new Card_Basics (8, Names.character.Pot, Names.attack_selector.attack_02, Names.attack_names.Overflow, 2, "Overflow to inflict 1 damage to multiple surrounding tiles"),
        new Card_Basics (9, Names.character.Spoon, Names.attack_selector.attack_01, Names.attack_names.Sharpshooter, 1, "Shoot food to inflict 2 damage in a single tile"),
        new Card_Basics (10, Names.character.Spoon, Names.attack_selector.attack_02, Names.attack_names.Meatball, 1, "Throw a meatball to inflict 1 damage to a row of three tiles"),
        
    };
}
