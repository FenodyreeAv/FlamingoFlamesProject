using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Ross
public class UIFlicker : MonoBehaviour
{
    public Image flickerImage;
    public float minAlpha = 0.3f;
    public float maxAlpha = 0.6f;
    public float flickerSpeed = 0.1f;

    private float timer;
    
    void Start()
    {
        if(flickerImage == null)
            flickerImage = GetComponent<Image>();
    }

    
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= flickerSpeed)
        {
            float alpha = Random.Range(minAlpha, maxAlpha);
            Color c = flickerImage.color;
            c.a = alpha;
            flickerImage.color = c;
            timer = 0;
        }
        
    }
}
