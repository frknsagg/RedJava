using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class suAnimasyonu : MonoBehaviour
{
    public Sprite[] animasyonKareleri;
    SpriteRenderer spriteRendere;
    float zaman = 0;
    int animasyonKarelerSayacı = 0;
    void Start()
    {
        spriteRendere = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        zaman += Time.deltaTime;
        if (zaman > 0.03f)
        {
            spriteRendere.sprite = animasyonKareleri[animasyonKarelerSayacı++];
            if (animasyonKareleri.Length == animasyonKarelerSayacı)
            {
                animasyonKarelerSayacı = 0;
            }
            zaman = 0;
        }
    }
}

