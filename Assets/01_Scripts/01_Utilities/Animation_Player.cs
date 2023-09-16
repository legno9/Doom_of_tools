using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Animation_Player : MonoBehaviour
{

    public Image image;

    public Sprite[] sprites;
    private float anim_speed = 0.1f;
    private float anim_delay = 4f;

    private int sprite_index;
    Coroutine coroutine;
    bool done;


    private void Start()
    {
        done = false;
        StartCoroutine(Func_PlayAnimUI());
    }

    public void Func_StopUIAnim()
    {
        done = true;
        StopCoroutine(Func_PlayAnimUI());
    }
    IEnumerator Func_PlayAnimUI()
    {
        yield return new WaitForSeconds(anim_speed);
        if (sprite_index >= sprites.Length)
        {
            sprite_index = 0;
            yield return new WaitForSeconds(anim_delay);
            
        }
        image.sprite = sprites[sprite_index];
        sprite_index += 1;
        if (done == false)
            coroutine = StartCoroutine(Func_PlayAnimUI());
    }
}