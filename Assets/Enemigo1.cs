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
    public float jumpVel;
    private bool jumping;
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
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
    void Update()
    {
        Vector3 v = new Vector3(transform.position.x, transform.position.y + box.size.y / 2);
        RaycastHit2D r = Physics2D.Raycast(v, Vector3.down, .5f, LayerMask.GetMask("Map"));
        if (r) rigid.velocity = new Vector2(rigid.velocity.x, 0);
        anim.SetBool("caminando", true);
        if (siguiendo == 0)movSinSeguir();
        else if (siguiendo == 1) seguirPlayer();
        else if (siguiendo == 2) seguirDibujo();
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
                                ruta[punto].transform.position.x, velocity), transform.position.y, 0);
                if (Mathf.Abs(transform.position.x - ruta[punto].transform.position.x) < velocity)
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
        
        if (r && r.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            blanco = r.collider.gameObject;
            transform.position = new Vector3(Mathf.MoveTowards(transform.position.x,
                              blanco.transform.position.x, followVelocity), transform.position.y, 0);
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
        if (Mathf.Abs(blanco.transform.position.x - transform.position.x)>followVelocity/4)
                Flip(blanco.transform.position.x < transform.position.x);
        float distancia = Vector3.Distance(transform.position, blanco.transform.position);
        Vector3 direccion = new Vector3(blanco.transform.position.x - transform.position.x,
                            blanco.transform.position.y - transform.position.y);
        RaycastHit2D r = Physics2D.Raycast(transform.position, direccion, rayLength,LayerMask.GetMask("Map","Player","Drawing"));
        if (r && r.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            blanco = r.collider.gameObject;
            siguiendo = 1;
            transform.position = new Vector3(Mathf.MoveTowards(transform.position.x,
                              blanco.transform.position.x, followVelocity), transform.position.y, 0);
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
                              blanco.transform.position.x, followVelocity), transform.position.y, 0);
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

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            siguiendo = 1;
            esperando = 0;
            blanco=col.gameObject;
        }
        else if (siguiendo != 1 && col.gameObject.layer == LayerMask.NameToLayer("Drawing"))
        {
            siguiendo = 2;
            esperando = 0;
            blanco = col.gameObject;
        }
    }
}
