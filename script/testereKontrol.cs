using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class testereKontrol : MonoBehaviour
{
    public int can;
    GameObject[] Gidileceknoktalar;

    bool aradakiMesafeyiBirKereAl = true;
    bool ilerimiGeriMi = true;

    Vector3 aradakiMesafe;
    int aradakiMesafeSayacı = 0;
    void Start()
    {
        Gidileceknoktalar = new GameObject[transform.childCount];
        for (int i = 0; i < Gidileceknoktalar.Length; i++)
        {
            Gidileceknoktalar[i]=transform.GetChild(0).gameObject;
            Gidileceknoktalar[i].transform.SetParent(transform.parent);
        }
    }

   
    void FixedUpdate()
    {
        transform.Rotate(0, 0, 5);
        noktalaraGit();
    }
    void noktalaraGit()
    {
        if (aradakiMesafeyiBirKereAl)
        {
            aradakiMesafe = (Gidileceknoktalar[aradakiMesafeSayacı].transform.position - transform.position).normalized;
            aradakiMesafeyiBirKereAl = false;
        }
        float mesafe = Vector3.Distance(transform.position, Gidileceknoktalar[aradakiMesafeSayacı].transform.position);
        transform.position += aradakiMesafe * Time.deltaTime*10;


        if (mesafe<0.5f)
        {
            aradakiMesafeyiBirKereAl = true;
            if (aradakiMesafeSayacı==Gidileceknoktalar.Length-1)
            {
                ilerimiGeriMi = false;
            }
            else if (aradakiMesafeSayacı==0)
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
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.GetChild(i).transform.position, 1f);
        }
        for (int i = 0; i < transform.childCount-1; i++)
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
[CustomEditor(typeof(testereKontrol))]
[System.Serializable()]
 class testereKontrolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        testereKontrol script = (testereKontrol)target;
        if (GUILayout.Button("ÜRET",GUILayout.MinWidth(100),GUILayout.Width(100)))
        {
            GameObject yeniobjem = new GameObject();
            yeniobjem.transform.parent = script.transform;
            yeniobjem.transform.position = script.transform.position;
            yeniobjem.name = script.transform.childCount.ToString();
            
        }
    }
}
#endif