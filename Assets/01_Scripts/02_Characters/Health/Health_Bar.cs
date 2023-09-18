using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Health_Bar : MonoBehaviour
{
    [SerializeField] private GameObject separator_end;
    [SerializeField] private Sprite separator;
    private readonly List<GameObject> separators_list = new();
    private readonly float bar_width = 90.5f;
    private float character_health_points;
    public Health_System character_health_system;
    public Image fill;
    public Image border;
    public Material dissolve;
    private Characters_Basic basics;
    

    void Start(){ 
        basics = GetComponentInParent<Characters_Basic>();
        character_health_points = basics.health;
        character_health_system = basics.health_system;
        
        int health_blocks_needed = (int)(character_health_points/10);

        if (health_blocks_needed > 1){

                for (int separators_created = 1; separators_created< health_blocks_needed; separators_created++ ){
                    
                    GameObject separator_gameobject = new("Separator");
                    separator_gameobject.AddComponent<Image>().sprite = separator;
                    separator_gameobject.transform.SetParent(transform);
                    separator_gameobject.GetComponent<Image>().SetNativeSize();

                    float separator_position = bar_width/health_blocks_needed * separators_created ;

                    separator_gameobject.GetComponent<RectTransform>().localPosition = new Vector3 (-separator_position,0,0);

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

        fill.fillAmount= character_health_system.GetHealth()/character_health_points * bar_width/100 ; // Hp% left * total bar size/100
        
        if (character_health_points != character_health_system.health){

            if (character_health_system.health <= 0){

                basics.Dead();
                border.material = dissolve;
                Destroy(separator_end);
                foreach (GameObject separator in separators_list)
                {
                    Destroy(separator);
                }

                Destroy(separator_end);
                StartCoroutine(DissolveHealth());


            }else{
                
                separator_end.GetComponent<Image>().color = Color.white;
                separator_end.GetComponent<RectTransform>().localPosition = new Vector3 (separators_list[(int) character_health_system.GetHealth()/10 -1].GetComponent<RectTransform>().localPosition.x,0,0);
            }
        }

    }

    public IEnumerator DissolveHealth(){
        
        float fade = 1;
        border.material.SetFloat("_Fade", fade);

        while (fade >= 0 ){

            border.material.SetFloat("_Fade", fade);

            fade -= Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }
}
