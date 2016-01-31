using UnityEngine;
using System.Collections;

public class Enemigo1 : MonoBehaviour
{
    public GameObject[] ruta;
    public GameObject limIz;
    public GameObject limDer;
    public int punto;
    public float espera;
    public float tiempoResignacion;
    float esperando;
    public float velocity;
    public float followVelocity;


    private bool snapped;
    private Vector2 snapNormal;
    private float prevVelY;
    private Rigidbody2D rigid;
    private RaycastHit2D enSuelo;
    private BoxCollider2D box;
    private bool flipped;
    private bool butterflied;

    public float rayLength;
    public int siguiendo;
    Animator anim;
    GameObject blanco;


	// Use this for initialization
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        esperando = espera;
        //Prueba
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
    void Update()
    {
        
        anim.SetBool("caminando", true);
        if (siguiendo == 0)movSinSeguir();
        else if (siguiendo == 1) seguirPlayer();
        else if (siguiendo == 2) seguirDibujo();
        if (snapNormal.x < -0.5f && rigid.velocity.x < 0 ||
                snapNormal.x > 0.5f && rigid.velocity.x > 0)
            snapped = false;
        if(snapped) rigid.velocity = new Vector2(0, rigid.velocity.y);
        Vector3 v = new Vector3(transform.position.x, transform.position.y + box.size.y / 2);
        RaycastHit2D r = Physics2D.Raycast(v, Vector3.down, .5f, LayerMask.GetMask("Map"));
        if (r) rigid.velocity = new Vector2(rigid.velocity.x, 0);
	}

    void movSinSeguir()
    {
        if (ruta.Length > 1)
        {
            if (esperando < espera)
            {
                esperando += Time.deltaTime;
                anim.SetBool("caminando", false);
            }
            else
            {
                transform.position = new Vector3(Mathf.MoveTowards(transform.position.x,
                                ruta[punto].transform.position.x, velocity*Time.deltaTime), transform.position.y, 0);
                if (Mathf.Abs(transform.position.x - ruta[punto].transform.position.x) < velocity * Time.deltaTime)
                {
                    punto = (punto + 1) % ruta.Length;
                    esperando = 0;
                }
                else
                {
                    Flip(transform.position.x >= ruta[punto].transform.position.x);
                }
            }
        }
    }

    void seguirPlayer()
    {
        Flip(blanco.transform.position.x < transform.position.x);
        float distancia = Vector3.Distance(transform.position, blanco.transform.position);
        Vector3 direccion = new Vector3(blanco.transform.position.x - transform.position.x,
                            blanco.transform.position.y - transform.position.y);
        RaycastHit2D r = Physics2D.Raycast(transform.position, direccion, rayLength, LayerMask.GetMask("Map", "Player", "Drawing"));

        if (r && r.collider.gameObject.layer == LayerMask.NameToLayer("Player") &&
                        r.collider.tag.CompareTo("Hide") != 0)
        {
            blanco = r.collider.gameObject;
            transform.position = new Vector3(Mathf.MoveTowards(transform.position.x,
                              blanco.transform.position.x, followVelocity * Time.deltaTime), transform.position.y, 0);
            if (limIz != null && transform.position.x < limIz.transform.position.x)
            {
                transform.position = new Vector3(limIz.transform.position.x, transform.position.y);
                anim.SetBool("caminando", false);
            }
            if (limDer != null && transform.position.x > limDer.transform.position.x)
            {
                transform.position = new Vector3(limDer.transform.position.x, transform.position.y);
                anim.SetBool("caminando", false);
            }
        }
        else
        {
            esperando += Time.deltaTime;
            anim.SetBool("caminando", false);
            if (esperando > tiempoResignacion)
            {
                esperando = 0;
                siguiendo = 0;
                punto = 0;
                blanco = null;
                return;
            }
        }
    }

    void seguirDibujo()
    {
        if (blanco == null)
        {
            siguiendo = 0;
            esperando = 0;
            punto = 0;
            return;
        }
        if (Mathf.Abs(blanco.transform.position.x - transform.position.x) > followVelocity * Time.deltaTime/4)
                Flip(blanco.transform.position.x < transform.position.x);
        Vector3 direccion = new Vector3(blanco.transform.position.x - transform.position.x,
                            blanco.transform.position.y - transform.position.y);
        RaycastHit2D r = Physics2D.Raycast(transform.position, direccion, rayLength,LayerMask.GetMask("Map","Player","Drawing"));
        if (r && r.collider.gameObject.layer == LayerMask.NameToLayer("Player") &&
                        r.collider.tag.CompareTo("Hide") != 0)
        {
            blanco = r.collider.gameObject;
            siguiendo = 1;
            transform.position = new Vector3(Mathf.MoveTowards(transform.position.x,
                              blanco.transform.position.x, followVelocity * Time.deltaTime), transform.position.y, 0);
            if (limIz != null && transform.position.x < limIz.transform.position.x)
            {
                transform.position = new Vector3(limIz.transform.position.x, transform.position.y);
                anim.SetBool("caminando", false);
            }
            if (limDer != null && transform.position.x > limDer.transform.position.x)
            {
                transform.position = new Vector3(limDer.transform.position.x, transform.position.y);
                anim.SetBool("caminando", false);
            }
        }
        else if (r && r.collider.gameObject.layer == LayerMask.NameToLayer("Drawing"))
        {
            blanco = r.collider.gameObject;
            transform.position = new Vector3(Mathf.MoveTowards(transform.position.x,
                              blanco.transform.position.x, followVelocity * Time.deltaTime), transform.position.y, 0);
            if (limIz != null && transform.position.x < limIz.transform.position.x)
            {
                transform.position = new Vector3(limIz.transform.position.x, transform.position.y);
                anim.SetBool("caminando", false);
            }
            if (limDer != null && transform.position.x > limDer.transform.position.x)
            {
                transform.position = new Vector3(limDer.transform.position.x, transform.position.y);
                anim.SetBool("caminando", false);
            }
        }
        else
        {
            esperando += Time.deltaTime;
            anim.SetBool("caminando", false);
            if (esperando > tiempoResignacion)
            {
                esperando = 0;
                siguiendo = 0;
                punto = 0;
                blanco = null;
                return;
            }
        }
    }

    void Flip(bool b)
    {
        if(b)transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        flipped = b;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            Vector3 direccion = new Vector3(col.transform.position.x - transform.position.x,
                                col.transform.position.y - transform.position.y);
            RaycastHit2D r = Physics2D.Raycast(transform.position, direccion, rayLength, LayerMask.GetMask("Map", "Player", "Drawing"));
            if (r && r.collider.gameObject.layer == LayerMask.NameToLayer("Player") &&
                        r.collider.tag.CompareTo("Hide")!=0)
            {
                siguiendo = 1;
                esperando = 0;
                blanco = col.gameObject;
            }
        }
        else if (siguiendo != 1 && col.gameObject.layer == LayerMask.NameToLayer("Drawing"))
        {
            Vector3 direccion = new Vector3(col.transform.position.x - transform.position.x,
                                col.transform.position.y - transform.position.y);
            RaycastHit2D r = Physics2D.Raycast(transform.position, direccion, rayLength, LayerMask.GetMask("Map", "Player", "Drawing"));
            if (r && r.collider.gameObject.layer == LayerMask.NameToLayer("Drawing"))
            {
                siguiendo = 2;
                esperando = 0;
                blanco = col.gameObject;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            blanco = null;
            col.gameObject.GetComponent<Grimbo>().DestroyPlayer();
            anim.SetBool("agarrar", true);
            siguiendo = 0;
            esperando = -60;
        }
        else if (col.gameObject.layer == LayerMask.NameToLayer("Drawing"))
        {
            blanco = null;
            col.gameObject.GetComponent<Drawing>().DestroyDraw();
            siguiendo = 0;
            esperando = 0;
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Map"))
        {
            for (int i = 0; i < coll.contacts.Length; i++)
            {
                if (Mathf.Abs(coll.contacts[i].normal.x) > 0.5f && !snapped)
                {
                    rigid.velocity = new Vector2(0, prevVelY);
                    snapped = true;
                    snapNormal = coll.contacts[i].normal;
                }
            }
        }
    }
    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Map"))
        {
            snapped = false;
            snapNormal = Vector2.zero;
        }
    }
}
