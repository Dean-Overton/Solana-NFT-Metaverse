using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoRoom : MonoBehaviour
{
    private float timer = 0f;
    [SerializeField] private float frequency = 0.3f;
    [SerializeField] private SpriteRenderer lights;
    [SerializeField] private Color[] color = new Color[6];
    int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        lights = GetComponent<SpriteRenderer>();
        color[0] = new Color32(20, 20, 255, 50);
        color[1] = new Color32 (255, 130, 20, 50);
        color[2] = new Color32(20, 255, 30, 50);
        color[3] = new Color32 (222, 18, 255, 50);
        color[4] = new Color32(20, 150, 255, 50); 
        color[5] = new Color32(255, 20, 20, 50); 
        lights.color = color[index];
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > frequency){
            if(++index == color.Length)index = 0;
            lights.color = color[index];
            timer = 0;
        }
    }
}
