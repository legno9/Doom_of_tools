public class Names{
    public enum character{
        None,
        Peeler,
        Blender,
        Microwave,
        Mill,
        Pot,
        Cutter,
        Rolling,
        SaltShaker,
        Spatula,
        Spoon,
    }

    public enum direction_name{
        Top,
        Bottom,
        Right,
        Left,
    }

    public enum status_effect{
        None,
        Blind,
        Inmobil,
        Disabled,
        Bless,
        Curse,
        Burn,
        Heal,
        Posion,
        Weak,
        Acceleration,
        Slowdown,
        Light,
        Heavy,
        Metallic,
        Shield,
        Invisible,
        Taunt,
    }

    public enum tiles_selector{
        adjacent_tile,
        adjacent_tiles,
        vertical_tiles,
        horizontal_tiles,
        cross_tiles,
        surrounding_tiles,
        multiple_hortizontal_tiles,
        enemies_tiles,
        one_enemy_tile,
        ally_tiles,
        one_ally_tile,
        characters_tiles,
        aleatory_tiles1,
    } 

    public enum attack_selector{

        none,
        attack_01,
        attack_02,
        attack_03,
        attack_04,
        attack_05,
        attack_06,
    }

    public enum attack_names{

        none,
        Sweep,
        DeathMark,
        Headbutt,
        Stomp,
        Overload,
        Hypershot,
        Splash,
        Overflow,
        Sharpshooter,
        Meatball
    }

}
