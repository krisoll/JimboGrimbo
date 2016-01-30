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


	// Use this for initialization
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        esperando = espera;
        siguiendo = -1;

	}
	
	// Update is called once per frame
    void Update()
    {
        if (siguiendo == -1)
        {
            Vector2 dir;
            if (flipped) dir = Vector2.left;
            else dir = Vector2.right;
            RaycastHit2D r = Physics2D.Raycast(transform.position, dir, rayLength,  LayerMask.GetMask("Player"));
            if (r)
            {
                if (r.collider.tag.CompareTo("Player")==0)
                {
                    siguiendo = 0;
                    espera = 0;
                }
                else
                {
                    espera = 0;
                    siguiendo = 1;
                }
            }
            movSinSeguir();
        }
        else if (siguiendo == 0) seguirPlayer();
        else if (siguiendo == 1) seguirDibujo();
	}

    void movSinSeguir()
    {
        if (ruta.Length > 1)
        {
            if (esperando < espera) esperando += Time.deltaTime;
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
        float distancia = Vector3.Distance(transform.position, Manager.gManager.player.transform.position);
        Vector3 direccion = new Vector3(Manager.gManager.player.transform.position.x - transform.position.x,
                            Manager.gManager.player.transform.position.y - transform.position.y);
        RaycastHit2D r = Physics2D.Raycast(transform.position, direccion, rayLength, LayerMask.GetMask("Player"));
        if (!r) esperando += Time.deltaTime;
        if (esperando > tiempoResignacion)
        {
            espera = 0;
            siguiendo = -1;
            punto = 0;
            return;
        }
        if (r)
        {
            transform.position = new Vector3(Mathf.MoveTowards(transform.position.x,
                              Manager.gManager.player.transform.position.x, followVelocity), transform.position.y, 0);
            if (limIz != null && transform.position.x < limIz.transform.position.x)
                transform.position = new Vector3(limIz.transform.position.x, transform.position.y);
            if (limDer != null && transform.position.x > limDer.transform.position.x)
                transform.position = new Vector3(limDer.transform.position.x, transform.position.y);
        }
    }

    void seguirDibujo()
    {
        if (Manager.gManager.asignedPlayer == null)
        {
            siguiendo = -1;
            esperando = 0;
            punto = 0;
            return;
        }
        float distancia = Vector3.Distance(transform.position, Manager.gManager.asignedPlayer.transform.position);
        Vector3 direccion = new Vector3(Manager.gManager.asignedPlayer.transform.position.x - transform.position.x,
                            Manager.gManager.asignedPlayer.transform.position.y - transform.position.y);
        RaycastHit2D r = Physics2D.Raycast(transform.position, direccion, rayLength, LayerMask.GetMask("Player"));
        if (!r) espera += Time.deltaTime;
        else if (r.collider.tag.CompareTo("Player") == 0) siguiendo = 0;

        if (espera > tiempoResignacion)
        {
            espera = 0;
            siguiendo = -1;
            punto = 0;
            return;
        }
        transform.position = new Vector3(Mathf.MoveTowards(transform.position.x,
                                Manager.gManager.asignedPlayer.transform.position.x, followVelocity), transform.position.y, 0);
    }

    void Flip(bool b)
    {
        if(b)transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        flipped = b;
    }
}
