using UnityEngine;
using System.Collections;

public class Enemigo2 : MonoBehaviour
{
    public GameObject[] ruta;
    public GameObject limIz;
    public GameObject limDer;
    public GameObject limSup;
    public GameObject limInf;
    public int punto;
    public float espera;
    public float tiempoResignacion;
    float esperando;
    public float velocity;
    public float followVelocity;

    private bool flipped;

    public float rayLength;
    public int siguiendo;
    Animator anim;
    GameObject blanco;


	// Use this for initialization
    void Start()
    {
        esperando = espera;
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
    void Update()
    {
        if (siguiendo == 0)movSinSeguir();
        else if (siguiendo == 1) seguirPlayer();
        else if (siguiendo == 2) seguirDibujo();
	}

    void ajustarALimites()
    {
        if (limIz != null && transform.position.x < limIz.transform.position.x)
        {
            transform.position = new Vector3(limIz.transform.position.x, transform.position.y);
        }
        if (limDer != null && transform.position.x > limDer.transform.position.x)
        {
            transform.position = new Vector3(limDer.transform.position.x, transform.position.y);
        }
        if (limSup != null && transform.position.y > limSup.transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, limSup.transform.position.y);
        }
        if (limInf != null && transform.position.y < limInf.transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, limInf.transform.position.y);
        }
    }

    void movSinSeguir()
    {
        if (ruta.Length > 1)
        {
            if (esperando < espera)
            {
                esperando += Time.deltaTime;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, 
                            ruta[punto].transform.position, velocity);
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

        if (r && r.collider.gameObject.layer == LayerMask.NameToLayer("Player") &&
                        r.collider.tag.CompareTo("Hide") != 0)
        {
            blanco = r.collider.gameObject;
            transform.position = Vector3.MoveTowards(transform.position, blanco.transform.position, followVelocity);
            ajustarALimites();
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
        Vector3 direccion = new Vector3(blanco.transform.position.x - transform.position.x,
                            blanco.transform.position.y - transform.position.y);
        RaycastHit2D r = Physics2D.Raycast(transform.position, direccion, rayLength,LayerMask.GetMask("Map","Player","Drawing"));
        if (r && r.collider.gameObject.layer == LayerMask.NameToLayer("Player") &&
                        r.collider.tag.CompareTo("Hide") != 0)
        {
            blanco = r.collider.gameObject;
            siguiendo = 1;
            transform.position = Vector3.MoveTowards(transform.position, blanco.transform.position, followVelocity);
            ajustarALimites();
        }
        else if (r && r.collider.gameObject.layer == LayerMask.NameToLayer("Drawing"))
        {
            blanco = r.collider.gameObject;
            transform.position = Vector3.MoveTowards(transform.position, blanco.transform.position, followVelocity);
            ajustarALimites();
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
}
