using UnityEngine;
using System.Collections;

public class Drawing : MonoBehaviour {
    public float velocity;
    public bool activated;
    public bool flipped;
    private Rigidbody2D rigid;
    public enum DrawingType
    {
        BUTTERFLY
    }
    public DrawingType drawingType;
	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody2D>();
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
}
