using UnityEngine;
using System.Collections;

public class Drawing : MonoBehaviour {
    public float velocity;
    public bool activated;
    public bool flipped;
    private Rigidbody2D rigid;
    private RaycastHit2D colPlayer;
    private BoxCollider2D box;
    public enum DrawingType
    {
        NONE,
        BUTTERFLY
    }
    public DrawingType drawingType;
	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
    void Update()
    {
        if (!activated) return;
        if (activated)
        {
            switch (drawingType)
            {
                case DrawingType.BUTTERFLY:
                    rigid.velocity = new Vector2(Input.GetAxis("Horizontal") * velocity, Input.GetAxis("Vertical") * velocity);
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
                    if (Input.GetButtonDown("Fire1") && !Manager.gManager.player.switched)
                    {
                        setActive(false);
                        Manager.gManager.player.setActive(true);
                        Manager.gManager.asignedPlayer = Manager.gManager.player.gameObject;
                    }
                    colPlayer = Physics2D.BoxCast(transform.position, box.size, 0, Vector2.zero, 1, LayerMask.GetMask("Player"));
                    if (Input.GetButtonDown("Fire2")&&colPlayer)
                    {
                        Manager.gManager.player.setActive(true);
                        Manager.gManager.asignedPlayer = Manager.gManager.player.gameObject;
                        Manager.gManager.player.empower(1);
                        Destroy(this.gameObject);
                    }
                    if (Manager.gManager.player.switched)
                    {
                        Manager.gManager.player.switched = false;
                    }
                    break;
            }
        }
    }
    void Flip()
    {
        flipped = !flipped;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
    public void setActive(bool a)
    {
        activated = a;
        if (!a) rigid.velocity = new Vector2(0, 0);
    }
    void DestroyDraw()
    {
        Manager.gManager.player.setActive(true);
        Manager.gManager.asignedPlayer = Manager.gManager.player.gameObject;
        Destroy(this.gameObject);
    }
}
