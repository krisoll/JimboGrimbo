using UnityEngine;
using System.Collections;

public class Grimbo : MonoBehaviour {
    public float velocity;
    public float jumpVel;
    public GameObject drawingObj;
    public Transform spawner;
    private bool jumping;
    private Rigidbody2D rigid;
    private RaycastHit2D enSuelo;
    private BoxCollider2D box;
    private bool flipped;
    private bool empowered;
    private bool drawing;
    private Animator anim;
    private GameObject drawingInstance;
    private Drawing drawingScr;
    private bool snapped;
    private Vector2 snapNormal;
    private float prevVelY;
    public bool activated = true;
    public bool switched = false;
    private bool action;
    Drawing.DrawingType draw;
	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        Manager.gManager.player = this;
        Manager.gManager.asignedPlayer = this.gameObject;
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        anim.SetFloat("Velocity", Mathf.Abs(rigid.velocity.x));
        anim.SetFloat("VelocidadCaida", rigid.velocity.y);
        if (!activated) return;
        if (draw == Drawing.DrawingType.NONE)
        {
            if (!drawing && !snapped) rigid.velocity = new Vector2(Input.GetAxis("Horizontal") * velocity, rigid.velocity.y);
            else if (snapped) rigid.velocity = new Vector2(0, rigid.velocity.y);
            if (enSuelo)
            {
                if (Input.GetButtonDown("Fire3") && !drawing)
                {
                    if (drawingInstance != null)
                    {
                        Destroy(drawingInstance);
                    }
                    drawing = true;
                    anim.SetBool("Drawing", true);
                }
                if (Input.GetButtonDown("Fire1") && drawingInstance != null)
                {
                    setActive(false);
                    drawingScr.setActive(true);
                    Manager.gManager.asignedPlayer = drawingInstance;
                    switched = true;
                }
            }
        }
        else if (draw == Drawing.DrawingType.BUTTERFLY)
        {
            if (!drawing && !snapped && !action) rigid.velocity = new Vector2(Input.GetAxis("Horizontal") * velocity, rigid.velocity.y);
            else if (snapped && !action) rigid.velocity = new Vector2(0, rigid.velocity.y);
            else if (action&&rigid.velocity.y<=-1.2f) rigid.velocity = new Vector2(Input.GetAxis("Horizontal") * velocity, -1.2f);
            if (Input.GetButtonDown("Fire3"))
            {
                action = true;
                anim.SetBool("Action", action);
            }
            if (Input.GetButtonUp("Fire3"))
            {
                action = false;
                anim.SetBool("Action", action);
            }
        }
        enSuelo = Physics2D.BoxCast(transform.position+new Vector3(box.offset.x,box.offset.y,0), new Vector2(box.size.x, box.size.x), 0, new Vector2(0, -1), (box.size.y/2)+0.01f, LayerMask.GetMask("Map"));
        if (enSuelo)
        {
            if (!anim.GetBool("EnSuelo")) anim.SetBool("EnSuelo", true);
            if (Input.GetAxis("Vertical")>0.01f&&!drawing)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, jumpVel);
            }
        }
        else
        {
            if (anim.GetBool("EnSuelo")) anim.SetBool("EnSuelo", false);
        }
        if (!flipped)
        {
            if (Input.GetAxis("Horizontal") < -0.01f)
            {
                Flip();
            }
        }
        else
        {
            if (Input.GetAxis("Horizontal") > 0.01f)
            {
                Flip();
            }
        }
        if (snapNormal.x < -0.5f && Input.GetAxis("Horizontal") < -0.01f ||
            snapNormal.x > 0.5f && Input.GetAxis("Horizontal") > 0.01f)
            snapped = false;
        
	}
    void FixedUpdate()
    {
        prevVelY = rigid.velocity.y;
    }
    void Flip()
    {
        flipped = !flipped;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
    void InstantiateDrawing(int i)
    {
        drawingInstance = (GameObject)Instantiate(drawingObj, spawner.position, Quaternion.identity);
        drawing = false;
        anim.SetBool("Drawing", false);
        drawingScr = drawingInstance.GetComponent<Drawing>();
        drawingScr.setActive(true);
        setActive(false);
        Manager.gManager.asignedPlayer = drawingInstance;
    }
    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Map"))
        {
            for (int i = 0; i < coll.contacts.Length; i++)
            {
                if (Mathf.Abs(coll.contacts[i].normal.x) > 0.5f&&!snapped)
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
    public void setActive(bool a)
    {
        activated = a;
        if(!a)rigid.velocity = new Vector2(0, 0);
    }
    public void empower(int a)
    {
        if (a == 1)
        {
            draw = Drawing.DrawingType.BUTTERFLY;
            empowered = true;
        }
        anim.SetInteger("Power", a);
    }
    public void DestroyPlayer()
    {
        Manager.gManager.player.setActive(true);
        Manager.gManager.asignedPlayer = Manager.gManager.player.gameObject;
        Destroy(this.gameObject);
    }
}
