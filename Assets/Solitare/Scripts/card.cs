using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class card : MonoBehaviour {

    //decide if this card will be auto-setup or if suit, ranck, face ordientation and card type will be set manually
    public enum my_setup
    {
        automatic,
        manual
    }
    public my_setup my_setup_selected = my_setup.automatic;

    public enum card_suit
    {
        random = -1,
        clubs = 0,
        spades = 1,
        diamonds = 2,
        hearts = 3
    }
    public card_suit card_suit_selected = card_suit.random;//decide if this card will have a random suit at each game start/restart or always the same


    public enum card_rank
    {
        random = -1,
        _A = 0,
        _2 = 1,
        _3 = 2,
        _4 = 3,
        _5 = 4,
        _6 = 5,
        _7 = 6,
        _8 = 7,
        _9 = 8,
        _10 = 9,
        _J = 10,
        _Q = 11,
        _K = 12
    }
    public card_rank card_rank_selected = card_rank.random;//decide if this card will have a random rank at each game start/restart or always the same

    //the final rank and suit of this card
    public int my_suit;
    public int my_rank;

    //card appearance, it will be auto-setup from deck script
    private Image img;
    public Sprite my_back;
    public bool shaking = false;

    public bool face_up;//the current face orientation
    public bool always_face_up;//if true, this card will be always face up, otherwise will be face up only if this card not have any card over itself
    public bool this_is_a_bottom_card;//this card not have any other card under itself (deck script use this for call bottom_card_score when this card is pick)

    public Transform[] cards_over_me;//only it is none, this card can be click

    //rotation settings for animation. The script auto-setup this
    Quaternion my_rotation_up;
    Vector3 my_scale_up;
    Quaternion my_rotation_down;
    Vector3 my_scale_down;
    public int level = 0;

    void Awake()
    {
        img = gameObject.GetComponent<Image>();
        my_rotation_up = this.transform.localRotation;
        my_scale_up = this.transform.localScale;
    }

    // Use this for initialization
    void Start ()
	{
        //StartMe ();
    }

    public void Reset()
    {
        this.gameObject.SetActive (true);
		if (!always_face_up)
		{
			face_up = false;
		}
    }
	
    public void Init(int suit, int rank)
    {
        my_suit = suit;
        my_rank = rank;
		img.sprite = deck.GetInstance().card_deck_sprite [my_suit, my_rank];
		my_back = deck.GetInstance().card_back_sprite;
        FindCardsOverMe ();
        TurnThisCard ();
    }

    void FindCardsOverMe()
    {
        //store the overlap detectors
        int total_lengh = 0;

        Bounds b = gameObject.GetComponent<BoxCollider2D>().bounds;
        Collider2D[] temp_colliders = Physics2D.OverlapBoxAll(b.center, b.size, gameObject.transform.rotation.eulerAngles.z);
        total_lengh = temp_colliders.Length;

        //check if there are cards over this one
        Transform[] temp_cards_over_me = new Transform[total_lengh];
        Transform[] temp_cards_over_me_check = new Transform[total_lengh];
        bool is_new = true;

        int temp_count = 0;
        int total = 0;

        this_is_a_bottom_card = true;

        for (int ii = 0; ii < temp_colliders.Length; ii++)
        {
            int otherLevel = temp_colliders[ii].gameObject.GetComponent<card>().level;
            //Debug.Log(temp_colliders[ii].transform.position.z + " " + this.transform.position.z);
            //Debug.Log(otherLevel + " " + this.level);
            //if (temp_colliders[ii].transform.position.z < this.transform.position.z)//there is a card over me
            if(otherLevel > this.level)
            {
                is_new = true;

                for (int c = 0; c < temp_cards_over_me_check.Length; c++)
                {
                    if (temp_cards_over_me_check[c] == temp_colliders[ii].transform)
                    {
                        is_new = false;
                    }
                }

                if (is_new)
                {
                    temp_cards_over_me[temp_count] = temp_colliders[ii].transform;
                    temp_cards_over_me_check[temp_count] = temp_colliders[ii].transform;
                    total++;
                }
            }
            //else if (temp_colliders[ii].transform.position.z > this.transform.position.z)//there is a card under me
            else if (otherLevel < this.level)//there is a card under me
            {
                this_is_a_bottom_card = false;
            }
            temp_count++;
        }

        //save card found in an array in order to know when the player will pick all the card above this one
        cards_over_me = new Transform[total];

        temp_count = 0;
        for (int i = 0; i < total_lengh; i++)
        {
            if (temp_cards_over_me[i] != null)
            {
                cards_over_me[temp_count] = temp_cards_over_me[i];
                temp_count++;
            }
        }
    }


    void OnMouseDown()
    {
		if (deck.GetInstance().player_can_move && Input.touches.Length <= 1)//if the palyer click or tap on this card
        {
            //Debug.Log ("S"+my_suit + "R" + my_rank + " * up = " + face_up + " * free = " +This_card_is_free());
            if (ThisCardIsFree()) //if this card is free
            {
                //check if this card can became the new current card
                if (my_rank == 0)
                {
					if (deck.GetInstance().current_card_rank == 12 || (deck.GetInstance().current_card_rank == (my_rank + 1)))
                        ThisIsTheNewCurrentCard ();
                    else
                        WrongCard();
                } 
                else if (my_rank == 12)
                {
					if (deck.GetInstance().current_card_rank == 0 || (deck.GetInstance().current_card_rank == (my_rank - 1)))
						ThisIsTheNewCurrentCard ();
                    else
                        WrongCard();
                }
                else
                {
					if ( (deck.GetInstance().current_card_rank == (my_rank + 1)) || (deck.GetInstance().current_card_rank == (my_rank - 1)) )
						ThisIsTheNewCurrentCard ();
                    else
                        WrongCard();
                }
            } 
            else
                BlockedCard();//this card have another card over itself
        }
    }

    public bool ThisCardIsFree()//check if the card not have others card over itself anymore
    {
        bool return_this = true;

        if (cards_over_me.Length == 0)//this card neved had other card over itself
            return_this = true;
        else
        {
            for (int i = 0; i < cards_over_me.Length; i++)
            {
                if (cards_over_me[i]!= null)//there is a card over me
                {
                    if (cards_over_me[i].gameObject.activeSelf == true)//in it is active
                    {
                        return_this = false;//I found a card over me, so player can't pick me
                        break;
                    }
                }
            }
        }
        return return_this;
    }

    public bool TurnThisCard()
    {
        //turn up random cards at start

        bool wasFaceUp = face_up;

        if (always_face_up) //turn up card set up manually
        {
            face_up = always_face_up;
        }
        else //turn up only top card
        {
            face_up = ThisCardIsFree();
        }

        if (face_up)
        {
			if (deck.GetInstance().game_started) // All cards are face up by default
            {
                StartCoroutine(Rotate(my_rotation_up, my_scale_up, null)); //rotate face up

                if (!wasFaceUp)
                {
                    return true;
                }
            }
        }
        else //rotate face down
        {
            if (transform.rotation.y ==  my_rotation_up.y)
            {
				if (deck.GetInstance().game_started)
                {
                    StartCoroutine(Rotate(my_rotation_down,my_scale_down,my_back)); //with animation because this card return turn down due an UNDO
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
        }

        return false;

    }

    IEnumerator Rotate(Quaternion end_point_rotation, Vector3 end_point_scale, Sprite end_point_sprite)//the rotate animation
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

    void ThisIsTheNewCurrentCard()//put this card in the target deck
    {
        //put this card sprite on the ghost card used to show the card movement from board to target deck
        deck.GetInstance().ghost_card.GetComponent<Image>().sprite = deck.GetInstance().card_deck_sprite [my_suit, my_rank];

        gameObject.SetActive (false);

		deck.GetInstance().StartCoroutine (deck.GetInstance().MoveTo (this.gameObject,transform,deck.GetInstance().target_new_card.transform,false));//show the card movement from board to target deck (it is an illusion, this card just became inactive and the ghost card move from this card position to target deck position)
		deck.GetInstance().StartCoroutine(deck.GetInstance().NewCurrentCard (my_suit, my_rank, false, false));//set this card as new top card on the target deck


		int base_score = deck.GetInstance().normal_card_score;
        int combo_score = 0;

        // Create visual score for this card now.
		deck.GetInstance().CreateScoreEvent(this.transform.position, base_score.ToString(deck.GetInstance().scoreFormat), deck.GetInstance().current_score  + base_score);

        // Check combo
		if (deck.GetInstance().combo_enabled)
        {
			combo_score = deck.GetInstance().AddCombo();

            if (combo_score != 0)
            {
                // Combo event
				deck.GetInstance().StartCoroutine(deck.GetInstance().ComboScore(combo_score));
            }
        }

        base_score += combo_score;
       
		if (deck.GetInstance ().current_total_card_on_board > 0)
		{//if there are cards left
			// Rotate, create visual score for exposed cards and add real score.
			deck.GetInstance ().StartCoroutine (deck.GetInstance ().RefreshCardRotationAndAddScore (gameObject, base_score));
		}	
    }


    void BlockedCard()
    {
        //Debug.Log ("this card have something over it");
    }

    void WrongCard()
    {
        //Debug.Log ("wrong card rank");
        MenuManager.GetInstance().PlaySound(MenuManager.SoundNamesEnum.ERROR_SOUND);
		StartCoroutine(deck.GetInstance().ShakeCard(this));
    }

}
