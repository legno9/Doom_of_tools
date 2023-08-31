using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mill_Controller : Enemy_Basics
{
    private void Awake() {
        
        available_attacks = new Names.attack_selector[] {Names.attack_selector.attack_01 };
    }
}
