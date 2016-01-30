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
    private bool butterflied;
    private bool drawing;
    private Animator anim;
    private GameObject drawingInstance;
    private bool snapped;
    private Vector2 snapNormal;
	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        Manager.gManager.player = this;
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if(!drawing&&!snapped)rigid.velocity = new Vector2(Input.GetAxis("Horizontal")*velocity,rigid.velocity.y);
        else if (snapped) rigid.velocity = new Vector2(0, rigid.velocity.y);
        anim.SetFloat("Velocity", Mathf.Abs(rigid.velocity.x));
        enSuelo = Physics2D.BoxCast(transform.position+new Vector3(box.offset.x,box.offset.y,0), new Vector2(box.size.x, box.size.x), 0, new Vector2(0, -1), (box.size.y/2)+0.01f, LayerMask.GetMask("Map"));
        if (enSuelo)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                rigid.velocity = new Vector2(rigid.velocity.x, jumpVel);
            }
            if (Input.GetButtonDown("Fire3")&&!drawing&&drawingInstance==null)
            {
                drawing = true;
                anim.SetBool("Drawing", true);
            }
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
    void Flip()
    {
        flipped = !flipped;
        transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, transform.localScale.z);
    }
    void InstantiateDrawing(int i)
    {
        drawingInstance = (GameObject)Instantiate(drawingObj, spawner.position, Quaternion.identity);
        drawing = false;
        anim.SetBool("Drawing", false);
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Map"))
        {
            if (Mathf.Abs(coll.contacts[0].normal.x) > 0.5f)
            {
                snapped = true;
                snapNormal = coll.contacts[0].normal;
            }
        }
    }
}
