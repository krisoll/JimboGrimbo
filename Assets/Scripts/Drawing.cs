using UnityEngine;
using System.Collections;

public class Drawing : MonoBehaviour {
    public int num = 0;
    public bool activated;
    public bool flipped;
    public enum DrawingType
    {
        BUTTERFLY
    }
    public DrawingType drawingType;
	// Use this for initialization
	void Start () {
        if (num == 0)
        {
            drawingType = DrawingType.BUTTERFLY;
        }
	}
	
	// Update is called once per frame
    void Update()
    {
        if (activated)
        {
            switch (drawingType)
            {
                case DrawingType.BUTTERFLY:
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
                    break;
            }
        }
    }
    void Flip()
    {
        flipped = !flipped;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}
