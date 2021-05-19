using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class dusmanKontrol : MonoBehaviour
{
   
    GameObject[] Gidileceknoktalar;
    GameObject karakter;
    public GameObject kursun;

    bool aradakiMesafeyiBirKereAl = true;
    bool ilerimiGeriMi = true;

    Vector3 aradakiMesafe;

    int aradakiMesafeSayacı = 0;
    int hiz = 5;
    float atesZamani = 0;

    RaycastHit2D ray;

    public LayerMask layermask;

    public Sprite onTaraf;
    public Sprite arkaTaraf;

    SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        karakter = GameObject.FindGameObjectWithTag("Player");
        Gidileceknoktalar = new GameObject[transform.childCount];
        for (int i = 0; i < Gidileceknoktalar.Length; i++)
        {
            Gidileceknoktalar[i] = transform.GetChild(0).gameObject;
            Gidileceknoktalar[i].transform.SetParent(transform.parent);
        }
    }


    void FixedUpdate()
    {
        beniGorduMu();
        if (ray.collider.tag == "Player")
        {
            hiz = 8;
            spriteRenderer.sprite = onTaraf;
            atesEt();
        }
        else
        {
            hiz = 4;
            spriteRenderer.sprite = arkaTaraf;
        }
        noktalaraGit();
        
    }
    void atesEt()
    {
        atesZamani += Time.deltaTime;
        if (atesZamani>Random.Range(0.2f,1))
        {
            Instantiate(kursun, transform.position, Quaternion.identity);
            atesZamani = 0;
            
        }
    }
    void beniGorduMu()
    {
        Vector3 rayYonum = karakter.transform.position - transform.position;
        ray = Physics2D.Raycast(transform.position,rayYonum,1000,layermask);
        Debug.DrawLine(transform.position,ray.point,Color.magenta);
    }
    void noktalaraGit()
    {
        if (aradakiMesafeyiBirKereAl)
        {
            aradakiMesafe = (Gidileceknoktalar[aradakiMesafeSayacı].transform.position - transform.position).normalized;
            aradakiMesafeyiBirKereAl = false;
        }
        float mesafe = Vector3.Distance(transform.position, Gidileceknoktalar[aradakiMesafeSayacı].transform.position);
        transform.position += aradakiMesafe * Time.deltaTime * hiz;


        if (mesafe < 0.5f)
        {
            aradakiMesafeyiBirKereAl = true;
            if (aradakiMesafeSayacı == Gidileceknoktalar.Length - 1)
            {
                ilerimiGeriMi = false;
            }
            else if (aradakiMesafeSayacı == 0)
            {
                ilerimiGeriMi = true;
            }
            if (ilerimiGeriMi)
            {
                aradakiMesafeSayacı++;
            }
            else
            {
                aradakiMesafeSayacı--;

            }

        }
    }
    public Vector2 getYon()
    {
        return (karakter.transform.position - transform.position).normalized;
    }
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.GetChild(i).transform.position, 1f);
        }
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.GetChild(i).transform.position, transform.GetChild(i + 1).transform.position);
            Debug.Log("çizgi çizdi");
        }
        Debug.Log("bu metod çalışıyo");
    }

#endif
}



#if UNITY_EDITOR
[CustomEditor(typeof(dusmanKontrol))]
[System.Serializable()]
class dusmanSKontrolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        dusmanKontrol script = (dusmanKontrol)target;
        EditorGUILayout.Space();
        if (GUILayout.Button("ÜRET", GUILayout.MinWidth(100), GUILayout.Width(100)))
        {
            GameObject yeniobjem = new GameObject();
            yeniobjem.transform.parent = script.transform;
            yeniobjem.transform.position = script.transform.position;
            yeniobjem.name = script.transform.childCount.ToString();

        }
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("layermask"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("onTaraf"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("arkaTaraf"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("kursun"));
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }
}
#endif
