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
	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        Manager.gManager.player = this;
	}
	
	// Update is called once per frame
	void Update () {
        rigid.velocity = new Vector2(Input.GetAxis("Horizontal")*velocity,rigid.velocity.y);
        enSuelo = Physics2D.BoxCast(transform.position, new Vector2(box.size.x, box.size.x), 0, new Vector2(0, -1), (box.size.y/2)+0.01f, LayerMask.GetMask("Map"));
        if (enSuelo)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                rigid.velocity = new Vector2(rigid.velocity.x, jumpVel);
            }
            if (Input.GetButtonDown("Fire3"))
            {
                drawing = true;
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
	}
    void Flip()
    {
        flipped = !flipped;
        transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, transform.localScale.z);
    }
    void InstantiateDrawing(int i)
    {
        Instantiate(drawingObj, spawner.position, Quaternion.identity);
    }
}
