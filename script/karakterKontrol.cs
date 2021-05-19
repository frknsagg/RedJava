using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class karakterKontrol : MonoBehaviour
{
    public Sprite[] beklemeAnim;
    public Sprite[] ziplamaAnim;
    public Sprite[] yurumeAnim;

    public Text canText;
    public Image siyaharkaplan;
    public Text altinText;
    

    SpriteRenderer spriteRendere;

    float horizontal = 0;
    float beklemeAnimZaman = 0;
    float yurumeAnimZaman = 0;
    float siyahArkaPlanSayaci = 0;
    float anaMenuyeDonZaman = 0;

    Rigidbody2D fizik;

    Vector3 vec;
    Vector3 kameraSonPos;
    Vector3 kamerailkPos;

    bool birKerezipla = true;

    int beklemeAnimSayac=0;
    int yurumeAnimSayac=0;
    int can=20;
    int altinSayaci = 0;
    
    

    GameObject kamera;

    
    void Start()
    {
        Time.timeScale = 1;
        siyaharkaplan.gameObject.SetActive(false);
        kamera = GameObject.FindGameObjectWithTag("MainCamera");
        spriteRendere = GetComponent<SpriteRenderer>();
        fizik = GetComponent<Rigidbody2D>();

        if (SceneManager.GetActiveScene().buildIndex>PlayerPrefs.GetInt("kacincilevel"))
        {
            PlayerPrefs.SetInt("kacincilevel", SceneManager.GetActiveScene().buildIndex);
        }

        
        kamerailkPos = kamera.transform.position - transform.position;
        canText.text = "CAN    "+can;
        altinText.text = "30 - " + altinSayaci;
    }

    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            if (birKerezipla)
            {
                fizik.AddForce(new Vector2(0, 500));
                birKerezipla = false;
            }
            
        }
        

    }
    void LateUpdate()
    {
        kameraKontrol();
    }
    void FixedUpdate ()
    {
        karakterHareket();
        Animasyon();
        if (can<=0)
        {
            Time.timeScale = 0.3f;
            canText.enabled = false;
            
            siyahArkaPlanSayaci += 0.04f;
            siyaharkaplan.gameObject.SetActive(true);
            siyaharkaplan.color = new Color(0,0,0,siyahArkaPlanSayaci);
            anaMenuyeDonZaman += Time.deltaTime;
            if (anaMenuyeDonZaman>1)
            {
                SceneManager.LoadScene("anamenu");
                
            }
        }
    }
    void karakterHareket()
    {
        horizontal = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        vec = new Vector3(horizontal*10,fizik.velocity.y,0);
        fizik.velocity = vec;
    }
    void Animasyon()
    {
        if (birKerezipla)
        {
            if (horizontal == 0)
            {
                beklemeAnimZaman += Time.deltaTime;
                if (beklemeAnimZaman > 0.05f)
                {
                    spriteRendere.sprite = beklemeAnim[beklemeAnimSayac++];
                    if (beklemeAnimSayac == beklemeAnim.Length)
                    {
                        beklemeAnimSayac = 0;
                    }
                    beklemeAnimZaman = 0;
                }

            }
            else if (horizontal > 0)
            {
                yurumeAnimZaman += Time.deltaTime;
                if (yurumeAnimZaman > 0.01f)
                {
                    spriteRendere.sprite = yurumeAnim[yurumeAnimSayac++];
                    if (yurumeAnimSayac == yurumeAnim.Length)
                    {
                        yurumeAnimSayac = 0;
                    }
                    yurumeAnimZaman = 0;

                }
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (horizontal < 0)
            {
                yurumeAnimZaman += Time.deltaTime;
                if (yurumeAnimZaman > 0.01f)
                {
                    spriteRendere.sprite = yurumeAnim[yurumeAnimSayac++];
                    if (yurumeAnimSayac == yurumeAnim.Length)
                    {
                        yurumeAnimSayac = 0;
                    }
                    yurumeAnimZaman = 0;
                }
                transform.localScale = new Vector3(-1, 1, 1);
            }
            
        }
        else
        {
            if (fizik.velocity.y > 0)
            {
                spriteRendere.sprite = ziplamaAnim[0];
            }
            else
            {
                spriteRendere.sprite = ziplamaAnim[1];
            }
            if (horizontal>0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (horizontal<0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag=="kursun")
        {
            can--;
            canText.text = "CAN   " + can;
        }
        if (col.gameObject.tag=="dusman")
        {
            can =can- 10;
            canText.text = "CAN   " + can;
        }
        if (col.gameObject.tag == "testere")
        {
            can -= 10;
            canText.text = "CAN   " + can;
        }
        if (col.gameObject.tag == "levelbitsin")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }
        if (col.gameObject.tag == "canver")
        {
            can += 10;
            canText.text = "CAN   " + can;
            col.GetComponent<BoxCollider2D>().enabled = false;
            col.GetComponent<canver>().enabled = true;
            Destroy(col.gameObject,3);
            
        }
        if (col.gameObject.tag == "altin")
        {
            altinSayaci++;
            altinText.text = "30 - " + altinSayaci;
            Destroy(col.gameObject);

        }
        if (col.gameObject.tag == "su")
        {
            can = 0;
            

        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        birKerezipla = true;
    }
    
    void kameraKontrol()
    {
        kameraSonPos = kamerailkPos + transform.position;
        kamera.transform.position = Vector3.Lerp(kamera.transform.position, kameraSonPos, 0.1f);
    }
       
}
