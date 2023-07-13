using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_Bar : MonoBehaviour
{
    [SerializeField] private GameObject separator_end;
    [SerializeField] private Sprite separator;
    private List<GameObject> separators_list = new List<GameObject>();
    private float bar_width = -93.75f;
    private float character_health_points;
    public Health_System character_health_system;
    public Image fill;
    

    void Start(){ 

        character_health_points = GetComponentInParent<Character_Script>().health;
        character_health_system = GetComponentInParent<Character_Script>().health_system;
        
        int health_blocks_needed = (int)(character_health_points/10);

        if (health_blocks_needed > 1){

                for (int separators_created = 1; separators_created< health_blocks_needed; separators_created++ ){
                    
                    GameObject separator_gameobject = new GameObject ("Separator");
                    separator_gameobject.AddComponent<Image>().sprite = separator;
                    separator_gameobject.transform.SetParent(transform);
                    separator_gameobject.GetComponent<Image>().SetNativeSize();

                    float separator_position = bar_width/health_blocks_needed * separators_created +3.1375f;

                    separator_gameobject.GetComponent<RectTransform>().localPosition = new Vector3 (separator_position,0,0);

                    separator_gameobject.GetComponent<RectTransform>().anchorMin = new Vector2 (1, 0.5f);
                    separator_gameobject.GetComponent<RectTransform>().anchorMax = new Vector2 (1, 0.5f);
                    separator_gameobject.GetComponent<RectTransform>().pivot = new Vector2 (1, 0.5f);
                    separator_gameobject.transform.localScale = new Vector3 (1,1,1);
                    
                    separators_list.Add(separator_gameobject);
                }
            }
        separators_list.Reverse();

    }


    public void UpdateBar() 
    {

        fill.fillAmount= character_health_system.GetHealth()/character_health_points;
        
        if (character_health_points != character_health_system.health){

            if (character_health_system.health == 0){
            
                separator_end.GetComponent<Image>().color = Color.clear;
            }else{
                
                separator_end.GetComponent<Image>().color = Color.white;
                separator_end.GetComponent<RectTransform>().localPosition = new Vector3 (separators_list[(int) character_health_system.GetHealth()/10 -1].GetComponent<RectTransform>().localPosition.x,0,0);
            }
        }

    }
}
