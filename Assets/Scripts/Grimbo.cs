using UnityEngine;
using System.Collections;

public class Grimbo : MonoBehaviour {
    public float velocity;
    public float jumpVel;
    private bool jumping;
    private Rigidbody2D rigid;
    private RaycastHit2D enSuelo;
    private BoxCollider2D box;
	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
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
        }

	}
}
