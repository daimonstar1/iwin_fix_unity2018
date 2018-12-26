using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class kcard : MonoBehaviour {

    //the final rank and suit of this card
    public int suit;
    public int rank;
    public int tableauId;
    public bool shaking = false;

    private Image img;
    public Sprite my_back;

    //rotation settings for animation. The script auto-setup this
    Quaternion my_rotation_up;
    Vector3 my_scale_up;
    Quaternion my_rotation_down;
    Vector3 my_scale_down;

    public BoxCollider2D ccollider;

    public bool selfDestroy = false;

	// Use this for initialization
	void Start () 
    {
       
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    public void Init()
    {
        img = gameObject.GetComponent<Image>();

        my_rotation_up = this.transform.localRotation;
        my_scale_up = this.transform.localScale;

        ccollider = gameObject.GetComponent<BoxCollider2D>();
    }

    public void EnableCollider(bool e)
    {
        ccollider.enabled = e;
    }

    public void SetFace(bool up, bool anim)
    {
        if (up)
        {
            if (anim) // All cards are face up by default
            {
                StartCoroutine(Rotate_me(my_rotation_up, my_scale_up, null)); //rotate face up

                /*if (!wasFaceUp)
                {
                    return true;
                }*/
            }

            ccollider.enabled = true;
        }
        else //rotate face down
        {
            if (transform.rotation.y ==  my_rotation_up.y)
            {
                if (anim)
                {
                    StartCoroutine(Rotate_me(my_rotation_down,my_scale_down,my_back)); //with animation because this card return turn down due an UNDO
                }
                else //automatically at game start
                {   
                    transform.Rotate (my_rotation_up.x, my_rotation_up.y + 180, my_rotation_up.z, Space.Self);
                    transform.localScale = new Vector3(-1.0F, 1.0F, 0);// .Set(-1, transform.localScale.y, transform.localScale.z);
                    my_rotation_down = this.transform.localRotation;
                    my_scale_down = this.transform.localScale;
                    img.overrideSprite = my_back;
                }
            }

            ccollider.enabled = false;
        }
    }

    IEnumerator Rotate_me(Quaternion end_point_rotation, Vector3 end_point_scale, Sprite end_point_sprite)//the rotate animation
    {
        Quaternion start_point_rotation = this.transform.rotation;
        float duration = 0.25f;
        float time = 0;

        while (time < 1)
        {
            float dtime = Time.smoothDeltaTime / duration;
            if (time < 0.5F && time + dtime >= 0.5F)
            {
                transform.localScale = end_point_scale;
                img.overrideSprite = end_point_sprite;
            }

            time += dtime;
            transform.rotation = Quaternion.Lerp(start_point_rotation,end_point_rotation,time);



            yield return null;
        }
    }

    void OnMouseUpAsButton()
    {
        kdeck.GetInstance().OnCardClicked(this);
    }

	void OnMouseDown()
	{
		kdeck.GetInstance().OnCardDrag(this);
	}

    void OnMouseUp()
    {   
        {
            kdeck.GetInstance().OnCardDrop(this);
        }
    }

}
