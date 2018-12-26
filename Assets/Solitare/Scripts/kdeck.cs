using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

#pragma warning disable 0219

public class kdeck : MonoBehaviour
{
  static kdeck instance = null;

  public Sprite[,] card_deck_sprite;

  //black
  public Sprite[] clubs = new Sprite[13];
  public Sprite[] spades = new Sprite[13];

  //red
  public Sprite[] diamonds = new Sprite[13];
  public Sprite[] hearts = new Sprite[13];

  public Sprite card_back_sprite;

  public bool editor_show_sprites;
  public bool editor_show_sprites_normal_clubs;
  public bool editor_show_sprites_normal_spades;
  public bool editor_show_sprites_normal_diamonds;
  public bool editor_show_sprites_normal_hearts;

  public Transform boardPile0;
  public Transform boardPile1;
  public Transform boardPile2;
  public Transform boardPile3;
  public Transform boardPile4;
  public Transform boardPile5;
  public Transform boardPile6;
  public Transform[] boardPiles = new Transform[7];

  public Transform foundation_clubs;
  public Transform foundation_spades;
  public Transform foundation_hearts;
  public Transform foundation_diamonds;
  public Transform[] foundations = new Transform[4];
  int current_score;
  public bool game_end;

  public Transform ghostScoreFinalPos;

  public Image GhostWasteActive_Image;
  public Transform board;

  public int CAce_Board_To_Foundation;
  public int C2_Board_To_Foundation;
  public int C3_Board_To_Foundation;
  public int C4_Board_To_Foundation;
  public int C5_Board_To_Foundation;
  public int C6_Board_To_Foundation;
  public int C7_Board_To_Foundation;
  public int C8_Board_To_Foundation;
  public int C9_Board_To_Foundation;
  public int C10_Board_To_Foundation;
  public int CJ_Board_To_Foundation;
  public int CQ_Board_To_Foundation;
  public int CK_Board_To_Foundation;
  public int Exposed_Card;
  public int Waste_To_Foundation;
  public int Waste_To_Board;
  public int Foundation_To_Board;

  public Image waste_card0;
//animation from deck to target deck
  public Image waste_card1;
//animation from deck to target deck
  public Image waste_card2;
//animation from deck to target deck

  Image[] waste_cards = new Image[3];

  // Deck to waste
  public Animation new_card_anim0;
//animation from deck to target deck
  public Animation new_card_anim1;
//animation from deck to target deck
  public Animation new_card_anim2;
//animation from deck to target deck

  //public float new_card_anim_length0;//animation from deck to target deck
  //public float new_card_anim_length1;//animation from deck to target deck
  //public float new_card_anim_length2;//animation from deck to target deck

  public Image new_card_anim_sprite0;
//animation from deck to target deck
  public Image new_card_anim_sprite1;
//animation from deck to target deck
  public Image new_card_anim_sprite2;
//animation from deck to target deck

  // Waste to waste
  public Animation waste_card_anim1;
//animation from deck to target deck
  public Animation waste_card_anim2;
//animation from deck to target deck

  public Image waste_card_anim_sprite1;
//animation from deck to target deck
  public Image waste_card_anim_sprite2;
//animation from deck to target deck

  public float animationSpeedTime;
//animation from deck to target deck
  //public float waste_card_anim_length2;//animation from deck to target deck

  public bool player_can_move;
  private int deck_card_left = 0;
  public float bonus_time;
  public float left_time;
  public int int_time;
  public bool timer_paused = false;
  public string scoreFormat = "N0";

  public Text final_score;
  public Text final_total_score;
  public Text final_bonus_score;
  public Text bonus_text;
  public Text score_text;

  public float show_win_or_lose_screen_after_delay;
  public float hide_win_or_lose_screen_after_delay;
  private float final_score_grow_time = 1.5F;
  float popup_animation_duration = 0.25f;

  public Button undo_button;

  public float tableau_back_offset;
  public float tableau_normal_offset;

  public int deckResetCount = 0;

  float timeBetwenDeckCardAnimations = 0.06f;

  int nextSuit = 0;

  public class UndoInfo
  {
    public List<card_class> undo_cards = new List<card_class> ();
    public int undo_previous_score = 0;
  }

  public List<UndoInfo> undo_list = new List<UndoInfo> ();
  public int maxUndoSteps;

  public enum card_foundation
  {
    deck = 0,
    waste,
    waste_active,
    tableau0,
    tableau1,
    tableau2,
    tableau3,
    tableau4,
    tableau5,
    tableau6,
    foundation_clubs,
    foundation_spades,
    foundation_diamonds,
    foundation_hearts
  }

  [System.Serializable]
  public class card_class
  {
    public int suit = -99;
    public int rank = -99;
    public card_foundation foundation = card_foundation.deck;
    public int wasteActivePos = -1;
    public kcard tableauKCard = null;
    public bool tableauFaceUp = false;
    public bool tableauPeek = false;
    public int tableauIndex = -1;
    public bool cardWasInFoundation = false;
    public bool destroyGOOnUndo = false;
    public float oldOffset = 0;
  }

  [System.Serializable]
  public class dragInfo
  {
    public Vector3 initialPos;
    public int index = 0;
  }

  List<dragInfo> draggedCards = new List<dragInfo> ();

  card_class[] deck_cards;


  public static kdeck GetInstance ()
  {
    return instance;
  }

  void Awake ()
  {
    instance = this;
    left_time = bonus_time;
    int_time = -1;
    timer_paused = false;

    Generate_card_deck_sprite ();

    boardPiles [0] = boardPile0;
    boardPiles [1] = boardPile1;
    boardPiles [2] = boardPile2;
    boardPiles [3] = boardPile3;
    boardPiles [4] = boardPile4;
    boardPiles [5] = boardPile5;
    boardPiles [6] = boardPile6;

    foundations [0] = foundation_clubs;
    foundations [1] = foundation_spades;
    foundations [2] = foundation_diamonds;
    foundations [3] = foundation_hearts;

    waste_cards [0] = waste_card0;
    waste_cards [1] = waste_card1;
    waste_cards [2] = waste_card2;

    GhostWasteActive_Image.gameObject.SetActive (false);

    GetComponent<Image> ().sprite = card_back_sprite;
    new_card_anim0.transform.GetChild (0).transform.GetChild (0).GetComponent<Image> ().sprite = card_back_sprite;
    new_card_anim1.transform.GetChild (0).transform.GetChild (0).GetComponent<Image> ().sprite = card_back_sprite;
    new_card_anim2.transform.GetChild (0).transform.GetChild (0).GetComponent<Image> ().sprite = card_back_sprite;

    //new_card_anim_length0 = new_card_anim0.GetClip("new_card_anim").length;
    //new_card_anim_length1 = new_card_anim1.GetClip("new_card_anim").length;
    //new_card_anim_length2 = new_card_anim2.GetClip("new_card_anim").length;

    animationSpeedTime = waste_card_anim1.GetClip ("deck_card_1_to_0").length;
    //waste_card_anim_length2 = waste_card_anim2.GetClip("deck_card_2_to_0").length;

    deck_card_left = 52;
    Generate_card_deck_sprite ();
  }

  // Use this for initialization
  void Start ()
  {
    if (KlondikeMenu.GetInstance ().endDialog_moveAnimator.gameObject.activeSelf)
      popup_animation_duration = KlondikeMenu.GetInstance ().endDialog_moveAnimator.GetCurrentAnimatorClipInfo (0).Length;
    player_can_move = false;
    left_time = bonus_time;
    int_time = -1;
    timer_paused = true;
    animationsCount = 0;
    deckResetCount = 0;
    nextSuit = 0;

    current_score = 0;
    undo_list.Clear ();
    game_end = false;
    score_text.text = current_score.ToString ();

    undo_button.gameObject.SetActive (false);

    PrintTime ();


    int val = PlayerData.GetInstance ().GetValueAsInt ("klondikeHTP");
    if (val == 0) {
      KlondikeMenu.GetInstance ().OpenPopup (2);
    } else {
      StartCoroutine (StartGame (0.2F));
    }
  }

  public IEnumerator StartGame (float initWaitTime = 0.0F)
  {
    player_can_move = false;
    timer_paused = true;
    animationsCount++;
    yield return new WaitForSeconds (initWaitTime);

    float duration = 0.07f;

    int movedCards = Fill_deck (duration);

    UpdateDeckBackImage ();

    yield return new WaitForSeconds (duration * movedCards);

    PlayDeckToWasteActive (false);

    animationsCount--;
    if (!KlondikeMenu.GetInstance ().pauseDialog_cg.interactable) {
      timer_paused = false;
    }
    CheckPlayerCanMove ();

    //DebugWin();
  }

  /*void DebugWin()
	{
		int wap = 0;
		for (int x = 0; x < deck_card_left; x++)
		{
			if ( IsOnTableau(x) )
			{
				deck_cards[x].tableauFaceUp = true;
				//deck_cards[x].tableauPeek = true;
				deck_cards[x].tableauKCard.SetFace(true, true);

				//if (wap >0 )
                //{
                //    wap--;
                //}
			}
			else if(deck_cards[x].foundation != card_foundation.waste_active)
			{

				deck_cards[x].tableauIndex = -1;
				deck_cards[x].tableauPeek = false;
				deck_cards[x].foundation = card_foundation.waste_active;
				deck_cards[x].wasteActivePos = wap;
				deck_cards[x].tableauFaceUp = true;
				deck_cards[x].cardWasInFoundation = false;
			}
		}

		IsGameCompleted();
	}*/

  int Fill_deck (float duration)//decide what card put in the deck, but some can change if give_an_useful_card_not_less_than_n_new_card_from_the_deck is trigger
  {
    deck_cards = new card_class[deck_card_left];

    List<int> suits = new List<int> ();
    for (int x = 0; x < 4; x++) {
      suits.Add (x);
    }

    List<int>[] ranks = new List<int>[4];
    for (int j = 0; j < 4; j++) {
      ranks [j] = new List<int> ();
      for (int k = 0; k < 13; k++) {
        ranks [j].Add (k);
      }
    }

    /*for (int x = 0; x < 100; x++)
		{
				Debug.Log(Util.Mod(x, 3));
		}*/


    // Fill deck
    for (int i = 0; i < deck_card_left; i++) {
      deck_cards [i] = new card_class ();
      int sindex = Util.GetRandomBetween0And (suits.Count - 1);
      int s = suits [sindex];

      int rindex = Util.GetRandomBetween0And (ranks [s].Count - 1);
      int r = ranks [s] [rindex];

      ranks [s].RemoveAt (rindex);
      if (ranks [s].Count == 0) {
        suits.RemoveAt (sindex);
      }

      /*deck_cards[i] = new card_class();
			int sindex =  Util.GetRandomBetween0And(ranks.Count-1);
			int rindex =  Util.GetRandomBetween0And(ranks[s].Count-1);
			deck_cards[i].suit = s;
			deck_cards[i].rank = r;*/

      /*deck_cards[i] = new card_class ();
			deck_cards[i].suit = Util.GetRandomBetween0And(3);
			deck_cards[i].rank = Util.GetRandomBetween0And(12);*/


      deck_cards [i].suit = s;
      deck_cards [i].rank = r;
      deck_cards [i].foundation = card_foundation.deck;
      deck_cards [i].wasteActivePos = -1;
      deck_cards [i].tableauKCard = null;
      deck_cards [i].tableauFaceUp = false;
      deck_cards [i].tableauPeek = false;
      deck_cards [i].tableauIndex = -1;
      deck_cards [i].cardWasInFoundation = false;
      deck_cards [i].destroyGOOnUndo = false;

    }


    // Set size of board piles
    /*int[] tableauData = new int[7] {1,2,3,4,5,6,7};
		int maxCards = GetMaxCardsOnAnyColumn(tableauData);

		// Fill board
		int movedCards = 0;
		for (int x = 0; x < tableauData.Length; x++)
		{
				for (int c = 0; c < tableauData[x]; c++)
				{
						// Change pile
						deck_cards[movedCards].foundation = (card_foundation)x+3;
						deck_cards[movedCards].tableauIndex = c; 

						// Create visual card and link it.
						if (c < tableauData[x]-1)
						{
								// Not peek
								deck_cards[movedCards].tableauFaceUp = false;
								deck_cards[movedCards].tableauPeek = false;
						}
						else
						{
								// Peek
								deck_cards[movedCards].tableauFaceUp = true;
								deck_cards[movedCards].tableauPeek = true;
						}

						Vector3 initial = board.transform.InverseTransformPoint(this.gameObject.transform.position);
						Vector3 end = new Vector3(boardPiles[x].localPosition.x, boardPiles[x].localPosition.y - tableau_back_offset * c,  -deck_cards[movedCards].tableauIndex);
						deck_cards[movedCards].tableauKCard = CreateCard(deck_cards[movedCards].suit, deck_cards[movedCards].rank, deck_cards[movedCards].tableauFaceUp, board.transform, initial);

						float duration = 1.0F;
						float totalPreviousRowTime =  (maxCards - GetCardsOnRow(tableauData, c )) * duration + (x*duration) ;
						Debug.Log(totalPreviousRowTime + " para (" + x + "," + c + ")");
						//float currentPreviousRowTime = (maxCards - GetCardsOnRow(tableauData, c-1 < 0 ? 0 : c-1 )) * duration ;
						StartCoroutine(MoveCardTo(deck_cards[movedCards].tableauKCard,initial,end, duration, totalPreviousRowTime  + duration  ));



						movedCards++;
				}
		}*/

    int maxColumns = 7;
    int columns = maxColumns;
    int movedCards = 0;
    for (int r = 0; r < 7; r++) {
      for (int c = columns - 1; c >= 0; c--) {
        int pileIndex = maxColumns - 1 - c;
        // Set pile
        deck_cards [movedCards].foundation = (card_foundation)(pileIndex + 3);
        deck_cards [movedCards].tableauIndex = r;

        // Create visual card and link it.
        if (c == columns - 1) {
          // Peek
          deck_cards [movedCards].tableauFaceUp = true;
          deck_cards [movedCards].tableauPeek = true;
        } else {
          // Not peek
          deck_cards [movedCards].tableauFaceUp = false;
          deck_cards [movedCards].tableauPeek = false;
        }

        Vector3 initial = board.transform.InverseTransformPoint (this.gameObject.transform.position);
        Vector3 end = new Vector3 (boardPiles [pileIndex].localPosition.x, boardPiles [pileIndex].localPosition.y - tableau_back_offset * r, -deck_cards [movedCards].tableauIndex);
        deck_cards [movedCards].tableauKCard = CreateCard (deck_cards [movedCards].suit, deck_cards [movedCards].rank, deck_cards [movedCards].tableauFaceUp, board.transform, initial);
        deck_cards [movedCards].tableauKCard.gameObject.SetActive (false);


        StartCoroutine (MoveCardTo (deck_cards [movedCards].tableauKCard, initial, end, duration, movedCards * duration, MenuManager.SoundNamesEnum.NEW_CARD_SOUND));

        movedCards++;
      }

      columns--;
    }

    return movedCards;

  }

  int GetMaxCardsOnAnyColumn (int[] tableauData)
  {
    int maxCards = 0;
    for (int i = 0; i < tableauData.Length; i++) {
      if (tableauData [i] > maxCards) {
        maxCards = tableauData [i];
      }
    }

    return maxCards;
  }

  int GetCardsOnRow (int[] tableauData, int r)
  {
    int cards = 0;
    for (int i = 0; i < tableauData.Length; i++) {
      int nval = Mathf.Max (0, tableauData [i] - r);
      if (nval > cards) {
        cards = nval;
      }
    }

    return cards;
  }

  kcard CreateCard (int suit, int rank, bool faceUp, Transform parent, Vector3 pos)
  {

    GameObject gkcard = (GameObject)Instantiate (Resources.Load ("Prefabs/KCard"), parent);
    gkcard.GetComponent<Image> ().sprite = card_deck_sprite [suit, rank];
    gkcard.transform.localPosition = pos;
    //Debug.Log(gkcard.transform.localPosition.ToString());
    //gkcard.transform.localRotation = pos.localRotation;
    gkcard.transform.localScale = new Vector3 (1.0F, 1.0F, 1.0F);
    kcard _kcard = gkcard.GetComponent<kcard> ();
    _kcard.my_back = this.card_back_sprite;
    _kcard.Init ();
    _kcard.SetFace (faceUp, false);

    return _kcard;
  }

  void Generate_card_deck_sprite ()//fuse the card sprite arrays in one
  {
    card_deck_sprite = new Sprite[4, 13];
    Sprite[] temp_rank = new Sprite[13];

    for (int suit = 0; suit < 4; suit++) {

      if (suit == 0) {
        temp_rank = clubs;
      } else if (suit == 1) {
        temp_rank = spades;
      } else if (suit == 2) {
        temp_rank = diamonds;
      } else if (suit == 3) {
        temp_rank = hearts;
      }

      for (int rank = 0; rank < 13; rank++) {
        card_deck_sprite [suit, rank] = temp_rank [rank];
      }
    }
  }

  int[] GetAvailabeCardsOnDeck (ref int count)
  {
    count = 0;
    int[] availableDeckCards = new int[3];

    for (int i = 0; i < deck_card_left; i++) {
      if (deck_cards [i].foundation == card_foundation.deck) {
        if (count < 3) {
          availableDeckCards [count] = i;

          count++;
        } else {
          break;
        }
      }
    }

    return availableDeckCards;
  }

  string CardString (card_class card)
  {
    string cad = "Suit: ";
    switch (card.suit) {
    case 0:
      {
        cad += "Clubs";
        break;
      }
    case 1:
      {
        cad += "Spades";
        break;
      }
    case 2:
      {
        cad += "Diamonds";
        break;
      }
    case 3:
      {
        cad += "Hearts";
        break;
      }

    }

    cad += " Rank: ";

    switch (card.rank) {
    case 0:
      {
        cad += "A";
        break;
      }
    case 1:
    case 2:
    case 3:
    case 4:
    case 5:
    case 6:
    case 7:
    case 8:
    case 9:
      {
        cad += (card.rank + 1).ToString ();
        break;
      }
    case 10:
      {
        cad += "J";
        break;
      }
    case 11:
      {
        cad += "Q";
        break;
      }
    case 12:
      {
        cad += "K";
        break;
      }
    }

    cad += " Foundation: " + card.foundation.ToString ();

    cad += " Tableau Index: " + card.tableauIndex.ToString ();

    if (card.tableauKCard) {
      cad += " Zindex: " + card.tableauKCard.transform.localPosition.z;
    }

    cad += " Peek: " + card.tableauPeek;

    return cad;
  }

  void PrintDeckCardsArray ()
  {
    string cad = "";
    for (int i = 0; i < deck_card_left; i++) {
      cad += " " + (int)deck_cards [i].foundation;
    }

    string cad2 = "";
    /*for (int i = 0; i < undo_cards.Count; i++)
		{
				cad2 += " " + (int)undo_cards[i].foundation;
		}*/

    Debug.Log (cad);
    Debug.Log (cad2);
  }

  int[] GetActiveCardsOnWaste (ref int count)
  {
    count = 0;
    int[] activeCards = new int[3];

    for (int i = 0; i < deck_card_left && count < 3; i++) {
      if (deck_cards [i].foundation == card_foundation.waste_active) {
        activeCards [count] = i;
        count++;
        if (count == 4) {
          Debug.Log ("ERROR");
        }
      }
    }

    return activeCards;
  }

  int GetCard (int suit, int rank)
  {
    for (int i = 0; i < deck_card_left; i++) {
      if (deck_cards [i].suit == suit && deck_cards [i].rank == rank) {
        return i;
      }
    }

    return -1;
  }

  public IEnumerator MoveGhostCardTo (wasteActive wa, Vector3 end_point, float duration = 0.08f, MenuManager.SoundNamesEnum sound = MenuManager.SoundNamesEnum.NUM_SOUND)//move the ghost card
  {
    player_can_move = false;
    animationsCount++;

    // Move to top

    GhostWasteActive_Image.transform.SetAsLastSibling ();

    //int si = go.transform.GetSiblingIndex();
    //ghost_card.gameObject.transform.SetParent(go.transform.parent);
    //ghost_card.gameObject.transform.SetSiblingIndex(si);

    //_kcard.transform.localPosition.z
    Vector3 start_point = GhostWasteActive_Image.transform.localPosition;
    //go.transform.rotation = start_point.rotation;

    //animation
    float time = 0;
    bool playedSound = false;
    while (time < 1) {
      time += Time.deltaTime / duration;
      Vector3 newpos = Vector3.Lerp (start_point, end_point, time);
      // Don't modify zindex because can be modified after call this method.
      newpos.z = GhostWasteActive_Image.transform.localPosition.z;
      GhostWasteActive_Image.transform.localPosition = newpos;

      if (time > 0.5f && !playedSound) {
        MenuManager.GetInstance ().PlaySound (sound);
        playedSound = true;

      }

      yield return null;
    }

    wa.gameObject.GetComponent<Image> ().enabled = true;
    GhostWasteActive_Image.gameObject.SetActive (false);
    UpdateActiveWasteSprites (0, true);

    animationsCount--;
    CheckPlayerCanMove ();
  }

  int animationsCount = 0;

  public void CheckPlayerCanMove ()
  {
    if (animationsCount == 0 && !game_end && !timer_paused) {
      player_can_move = true;
    }
  }

  public IEnumerator MoveCardTo (kcard _kcard, Vector3 start_point, Vector3 end_point, float duration = 0.2F, float initialWait = -1.0F, MenuManager.SoundNamesEnum sound = MenuManager.SoundNamesEnum.NUM_SOUND)//move the ghost card
  {
    player_can_move = false;
    animationsCount++;

    // Move to top

    if (initialWait > 0) {
      yield return new WaitForSeconds (initialWait);

    }

    _kcard.gameObject.SetActive (true);


    _kcard.transform.SetAsLastSibling ();

    //int si = go.transform.GetSiblingIndex();
    //ghost_card.gameObject.transform.SetParent(go.transform.parent);
    //ghost_card.gameObject.transform.SetSiblingIndex(si);

    //_kcard.transform.localPosition.z
    _kcard.transform.localPosition = start_point;
    //go.transform.rotation = start_point.rotation;



    //animation
    float time = 0;

    bool playedSound = false;
    while (time < 1) {
      time += Time.deltaTime / duration;
      Vector3 newpos = Vector3.Lerp (start_point, end_point, time);
      // Don't modify zindex because can be modified after call this method.
      newpos.z = _kcard.transform.localPosition.z;
      _kcard.transform.localPosition = newpos;
      if (!playedSound && sound != MenuManager.SoundNamesEnum.NUM_SOUND && time > 0.6f) {
        MenuManager.GetInstance ().PlaySound (sound);
        playedSound = true;

      }

      yield return null;
    }





    if (_kcard.selfDestroy) {
      Destroy (_kcard.gameObject);
      StartCoroutine (UpdateActiveWasteSprites (0, true));
    }

    animationsCount--;
    CheckPlayerCanMove ();
  }

  private bool IsOnTableau (card_foundation foundation)
  {
    return (int)foundation >= 3 && (int)foundation <= 9;
  }

  private bool IsOnTableau (int index)
  {
    return (int)deck_cards [index].foundation >= 3 && (int)deck_cards [index].foundation <= 9;
  }

  private bool IsOnFoundation (card_foundation foundation)
  {
    return (int)foundation >= 10 && (int)foundation <= 13;
  }

  private bool IsOnFoundation (int index)
  {
    return (int)deck_cards [index].foundation >= 10 && (int)deck_cards [index].foundation <= 13;
  }

  private bool MatchColor (int s1, int s2)
  {
    bool s1IsBlack = s1 < 2;
    bool s1IsRed = s1 > 1;
    bool s2IsBlack = s2 < 2;
    bool s2IsRed = s2 > 1;
    return (s1IsBlack && s2IsRed) || (s1IsRed && s2IsBlack);
  }

  int GetFirstEmtpyPile ()
  {
    bool[] pileInUse = new bool[7] {
      false,
      false,
      false,
      false,
      false,
      false,
      false
    };

    for (int x = 0; x < deck_card_left; x++) {
      if (IsOnTableau (x)) {
        pileInUse [((int)deck_cards [x].foundation) - 3] = true;
      }
    }

    for (int x = 0; x < 7; x++) {
      if (!pileInUse [x]) {
        return x;
      }
    }

    return -1;
  }

  bool IsEmptyPile (int pileIndex)
  {
    for (int x = 0; x < deck_card_left; x++) {
      if (((int)deck_cards [x].foundation - 3) == pileIndex) {
        return false;
      }
    }

    return true;
  }

  void UpdateFoundationColliders (int suit)
  {
    int maxRank = -1;
    int index = -1;
    for (int x = 0; x < deck_card_left; x++) {
      if (((int)deck_cards [x].foundation - 10) == suit) {
        if (maxRank < deck_cards [x].rank) {
          maxRank = deck_cards [x].rank;
          index = x;
        }

        if (deck_cards [x].tableauKCard) {
          deck_cards [x].tableauKCard.EnableCollider (false);
        }
      }
    }

    if (index != -1 && deck_cards [index].tableauKCard) {
      deck_cards [index].tableauKCard.EnableCollider (true);
    }
  }

  int GetRankOnFoundation (int suit)
  {
    int maxRank = -1;
    int index = -1;
    for (int x = 0; x < deck_card_left; x++) {
      if (((int)deck_cards [x].foundation - 10) == suit &&
       maxRank < deck_cards [x].rank) {
        maxRank = deck_cards [x].rank;
        index = x;
      }
    }

    return index;//maxRank;
  }

  int GetCardAtPos (card_foundation foundation, int pos)
  {
    for (int x = 0; x < deck_card_left; x++) {
      if (deck_cards [x].foundation == foundation && deck_cards [x].tableauIndex == pos) {
        return x;
      }
    }

    return -1;
  }

  int GetCardOnActiveWaste ()
  {
    int highestActiveWasteIndex = -1;
    int ret = -1;
    for (int i = 0; i < deck_card_left; i++) {
      if (deck_cards [i].foundation == card_foundation.waste_active &&
       highestActiveWasteIndex < deck_cards [i].wasteActivePos) {
        highestActiveWasteIndex = deck_cards [i].wasteActivePos;
        ret = i;
      }
    }

    return ret;
  }

  void ActivateWasteCard ()
  {
    int lastCardOnWasteIndex = -1;

    for (int x = 0; x < deck_card_left; x++) {
      if (deck_cards [x].foundation == card_foundation.waste) {
        lastCardOnWasteIndex = x;
      }
    }

    if (lastCardOnWasteIndex != -1) {
      for (int x = 0; x < deck_card_left; x++) {
        //Move all the cards active_waste to the left
        if (deck_cards [x].foundation == card_foundation.waste_active && deck_cards [x].wasteActivePos < 2) {
          int old = deck_cards [x].wasteActivePos;
          AddCardToUndo (x, false);
          deck_cards [x].wasteActivePos++;
          StartCoroutine (MoveDeckCard (deck_cards [x].suit, deck_cards [x].rank, false, old, deck_cards [x].wasteActivePos));
        }

      }

      AddCardToUndo (lastCardOnWasteIndex, false);
      deck_cards [lastCardOnWasteIndex].foundation = card_foundation.waste_active;
      deck_cards [lastCardOnWasteIndex].wasteActivePos = 0;
      waste_card0.gameObject.SetActive (true);
      waste_card0.sprite = card_deck_sprite [deck_cards [lastCardOnWasteIndex].suit, deck_cards [lastCardOnWasteIndex].rank];
      StartCoroutine (UpdateActiveWasteSprites (animationSpeedTime));

    } else {
      StartCoroutine (UpdateActiveWasteSprites (0, true));
    }
  }

  public void AddUndoStep ()
  {
    if (undo_list.Count >= maxUndoSteps) {
      undo_list.RemoveAt (0);
    }

    UndoInfo undoInfo = new UndoInfo ();
    undo_list.Add (undoInfo);
  }

  public void ClearUndo ()
  {
    undo_list.Clear ();
  }

  public void RemoveUndoStep ()
  {
    if (undo_list.Count >= 0) {
      undo_list.RemoveAt (undo_list.Count - 1);
    }
  }

  public void AddCardToUndo (card_class cc)
  {
    if (undo_list.Count > 0) {
      undo_list [undo_list.Count - 1].undo_cards.Add (cc);
    }
  }

  public void AddCardToUndo (int index, bool destroyGameObject)
  {
    // Just create a new card_class and add it to the list.

    if (undo_list.Count > 0) {
      card_class cc = new card_class ();

      cc.suit = deck_cards [index].suit;
      cc.rank = deck_cards [index].rank;
      cc.foundation = deck_cards [index].foundation;
      cc.wasteActivePos = deck_cards [index].wasteActivePos;
      cc.tableauFaceUp = deck_cards [index].tableauFaceUp;
      cc.tableauPeek = deck_cards [index].tableauPeek;
      cc.tableauIndex = deck_cards [index].tableauIndex;
      if (IsOnTableau (deck_cards [index].foundation)) {
        cc.oldOffset = getTableauOffset (deck_cards [index].foundation, deck_cards [index].tableauIndex);
        /*Debug.Log ("index " + index);
				Debug.Log ("tindex " + deck_cards [index].tableauIndex);
				Debug.Log ("offset " + cc.oldOffset);*/
      } else {
        cc.oldOffset = 0;
      }


      //Only used for undo. For optimization in some cases the gameObject must be destroyed.
      cc.destroyGOOnUndo = destroyGameObject;

      //cardWasInFoundation never is reverted


      undo_list [undo_list.Count - 1].undo_cards.Add (cc);
    }

  }

  float getTableauOffset (card_foundation foundation, int tindex)
  {
    float total = 0;

    for (int x = 0; x < deck_card_left; x++) {
      if (foundation == deck_cards [x].foundation && deck_cards [x].tableauIndex < tindex) {
        if (!deck_cards [x].tableauFaceUp) {
          total += tableau_back_offset;
        } else {
          total += tableau_normal_offset;
        }
      }
    }

    return total;
  }

  public void UndoLastMove ()
  {
    if (player_can_move && undo_list.Count > 0) {
      bool mustFixGOS = false;
      float timeWasteToDeck = 0.0F;
      float timeAnyToWaste = 0.0F;
      float deckMoves = 0;
      bool soundPlayed = false;

      List<card_class> undo_cards = undo_list [undo_list.Count - 1].undo_cards;

      //undo_cards.Sort((b, a) => a.wasteActivePos.CompareTo(b.wasteActivePos));

      for (int i = 0; i < undo_cards.Count; i++) {
        // Find current card
        int index = GetCard (undo_cards [i].suit, undo_cards [i].rank);
        if (index != -1) {
          if (deck_cards [index].foundation == card_foundation.waste_active && undo_cards [i].foundation == card_foundation.deck) {
            StartCoroutine (MoveUndoDeckCard (deck_cards [index].suit, deck_cards [index].rank, true, deck_cards [index].wasteActivePos, 0, (timeBetwenDeckCardAnimations * deckMoves)));

            if (timeWasteToDeck <= animationSpeedTime + (timeBetwenDeckCardAnimations * deckMoves)) {
              timeWasteToDeck = animationSpeedTime + (timeBetwenDeckCardAnimations * deckMoves);
            }

            deckMoves++;
          } else if (deck_cards [index].foundation == card_foundation.waste_active
              && undo_cards [i].foundation == card_foundation.waste_active) {

            if (deck_cards [index].wasteActivePos != undo_cards [i].wasteActivePos) {
              int initial = deck_cards [index].wasteActivePos < 0 ? 0 : deck_cards [index].wasteActivePos;
              int final = undo_cards [i].wasteActivePos < 0 ? 0 : undo_cards [i].wasteActivePos;

              if (initial < final) {
                StartCoroutine (MoveUndoDeckCard (deck_cards [index].suit, deck_cards [index].rank, false, initial, final));
              } else {
                StartCoroutine (MoveUndoDeckCard (deck_cards [index].suit, deck_cards [index].rank, false, initial, final));
              }

              if (undo_cards [i].wasteActivePos == 0) {
                waste_card0.gameObject.SetActive (true);
                //waste_card0.sprite = card_deck_sprite[undo_cards[i].suit, undo_cards[i].rank];
              }

              if (timeAnyToWaste <= animationSpeedTime) {
                timeAnyToWaste = animationSpeedTime;
              }
            }

          } else if (deck_cards [index].foundation == card_foundation.waste_active
              && undo_cards [i].foundation == card_foundation.waste) {
            // No animations
            /*if (undo_cards[i].wasteActivePos == 0)
						{
								waste_card0.gameObject.SetActive(true);
								waste_card0.sprite = card_deck_sprite[undo_cards[i].suit, undo_cards[i].rank];
						}*/
          } else if (deck_cards [index].foundation == card_foundation.waste && undo_cards [i].foundation == card_foundation.waste_active) {

            if (deck_cards [index].wasteActivePos != undo_cards [i].wasteActivePos) {
              StartCoroutine (MoveUndoDeckCard (deck_cards [index].suit, deck_cards [index].rank,
                false,
                deck_cards [index].wasteActivePos < 0 ? 0 : deck_cards [index].wasteActivePos,
                undo_cards [i].wasteActivePos < 0 ? 0 : undo_cards [i].wasteActivePos));

              if (timeAnyToWaste <= animationSpeedTime) {
                timeAnyToWaste = animationSpeedTime;
              }
            }


            if (undo_cards [i].wasteActivePos == 0) {
              waste_card0.gameObject.SetActive (true);
              waste_card0.sprite = card_deck_sprite [undo_cards [i].suit, undo_cards [i].rank];
            }

          } else if (IsOnFoundation (index) && undo_cards [i].foundation == card_foundation.waste_active) {
            if (deck_cards [index].tableauKCard) {
              //Debug.Log(CardString(deck_cards[wasteActiveIndex]));
              Image img = waste_cards [undo_cards [i].wasteActivePos];

              Vector3 nt = board.transform.InverseTransformPoint (img.transform.position);
              Vector3 newPos = new Vector3 (nt.x, nt.y, 0);

              // Move to waste active
              StartCoroutine (MoveCardTo (deck_cards [index].tableauKCard,
                deck_cards [index].tableauKCard.transform.localPosition,
                newPos, animationSpeedTime,
                -1,
                soundPlayed ? MenuManager.SoundNamesEnum.NUM_SOUND : MenuManager.SoundNamesEnum.TAKE_CARD_SOUND));

              soundPlayed = true;

              if (timeAnyToWaste <= animationSpeedTime) {
                timeAnyToWaste = animationSpeedTime;
              }
            }
          } else if (IsOnFoundation (index) && IsOnTableau (undo_cards [i].foundation)) {
            if (deck_cards [index].tableauKCard) {

              Vector3 initPos = new Vector3 (deck_cards [index].tableauKCard.transform.localPosition.x, deck_cards [index].tableauKCard.transform.localPosition.y, -undo_cards [i].tableauIndex);
              deck_cards [index].tableauKCard.transform.localPosition = initPos;


              Vector3 finalPos;

              if (undo_cards [i].oldOffset != 0) {
                finalPos = new Vector3 (boardPiles [(int)(undo_cards [i].foundation) - 3].transform.localPosition.x,
                  boardPiles [(int)(undo_cards [i].foundation) - 3].transform.localPosition.y - undo_cards [i].oldOffset,
                  -undo_cards [i].tableauIndex);
              } else {
                // It should never enter here.
                finalPos = new Vector3 (boardPiles [(int)(undo_cards [i].foundation) - 3].transform.localPosition.x,
                  boardPiles [(int)(undo_cards [i].foundation) - 3].transform.localPosition.y - (tableau_normal_offset * undo_cards [i].tableauIndex),
                  -undo_cards [i].tableauIndex);
              }

              // Move to tableau
              StartCoroutine (MoveCardTo (deck_cards [index].tableauKCard, initPos, finalPos, animationSpeedTime, -1, soundPlayed ? MenuManager.SoundNamesEnum.NUM_SOUND : MenuManager.SoundNamesEnum.TAKE_CARD_SOUND));

              soundPlayed = true;


            }
          } else if (deck_cards [index].foundation == card_foundation.deck && (undo_cards [i].foundation == card_foundation.waste || undo_cards [i].foundation == card_foundation.waste_active)) {
            // No animations
            mustFixGOS = true;

          } else if (IsOnTableau (index) && IsOnFoundation (undo_cards [i].foundation)) {

            if (deck_cards [index].tableauKCard) {
              Vector3 initPos = new Vector3 (deck_cards [index].tableauKCard.transform.localPosition.x, deck_cards [index].tableauKCard.transform.localPosition.y, 0);
              deck_cards [index].tableauKCard.transform.localPosition = initPos;

              Vector3 nt2 = board.transform.InverseTransformPoint (foundations [deck_cards [index].suit].transform.position);
              Vector3 finalPos = new Vector3 (nt2.x, nt2.y, 0);

              StartCoroutine (MoveCardTo (deck_cards [index].tableauKCard, initPos, finalPos, animationSpeedTime, -1, soundPlayed ? MenuManager.SoundNamesEnum.NUM_SOUND : MenuManager.SoundNamesEnum.TAKE_CARD_SOUND));

              soundPlayed = true;

            }

          } else if (IsOnTableau (index) && undo_cards [i].foundation == card_foundation.waste_active) {

            if (deck_cards [index].tableauKCard) {
              Vector3 initPos = new Vector3 (deck_cards [index].tableauKCard.transform.localPosition.x, deck_cards [index].tableauKCard.transform.localPosition.y, 0);
              deck_cards [index].tableauKCard.transform.localPosition = initPos;

              Image img = waste_cards [undo_cards [i].wasteActivePos];

              Vector3 nt = board.transform.InverseTransformPoint (img.transform.position);
              Vector3 finalPos = new Vector3 (nt.x, nt.y, 0);

              StartCoroutine (MoveCardTo (deck_cards [index].tableauKCard, initPos, finalPos, animationSpeedTime, -1, soundPlayed ? MenuManager.SoundNamesEnum.NUM_SOUND : MenuManager.SoundNamesEnum.TAKE_CARD_SOUND));

              soundPlayed = true;

              if (timeAnyToWaste <= animationSpeedTime) {
                timeAnyToWaste = animationSpeedTime;
              }
            }


          } else { // tableau => tableau
            if (deck_cards [index].tableauKCard && deck_cards [index].foundation != undo_cards [i].foundation) {
              if (deck_cards [index].tableauKCard) {
                Vector3 initPos = new Vector3 (deck_cards [index].tableauKCard.transform.localPosition.x, deck_cards [index].tableauKCard.transform.localPosition.y, -undo_cards [i].tableauIndex);
                deck_cards [index].tableauKCard.transform.localPosition = initPos;

                Vector3 finalPos;

                if (undo_cards [i].oldOffset != 0) {
                  finalPos = new Vector3 (boardPiles [(int)(undo_cards [i].foundation) - 3].transform.localPosition.x,
                    boardPiles [(int)(undo_cards [i].foundation) - 3].transform.localPosition.y - undo_cards [i].oldOffset,
                    -undo_cards [i].tableauIndex);
                } else {
                  // It should never enter here.
                  finalPos = new Vector3 (boardPiles [(int)(undo_cards [i].foundation) - 3].transform.localPosition.x,
                    boardPiles [(int)(undo_cards [i].foundation) - 3].transform.localPosition.y - (tableau_normal_offset * undo_cards [i].tableauIndex),
                    -undo_cards [i].tableauIndex);
                }

                StartCoroutine (MoveCardTo (deck_cards [index].tableauKCard, initPos, finalPos, animationSpeedTime, -1, soundPlayed ? MenuManager.SoundNamesEnum.NUM_SOUND : MenuManager.SoundNamesEnum.TAKE_CARD_SOUND));

                soundPlayed = true;

              }

            }


          }

          card_foundation oldF = deck_cards [index].foundation;
          // Undo card status
          deck_cards [index].foundation = undo_cards [i].foundation;
          deck_cards [index].wasteActivePos = undo_cards [i].wasteActivePos;
          deck_cards [index].tableauFaceUp = undo_cards [i].tableauFaceUp;
          deck_cards [index].tableauPeek = undo_cards [i].tableauPeek;
          deck_cards [index].tableauIndex = undo_cards [i].tableauIndex;
          if (deck_cards [index].tableauKCard) {
            if (IsOnTableau (undo_cards [i].foundation)) {
              deck_cards [index].tableauKCard.SetFace (undo_cards [i].tableauFaceUp, true);
            }
          }

          if (IsOnFoundation (undo_cards [i].foundation) || IsOnFoundation (oldF)) {
            UpdateFoundationColliders (undo_cards [i].suit);
          }

          if (undo_cards [i].destroyGOOnUndo) {
            // This card must be destroy afte the animation. The Coroutine will handle it.
            deck_cards [index].tableauKCard.selfDestroy = true;
            deck_cards [index].tableauKCard = null;
          }
        }
      }


      int undo_previous_score = undo_list [undo_list.Count - 1].undo_previous_score;

      if (undo_previous_score > 0) {
        UpdateScore (-undo_previous_score);
        CreateScoreEvent (undo_button.transform.position, (-undo_previous_score).ToString (scoreFormat), current_score);
      }

      if (deckMoves >= 2) {
        MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.NEW_THREE_CARD_SOUND);
      } else if (deckMoves == 1) {
        MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.NEW_CARD_SOUND);
      }

      RemoveUndoStep ();
      UpdateUndoButton ();

      float wtime = 0;
      if (timeAnyToWaste > 0 && timeWasteToDeck > 0) {
        wtime = Mathf.Min (timeAnyToWaste, timeWasteToDeck);
      } else {
        wtime = Mathf.Max (timeAnyToWaste, timeWasteToDeck);
      }

      StartCoroutine (UpdateActiveWasteSprites (wtime, mustFixGOS));
    }
  }

  int GetScoreByRank (int r)
  {
    switch (r) {
    case 0:
      {
        return CAce_Board_To_Foundation;
      }
    case 1:
      {
        return C2_Board_To_Foundation;
      }
    case 2:
      {
        return C3_Board_To_Foundation;
      }
    case 3:
      {
        return C4_Board_To_Foundation;
      }
    case 4:
      {
        return C5_Board_To_Foundation;
      }
    case 5:
      {
        return C6_Board_To_Foundation;
      }
    case 6:
      {
        return C7_Board_To_Foundation;
      }
    case 7:
      {
        return C8_Board_To_Foundation;
      }
    case 8:
      {
        return C9_Board_To_Foundation;
      }
    case 9:
      {
        return C10_Board_To_Foundation;
      }
    case 10:
      {
        return CJ_Board_To_Foundation;
      }
    case 11:
      {
        return CQ_Board_To_Foundation;
      }
    case 12:
      {
        return CK_Board_To_Foundation;
      }
    }

    return 0;
  }


  int GetPeekIndex (card_foundation f)
  {
    for (int k = 0; k < deck_card_left; k++) {
      if (deck_cards [k].foundation == f && deck_cards [k].tableauPeek) {
        return deck_cards [k].tableauIndex;
      }
    }

    return -1;
  }

  public int GetCardIndex (kcard card)
  {
    for (int i = 0; i < deck_card_left; i++) {
      if (deck_cards [i].tableauKCard == card) {
        return i;
      }
    }

    return -1;
  }



  bool PlayPileToFreePile (int i, int pi = -1)
  {
    int deltaScore = 0;
    if (deck_cards [i].rank == 12 && IsOnTableau (i)) { // K
      int pileIndex;

      if (pi == -1) {
        pileIndex = GetFirstEmtpyPile ();
      } else {
        pileIndex = IsEmptyPile (pi) ? pi : -1;
      }


      if (pileIndex != -1) {


        Vector3 newPos2 = new Vector3 (boardPiles [pileIndex].transform.localPosition.x, boardPiles [pileIndex].transform.localPosition.y, 0);
        StartCoroutine (MoveCardTo (deck_cards [i].tableauKCard, deck_cards [i].tableauKCard.transform.localPosition, newPos2, 0.2f, -1, MenuManager.SoundNamesEnum.TAKE_CARD_SOUND));

        // Move the cards under this card, save indexes in a list for update values after make other operations.
        List<int> indexes = new List<int> ();
        if (!deck_cards [i].tableauPeek) {
          // Card in tableau, find cards under this one

          for (int k = 0; k < deck_card_left; k++) {
            if (k != i && deck_cards [k].foundation == deck_cards [i].foundation && deck_cards [k].tableauIndex > deck_cards [i].tableauIndex) {
              indexes.Add (k);
            }
          }

          // Very important, sort because MoveCardTo puts the cards on top of hierarchy
          indexes.Sort ((a, b) => deck_cards [a].tableauIndex.CompareTo (deck_cards [b].tableauIndex));

          for (int l = 0; l < indexes.Count; l++) {
            int underCardIndex = indexes [l];
            Vector3 newPos = new Vector3 (boardPiles [pileIndex].transform.localPosition.x, boardPiles [pileIndex].transform.localPosition.y - (tableau_normal_offset * (l + 1)), boardPiles [pileIndex].localPosition.z);
            StartCoroutine (MoveCardTo (deck_cards [underCardIndex].tableauKCard, deck_cards [underCardIndex].tableauKCard.transform.localPosition, newPos));
          }
        }

        // Card over selected card
        int parentCardIndex = -1;
        int maxIndex = -1;
        for (int k = 0; k < deck_card_left; k++) {
          if (k != i && deck_cards [k].foundation == deck_cards [i].foundation &&
          deck_cards [k].tableauIndex < deck_cards [i].tableauIndex /*&&
                                            deck_cards[k].tableauFaceUp == false*/) {
            if (maxIndex < deck_cards [k].tableauIndex) {
              maxIndex = deck_cards [k].tableauIndex;
              parentCardIndex = k;
            }
          }
        }

        if (parentCardIndex != -1) {
          if (deck_cards [parentCardIndex].tableauFaceUp == false) {
            ClearUndo ();
            deltaScore += Exposed_Card;
            CreateScoreEvent (deck_cards [parentCardIndex].tableauKCard.transform.position, Exposed_Card.ToString (scoreFormat), current_score + deltaScore);
          } else {
            AddUndoStep ();
            AddCardToUndo (parentCardIndex, false);

            // If no card exposed, we can enable undo movement.
            AddCardToUndo (i, false);

            for (int l = 0; l < indexes.Count; l++) {
              int underCardIndex = indexes [l];

              AddCardToUndo (underCardIndex, false);
            }

          }

          deck_cards [parentCardIndex].tableauKCard.SetFace (true, true);
          deck_cards [parentCardIndex].tableauFaceUp = true;
          deck_cards [parentCardIndex].tableauPeek = true;
        } else {
          AddUndoStep ();
          // If no card exposed, we can enable undo movement.
          AddCardToUndo (i, false);

          for (int l = 0; l < indexes.Count; l++) {
            int underCardIndex = indexes [l];

            AddCardToUndo (underCardIndex, false);
          }
        }


        // Selected card
        deck_cards [i].tableauIndex = 0;
        Vector3 newPos3 = new Vector3 (deck_cards [i].tableauKCard.transform.localPosition.x, deck_cards [i].tableauKCard.transform.localPosition.y, -deck_cards [i].tableauIndex);
        deck_cards [i].tableauKCard.transform.localPosition = newPos3;
        deck_cards [i].tableauPeek = indexes.Count > 0 ? false : true;
        deck_cards [i].foundation = (card_foundation)(pileIndex + 3);
        //Debug.Log("Clicked Card => " + CardString(deck_cards[i]));

        // Cards under selected card
        for (int l = 0; l < indexes.Count; l++) {
          int underCardIndex = indexes [l];

          deck_cards [underCardIndex].tableauIndex = deck_cards [i].tableauIndex + (l + 1);


          Vector3 newPos4 = new Vector3 (deck_cards [underCardIndex].tableauKCard.transform.localPosition.x, deck_cards [underCardIndex].tableauKCard.transform.localPosition.y, -deck_cards [underCardIndex].tableauIndex);
          deck_cards [underCardIndex].tableauKCard.transform.localPosition = newPos4;

          deck_cards [underCardIndex].tableauPeek = (l == indexes.Count - 1) ? true : false;
          deck_cards [underCardIndex].foundation = deck_cards [i].foundation;

          //Debug.Log("Under Card => " + CardString(deck_cards[underCardIndex]));
        }

        UpdateUndoButton ();

        UpdateScore (deltaScore);

        IsGameCompleted ();

        return true;
      }
    }

    return false;
  }

  bool PlayPileToFoundation (int i, int suit = -1)
  {
    int deltaScore = 0;
    if (deck_cards [i].tableauPeek && IsOnTableau (i) && (deck_cards [i].suit == suit || suit == -1)) {
      int targetRankIndex = GetRankOnFoundation (deck_cards [i].suit);
      int targetRank = (targetRankIndex != -1) ? deck_cards [targetRankIndex].rank : -1;

      if (targetRank == deck_cards [i].rank - 1) {
        //if (!deck_cards[i].cardWasInFoundation)
        {
          int ttf = GetScoreByRank (deck_cards [i].rank);
          deltaScore += ttf;
          //Vector3 scorepos = new Vector3(deck_cards[i].tableauKCard.transform.position.x, deck_cards[i].tableauKCard.transform.position.y + 0.6f, deck_cards[i].tableauKCard.transform.position.z);
          Vector3 scorepos = new Vector3 (foundations [deck_cards [i].suit].transform.position.x, foundations [deck_cards [i].suit].transform.position.y - 1.05F, foundations [deck_cards [i].suit].transform.position.z);
          CreateScoreEvent (scorepos, ttf.ToString (scoreFormat), current_score + deltaScore);
        }


        Vector3 nt2 = board.transform.InverseTransformPoint (foundations [deck_cards [i].suit].transform.position);
        Vector3 newPos2 = new Vector3 (nt2.x, nt2.y, 0);
        StartCoroutine (MoveCardTo (deck_cards [i].tableauKCard, deck_cards [i].tableauKCard.transform.localPosition, newPos2, 0.2f, -1, MenuManager.SoundNamesEnum.CARD_TO_FOUNDATION_SOUND));


        // Card over selected card
        int parentCardIndex = -1;
        int maxIndex = -1;
        for (int k = 0; k < deck_card_left; k++) {
          if (k != i && deck_cards [k].foundation == deck_cards [i].foundation &&
          deck_cards [k].tableauIndex < deck_cards [i].tableauIndex /*&&
                                            deck_cards[k].tableauFaceUp == false*/) {
            if (maxIndex < deck_cards [k].tableauIndex) {
              maxIndex = deck_cards [k].tableauIndex;
              parentCardIndex = k;
            }
          }
        }

        bool exposed = false;
        if (parentCardIndex != -1) {
          if (deck_cards [parentCardIndex].tableauFaceUp == false) {
            exposed = true;
            ClearUndo ();
            deltaScore += Exposed_Card;
            CreateScoreEvent (deck_cards [parentCardIndex].tableauKCard.transform.position, Exposed_Card.ToString (scoreFormat), current_score + deltaScore, 0.2f);
          } else {
            AddUndoStep ();
            AddCardToUndo (parentCardIndex, false);
            AddCardToUndo (i, false);
          }

          deck_cards [parentCardIndex].tableauKCard.SetFace (true, true);
          deck_cards [parentCardIndex].tableauFaceUp = true;
          deck_cards [parentCardIndex].tableauPeek = true;
        } else {
          AddUndoStep ();
          AddCardToUndo (i, false);
        }


        UpdateScore (deltaScore);

        // Selected card
        deck_cards [i].tableauIndex = -1;
        deck_cards [i].tableauPeek = false;
        deck_cards [i].foundation = (card_foundation)(deck_cards [i].suit + 10);
        deck_cards [i].wasteActivePos = -1;
        deck_cards [i].tableauFaceUp = true;
        deck_cards [i].cardWasInFoundation = true;

        UpdateFoundationColliders (deck_cards [i].suit);

        UpdateUndoButton ();

        IsGameCompleted ();

        return true;
      }
    }

    return false;
  }

  bool PlayPileToPile (int i, int oi = -1)
  {
    int deltaScore = 0;

    int init = (oi == -1) ? 0 : oi;
    int end = (oi == -1) ? deck_card_left : oi + 1;

    if (deck_cards [i].rank != 12 && IsOnTableau (i)) { // Not K
      // Check if this card can be moved to another pile
      for (int j = init; j < end; j++) {
        if (deck_cards [j].foundation != deck_cards [i].foundation &&
        IsOnTableau (j) &&
        deck_cards [j].tableauPeek &&
        MatchColor (deck_cards [j].suit, deck_cards [i].suit) &&
        deck_cards [j].rank == deck_cards [i].rank + 1) {

          //Debug.Log("Target Card => " + CardString(deck_cards[j]));

          Vector3 newPos2 = new Vector3 (deck_cards [j].tableauKCard.transform.localPosition.x, deck_cards [j].tableauKCard.transform.localPosition.y - tableau_normal_offset, deck_cards [j].tableauKCard.transform.localPosition.z);
          StartCoroutine (MoveCardTo (deck_cards [i].tableauKCard, deck_cards [i].tableauKCard.transform.localPosition, newPos2, 0.2f, -1, MenuManager.SoundNamesEnum.TAKE_CARD_SOUND));

          // Move the cards under this card, save indexes in a list for update values after make other operations.
          List<int> indexes = new List<int> ();
          if (!deck_cards [i].tableauPeek) {
            // Card in tableau, find cards under this one

            for (int k = 0; k < deck_card_left; k++) {
              if (k != i && deck_cards [k].foundation == deck_cards [i].foundation && deck_cards [k].tableauIndex > deck_cards [i].tableauIndex) {
                indexes.Add (k);
              }
            }

            // Very important, sort because MoveCardTo puts the cards on top of hierarchy
            indexes.Sort ((a, b) => deck_cards [a].tableauIndex.CompareTo (deck_cards [b].tableauIndex));

            for (int l = 0; l < indexes.Count; l++) {
              int underCardIndex = indexes [l];
              Vector3 newPos = new Vector3 (deck_cards [j].tableauKCard.transform.localPosition.x, deck_cards [j].tableauKCard.transform.localPosition.y - (tableau_normal_offset * (l + 2)), deck_cards [j].tableauKCard.transform.localPosition.z);
              StartCoroutine (MoveCardTo (deck_cards [underCardIndex].tableauKCard, deck_cards [underCardIndex].tableauKCard.transform.localPosition, newPos));
            }
          }

          // Card over selected card
          int parentCardIndex = -1;
          int maxIndex = -1;
          for (int k = 0; k < deck_card_left; k++) {
            if (k != i && deck_cards [k].foundation == deck_cards [i].foundation &&
            deck_cards [k].tableauIndex < deck_cards [i].tableauIndex /*&&
	                                            deck_cards[k].tableauFaceUp == false*/) {
              if (maxIndex < deck_cards [k].tableauIndex) {
                maxIndex = deck_cards [k].tableauIndex;
                parentCardIndex = k;
              }
            }
          }

          if (parentCardIndex != -1) {
            if (deck_cards [parentCardIndex].tableauFaceUp == false) {
              ClearUndo ();
              deltaScore += Exposed_Card;
              CreateScoreEvent (deck_cards [parentCardIndex].tableauKCard.transform.position, Exposed_Card.ToString (scoreFormat), current_score + deltaScore);
            } else {
              AddUndoStep ();
              AddCardToUndo (parentCardIndex, false);

              AddCardToUndo (i, false);
              AddCardToUndo (j, false);

              for (int l = 0; l < indexes.Count; l++) {
                int underCardIndex = indexes [l];
                AddCardToUndo (underCardIndex, false);
              }
            }

            deck_cards [parentCardIndex].tableauKCard.SetFace (true, true);
            deck_cards [parentCardIndex].tableauFaceUp = true;
            deck_cards [parentCardIndex].tableauPeek = true;
          } else {
            AddUndoStep ();
            AddCardToUndo (i, false);
            AddCardToUndo (j, false);

            for (int l = 0; l < indexes.Count; l++) {
              int underCardIndex = indexes [l];
              AddCardToUndo (underCardIndex, false);
            }
          }


          // Selected card
          deck_cards [i].tableauIndex = GetPeekIndex (deck_cards [j].foundation) + 1;
          Vector3 newPos3 = new Vector3 (deck_cards [i].tableauKCard.transform.localPosition.x, deck_cards [i].tableauKCard.transform.localPosition.y, -deck_cards [i].tableauIndex);
          deck_cards [i].tableauKCard.transform.localPosition = newPos3;
          deck_cards [i].tableauPeek = indexes.Count > 0 ? false : true;
          deck_cards [i].foundation = deck_cards [j].foundation;
          //Debug.Log("Clicked Card => " + CardString(deck_cards[i]));


          // Cards under selected card
          for (int l = 0; l < indexes.Count; l++) {
            int underCardIndex = indexes [l];

            deck_cards [underCardIndex].tableauIndex = deck_cards [i].tableauIndex + (l + 1);


            Vector3 newPos4 = new Vector3 (deck_cards [underCardIndex].tableauKCard.transform.localPosition.x, deck_cards [underCardIndex].tableauKCard.transform.localPosition.y, -deck_cards [underCardIndex].tableauIndex);
            deck_cards [underCardIndex].tableauKCard.transform.localPosition = newPos4;

            deck_cards [underCardIndex].tableauPeek = (l == indexes.Count - 1) ? true : false;
            deck_cards [underCardIndex].foundation = deck_cards [i].foundation;

            //Debug.Log("Under Card => " + CardString(deck_cards[underCardIndex]));
          }

          // Target card
          deck_cards [j].tableauPeek = false;

          UpdateUndoButton ();

          UpdateScore (deltaScore);

          IsGameCompleted ();

          return true;
        }
      }
    }

    return false;
  }

  float offsetForDrag = 15;
  Vector2 initDrag = Vector2.zero;
  Vector2 initOffset = new Vector2 ();
  kcard initDraggedCard = null;
  wasteActive initDraggedWasteActive = null;

  bool CanDrag (Vector2 mp)
  {
    return Vector2.Distance (mp, initDrag) >= offsetForDrag && Input.touches.Length <= 1;
  }

  public void OnCardDrag (kcard card)
  {
    if (player_can_move && Input.touches.Length <= 1 && draggedCards.Count == 0 && initDrag == Vector2.zero) {
      Vector2 mousePos = MouseInBoard ();
      initDrag = mousePos;
      initDraggedCard = card;
      initDraggedWasteActive = null;
    }
  }

  public void OnWasteActiveDrag (wasteActive wa)
  {
    if (player_can_move && Input.touches.Length <= 1 && draggedCards.Count == 0 && initDrag == Vector2.zero) {
      int wasteActiveIndex = GetCardOnActiveWaste ();

      if (wasteActiveIndex != -1 && deck_cards [wasteActiveIndex].wasteActivePos == wa.wasteActiveIndex) {
        Vector2 mousePos = MouseInBoard ();
        initDrag = mousePos;
        initDraggedCard = null;
        initDraggedWasteActive = wa;
      }
    }
  }

  public void OnWasteActiveDrop (wasteActive wa)
  {
    if (player_can_move && Input.touches.Length <= 1 && draggedCards.Count != 0) {
      bool found = false;
      int wasteActiveIndex = GetCardOnActiveWaste ();

      if (wasteActiveIndex != -1) {

        // Find collisions
        Collider2D ccollider = GhostWasteActive_Image.gameObject.GetComponent<BoxCollider2D> ();
        Bounds b = GhostWasteActive_Image.gameObject.GetComponent<BoxCollider2D> ().bounds;
        Collider2D[] temp_colliders = Physics2D.OverlapBoxAll (b.center, b.size, GhostWasteActive_Image.gameObject.transform.rotation.eulerAngles.z);


        for (int x = 0; x < temp_colliders.Length && !found; x++) {
          if (temp_colliders [x] != ccollider) {
            kcard temp_kcard = temp_colliders [x].gameObject.GetComponent<kcard> ();
            if (temp_kcard) { // It's a card
              int tk = GetCardIndex (temp_kcard);
              if (tk != -1) {
                found = PlayWasteActiveToPile (wasteActiveIndex, wa, tk);
              }
            } else {
              kpile temp_kpile = temp_colliders [x].gameObject.GetComponent<kpile> ();
              if (temp_kpile) { // It's an empty pile
                found = PlayWasteActiveToFreePile (wasteActiveIndex, wa, temp_kpile.pileIndex);
              } else {
                kfoundation temp_kfoundation = temp_colliders [x].gameObject.GetComponent<kfoundation> ();
                if (temp_kfoundation) { // It's a foundation
                  found = PlayWasteActiveToFoundation (wasteActiveIndex, wa, temp_kfoundation.foundationIndex);
                }
              }
            }
          }
        }
      }

      // Fail dragging
      if (initDraggedWasteActive) {
        if (!found) {
          StartCoroutine (MoveGhostCardTo (initDraggedWasteActive, draggedCards [0].initialPos, animationSpeedTime, MenuManager.SoundNamesEnum.ERROR_SOUND));
        } else {
          initDraggedWasteActive.gameObject.GetComponent<Image> ().enabled = true;
          GhostWasteActive_Image.gameObject.SetActive (false);
        }
      }

      draggedCards.Clear ();
    }

    initDrag = Vector2.zero;
    initDraggedCard = null;
    initDraggedWasteActive = null;
  }

  public void OnCardDrop (kcard card)
  {
    if (player_can_move && draggedCards.Count != 0) {
      bool found = false;
      // Find card in deck_cards
      int i = GetCardIndex (card);
      if (i != -1) {
        // Find collisions
        Bounds b = card.gameObject.GetComponent<BoxCollider2D> ().bounds;
        Collider2D[] temp_colliders = Physics2D.OverlapBoxAll (b.center, b.size, card.gameObject.transform.rotation.eulerAngles.z);

        // Check all the collisions
        for (int x = 0; x < temp_colliders.Length && !found; x++) {
          if (temp_colliders [x] != card.ccollider) {
            kcard temp_kcard = temp_colliders [x].gameObject.GetComponent<kcard> ();
            if (temp_kcard) { // It's a card
              int tk = GetCardIndex (temp_kcard);
              if (tk != -1) {
                found = PlayPileToPile (i, tk);
                if (!found) {
                  found = PlayFoundationToPile (i, tk);
                }
              }
            } else {
              kpile temp_kpile = temp_colliders [x].gameObject.GetComponent<kpile> ();
              if (temp_kpile) { // It's an empty pile
                found = PlayPileToFreePile (i, temp_kpile.pileIndex);
                if (!found) {
                  found = PlayFoundationToFreePile (i, temp_kpile.pileIndex);
                }
              } else {
                kfoundation temp_kfoundation = temp_colliders [x].gameObject.GetComponent<kfoundation> ();
                if (temp_kfoundation) { // It's a foundation
                  found = PlayPileToFoundation (i, temp_kfoundation.foundationIndex);
                }
              }
            }
          }
        }
      }

      // Fail dragging
      if (!found && initDraggedCard) {
        for (int w = 0; w < draggedCards.Count; w++) {
          StartCoroutine (MoveCardTo (deck_cards [draggedCards [w].index].tableauKCard,
            deck_cards [draggedCards [w].index].tableauKCard.transform.localPosition,
            draggedCards [w].initialPos,
            animationSpeedTime,
            -1,
            w == 0 ? MenuManager.SoundNamesEnum.ERROR_SOUND : MenuManager.SoundNamesEnum.NUM_SOUND));
        }
      }


      draggedCards.Clear ();
    }

    initDrag = Vector2.zero;
    initDraggedCard = null;
    initDraggedWasteActive = null;
  }

  public void OnCardClicked (kcard card)
  {
    if (player_can_move && Input.touches.Length <= 1 && draggedCards.Count == 0) {
      // Find card in deck_cards
      for (int i = 0; i < deck_card_left; i++) {
        if (deck_cards [i].tableauKCard == card) {
          if (!PlayPileToFoundation (i)) {
            if (!PlayPileToFreePile (i)) {
              if (!PlayPileToPile (i)) {
                if (!PlayFoundationToFreePile (i)) {
                  if (!PlayFoundationToPile (i)) {
                    // Not available movement
                    WrongCard (card.gameObject, true);

                    if (IsOnTableau (deck_cards [i].foundation) && !deck_cards [i].tableauPeek) {
                      // Shake all cards below
                      for (int k = 0; k < deck_card_left; k++) {
                        if (k != i && deck_cards [k].foundation == deck_cards [i].foundation && deck_cards [k].tableauIndex > deck_cards [i].tableauIndex && deck_cards [k].tableauKCard) {
                          WrongCard (deck_cards [k].tableauKCard.gameObject, false);
                        }
                      }
                    }
                  }
                }
              }
            }
          }

          break;
        }
      }
    }
  }

  bool PlayFoundationToFreePile (int i, int pi = -1)
  {
    int deltaScore = 0;
    if (deck_cards [i].rank == 12 && IsOnFoundation (i)) { // K
      int pileIndex;

      if (pi == -1) {
        pileIndex = GetFirstEmtpyPile ();
      } else {
        pileIndex = IsEmptyPile (pi) ? pi : -1;
      }

      if (pileIndex != -1) {
        AddUndoStep ();

        AddCardToUndo (i, false);

        //deltaScore += Foundation_To_Board;
        deltaScore -= GetScoreByRank (deck_cards [i].rank);
        //Vector3 scorepos = new Vector3(deck_cards[i].tableauKCard.transform.position.x, deck_cards[i].tableauKCard.transform.position.y + 0.6f, deck_cards[i].tableauKCard.transform.position.z);
        Vector3 scorepos = new Vector3 (foundations [deck_cards [i].suit].transform.position.x, foundations [deck_cards [i].suit].transform.position.y - 1.05F, foundations [deck_cards [i].suit].transform.position.z);
        //CreateScoreEvent(scorepos, Foundation_To_Board.ToString(scoreFormat));
        CreateScoreEvent (scorepos, (-GetScoreByRank (deck_cards [i].rank)).ToString (scoreFormat), Mathf.Max (0, current_score + deltaScore));

        Vector3 newPos2 = new Vector3 (boardPiles [pileIndex].transform.localPosition.x, boardPiles [pileIndex].transform.localPosition.y, 0);
        StartCoroutine (MoveCardTo (deck_cards [i].tableauKCard, deck_cards [i].tableauKCard.transform.localPosition, newPos2, 0.2f, -1, MenuManager.SoundNamesEnum.TAKE_CARD_SOUND));

        // Update foundation card
        deck_cards [i].tableauIndex = 0;
        Vector3 newPos3 = new Vector3 (deck_cards [i].tableauKCard.transform.localPosition.x, deck_cards [i].tableauKCard.transform.localPosition.y, -deck_cards [i].tableauIndex);
        deck_cards [i].tableauKCard.transform.localPosition = newPos3;
        deck_cards [i].tableauPeek = true;
        deck_cards [i].foundation = (card_foundation)(pileIndex + 3);
        deck_cards [i].wasteActivePos = -1;
        deck_cards [i].tableauFaceUp = true;
        //deck_cards[foundationRankIndex].tableauKCard.EnableCollider(true);

        UpdateUndoButton ();

        UpdateScore (deltaScore);

        UpdateFoundationColliders (deck_cards [i].suit);

        IsGameCompleted ();

        return true;
      }
    }

    return false;
  }


  bool PlayFoundationToPile (int i, int oi = -1)
  {
    int deltaScore = 0;
    int init = (oi == -1) ? 0 : oi;
    int end = (oi == -1) ? deck_card_left : oi + 1;

    if (deck_cards [i].rank != 12 && IsOnFoundation (i)) { // Not K
      // Check if this card can be moved to another pile
      for (int j = init; j < end; j++) {
        if (deck_cards [j].foundation != deck_cards [i].foundation &&
        IsOnTableau (j) &&
        deck_cards [j].tableauPeek &&
        MatchColor (deck_cards [j].suit, deck_cards [i].suit) &&
        deck_cards [j].rank == deck_cards [i].rank + 1) {
          AddUndoStep ();

          AddCardToUndo (i, false);
          AddCardToUndo (j, false);

          //Debug.Log("Target Card => " + CardString(deck_cards[j]));

          //deltaScore += Foundation_To_Board;
          deltaScore -= GetScoreByRank (deck_cards [i].rank);
          //Vector3 scorepos = new Vector3(deck_cards[i].tableauKCard.transform.position.x, deck_cards[i].tableauKCard.transform.position.y + 0.6f, deck_cards[i].tableauKCard.transform.position.z);
          Vector3 scorepos = new Vector3 (foundations [deck_cards [i].suit].transform.position.x, foundations [deck_cards [i].suit].transform.position.y - 1.05F, foundations [deck_cards [i].suit].transform.position.z);
          //CreateScoreEvent(scorepos, Foundation_To_Board.ToString(scoreFormat));
          CreateScoreEvent (scorepos, (-GetScoreByRank (deck_cards [i].rank)).ToString (scoreFormat), Mathf.Max (0, current_score + deltaScore));

          Vector3 newPos2 = new Vector3 (deck_cards [j].tableauKCard.transform.localPosition.x, deck_cards [j].tableauKCard.transform.localPosition.y - tableau_normal_offset, deck_cards [j].tableauKCard.transform.localPosition.z);
          StartCoroutine (MoveCardTo (deck_cards [i].tableauKCard, deck_cards [i].tableauKCard.transform.localPosition, newPos2, 0.2f, -1, MenuManager.SoundNamesEnum.TAKE_CARD_SOUND));

          // Selected card
          deck_cards [i].tableauIndex = GetPeekIndex (deck_cards [j].foundation) + 1;
          Vector3 newPos3 = new Vector3 (deck_cards [i].tableauKCard.transform.localPosition.x, deck_cards [i].tableauKCard.transform.localPosition.y, -deck_cards [i].tableauIndex);
          deck_cards [i].tableauKCard.transform.localPosition = newPos3;
          deck_cards [i].tableauPeek = true;
          deck_cards [i].foundation = deck_cards [j].foundation;
          deck_cards [i].wasteActivePos = -1;
          deck_cards [i].tableauFaceUp = true;
          //Debug.Log("Clicked Card => " + CardString(deck_cards[i]));

          // Target card
          deck_cards [j].tableauPeek = false;


          UpdateUndoButton ();

          UpdateScore (deltaScore);

          UpdateFoundationColliders (deck_cards [i].suit);

          IsGameCompleted ();

          return true;

        }
      }
    }

    return false;
  }

  /*
	 public void OnFoundationClicked(int suit) // ahora esta solo deberia entrar cuando no hayan cartas, es decir rankonfoundation deberia ser -1, es decir no tiene sentido esta funcion ya
	{
			int deltaScore = 0;
			if (player_can_move && Input.touches.Length <= 1)
			{
					int foundationRankIndex = GetRankOnFoundation(suit);
					int foundationRank = (foundationRankIndex != -1) ? deck_cards[foundationRankIndex].rank : -1;

					if (foundationRank != -1)
					{
							// Find card board
							for (int i = 0; i < deck_card_left; i++)
							{
									if (deck_cards[i].tableauPeek && foundationRank == deck_cards[i].rank - 1 && MatchColor(deck_cards[i].suit, deck_cards[foundationRankIndex].suit) )
									{
											// Found
											ResetUndo();

											AddCardToUndo(foundationRankIndex,false);
											AddCardToUndo(i,false);

											//deltaScore += Foundation_To_Board;
											deltaScore -= GetScoreByRank(deck_cards[i].rank);
											//Vector3 scorepos = new Vector3(deck_cards[i].tableauKCard.transform.position.x, deck_cards[i].tableauKCard.transform.position.y + 0.6f, deck_cards[i].tableauKCard.transform.position.z);
											Vector3 scorepos = new Vector3(foundations[suit].transform.position.x, foundations[suit].transform.position.y - 1.05F, foundations[suit].transform.position.z); 
											//CreateScoreEvent(scorepos, Foundation_To_Board.ToString(scoreFormat));
											 CreateScoreEvent(scorepos, (-GetScoreByRank(deck_cards[i].rank)).ToString(scoreFormat));

											// Move from foundation to targetcard
											Vector3 nt2 = board.transform.InverseTransformPoint(foundations[suit].transform.position);
											Vector3 foundationPos = new Vector3(nt2.x, nt2.y, 0);

											Vector3 cardPos = new Vector3(deck_cards[i].tableauKCard.transform.localPosition.x, deck_cards[i].tableauKCard.transform.localPosition.y - tableau_back_offset, deck_cards[i].tableauKCard.transform.localPosition.z);
											StartCoroutine(MoveCardTo(deck_cards[foundationRankIndex].tableauKCard, foundationPos, cardPos));

											// Update foundation card
											deck_cards[foundationRankIndex].tableauIndex = deck_cards[i].tableauIndex+1;
											Vector3 newPos3 = new Vector3(deck_cards[foundationRankIndex].tableauKCard.transform.localPosition.x, deck_cards[foundationRankIndex].tableauKCard.transform.localPosition.y, -deck_cards[foundationRankIndex].tableauIndex);
											deck_cards[foundationRankIndex].tableauKCard.transform.localPosition = newPos3;
											deck_cards[foundationRankIndex].tableauPeek = true;
											deck_cards[foundationRankIndex].foundation = deck_cards[i].foundation;
											deck_cards[foundationRankIndex].wasteActivePos = -1;
											deck_cards[foundationRankIndex].tableauFaceUp = true;
											//deck_cards[foundationRankIndex].tableauKCard.EnableCollider(true);

											// Update target card
											deck_cards[i].tableauPeek = false;

											UpdateScore(deltaScore);

											UpdateFoundationColliders(deck_cards[foundationRankIndex].suit);

											IsGameCompleted();

											break;
									}
							}
					}
			}
	}
	 */

  bool PlayWasteActiveToFoundation (int wasteActiveIndex, wasteActive wa, int suit = -1)
  {
    int deltaScore = 0;

    if (wasteActiveIndex != -1 && deck_cards [wasteActiveIndex].wasteActivePos == wa.wasteActiveIndex && (deck_cards [wasteActiveIndex].suit == suit || suit == -1)) {
      int targetRankIndex = GetRankOnFoundation (deck_cards [wasteActiveIndex].suit);
      int targetRank = (targetRankIndex != -1) ? deck_cards [targetRankIndex].rank : -1;

      if (targetRank == deck_cards [wasteActiveIndex].rank - 1) {
        AddUndoStep ();

        // Save the current status but not add to undo list yet
        card_class cc = new card_class ();
        cc.suit = deck_cards [wasteActiveIndex].suit;
        cc.rank = deck_cards [wasteActiveIndex].rank;
        cc.foundation = deck_cards [wasteActiveIndex].foundation;
        cc.wasteActivePos = deck_cards [wasteActiveIndex].wasteActivePos;
        cc.tableauFaceUp = deck_cards [wasteActiveIndex].tableauFaceUp;
        cc.tableauPeek = deck_cards [wasteActiveIndex].tableauPeek;
        cc.tableauIndex = deck_cards [wasteActiveIndex].tableauIndex;
        cc.destroyGOOnUndo = true;


        //bool wasIF = deck_cards[wasteActiveIndex].cardWasInFoundation;

        // Selected card
        deck_cards [wasteActiveIndex].tableauIndex = -1;
        deck_cards [wasteActiveIndex].tableauPeek = false;
        deck_cards [wasteActiveIndex].foundation = (card_foundation)(deck_cards [wasteActiveIndex].suit + 10);
        deck_cards [wasteActiveIndex].wasteActivePos = -1;
        deck_cards [wasteActiveIndex].tableauFaceUp = true;
        deck_cards [wasteActiveIndex].cardWasInFoundation = true;

        // Create new card
        Vector3 nt;
        Vector3 newPos;

        if (suit != -1) {
          // Dragging
          nt = GhostWasteActive_Image.transform.localPosition;
        } else {
          // Click
          nt = board.transform.InverseTransformPoint (wa.gameObject.transform.position);
        }
        newPos = new Vector3 (nt.x, nt.y, 0);

        deck_cards [wasteActiveIndex].tableauKCard = CreateCard (deck_cards [wasteActiveIndex].suit, deck_cards [wasteActiveIndex].rank, true, board.transform, newPos);
        //deck_cards[wasteActiveIndex].tableauKCard.EnableCollider(false);

        //if (!wasIF)
        {
          int ttf = GetScoreByRank (deck_cards [wasteActiveIndex].rank) + Waste_To_Foundation;
          deltaScore += ttf;
          Vector3 scorepos = new Vector3 (foundations [deck_cards [wasteActiveIndex].suit].transform.position.x, foundations [deck_cards [wasteActiveIndex].suit].transform.position.y - 1.05F, foundations [deck_cards [wasteActiveIndex].suit].transform.position.z);
          CreateScoreEvent (scorepos, ttf.ToString (scoreFormat), current_score + deltaScore);
        }


        // Move to foundation
        Vector3 nt2 = board.transform.InverseTransformPoint (foundations [deck_cards [wasteActiveIndex].suit].transform.position);
        Vector3 newPos2 = new Vector3 (nt2.x, nt2.y, 0);
        StartCoroutine (MoveCardTo (deck_cards [wasteActiveIndex].tableauKCard, deck_cards [wasteActiveIndex].tableauKCard.transform.localPosition, newPos2, animationSpeedTime, -1, MenuManager.SoundNamesEnum.CARD_TO_FOUNDATION_SOUND));

        ActivateWasteCard ();

        AddCardToUndo (cc);

        UpdateScore (deltaScore);

        UpdateFoundationColliders (deck_cards [wasteActiveIndex].suit);

        IsGameCompleted ();

        return true;
      }
    }

    return false;
  }

  bool PlayWasteActiveToFreePile (int wasteActiveIndex, wasteActive wa, int pi = -1)
  {
    int deltaScore = 0;

    if (deck_cards [wasteActiveIndex].rank == 12 && deck_cards [wasteActiveIndex].wasteActivePos == wa.wasteActiveIndex) { // K
      int pileIndex;

      if (pi == -1) {
        pileIndex = GetFirstEmtpyPile ();
      } else {
        pileIndex = IsEmptyPile (pi) ? pi : -1;
      }


      if (pileIndex != -1) {
        AddUndoStep ();

        // Save the current status but not add to undo list yet
        card_class cc = new card_class ();
        cc.suit = deck_cards [wasteActiveIndex].suit;
        cc.rank = deck_cards [wasteActiveIndex].rank;
        cc.foundation = deck_cards [wasteActiveIndex].foundation;
        cc.wasteActivePos = deck_cards [wasteActiveIndex].wasteActivePos;
        cc.tableauFaceUp = deck_cards [wasteActiveIndex].tableauFaceUp;
        cc.tableauPeek = deck_cards [wasteActiveIndex].tableauPeek;
        cc.tableauIndex = deck_cards [wasteActiveIndex].tableauIndex;
        cc.destroyGOOnUndo = true;

        // Create new card
        Vector3 nt;
        Vector3 newPos;

        if (pi != -1) {
          // Dragging
          nt = GhostWasteActive_Image.transform.localPosition;
        } else {
          // Click
          nt = board.transform.InverseTransformPoint (wa.gameObject.transform.position);
        }
        newPos = new Vector3 (nt.x, nt.y, 0);


        deck_cards [wasteActiveIndex].tableauKCard = CreateCard (deck_cards [wasteActiveIndex].suit, deck_cards [wasteActiveIndex].rank, true, board.transform, newPos);


        deltaScore += Waste_To_Board;
        //Vector3 scorepos = new Vector3(wa.gameObject.transform.position.x + 0.5F, wa.gameObject.transform.position.y, wa.gameObject.transform.position.z);
        Vector3 scorePos = new Vector3 (boardPiles [pileIndex].transform.position.x, boardPiles [pileIndex].transform.position.y, boardPiles [pileIndex].transform.position.z);
        CreateScoreEvent (scorePos, Waste_To_Board.ToString (scoreFormat), current_score + deltaScore);


        // Move to the empty pile
        Vector3 newPos2 = new Vector3 (boardPiles [pileIndex].transform.localPosition.x, boardPiles [pileIndex].transform.localPosition.y, 0);
        StartCoroutine (MoveCardTo (deck_cards [wasteActiveIndex].tableauKCard, deck_cards [wasteActiveIndex].tableauKCard.transform.localPosition, newPos2, animationSpeedTime, -1, MenuManager.SoundNamesEnum.TAKE_CARD_SOUND));

        // Selected card
        deck_cards [wasteActiveIndex].tableauIndex = 0;
        deck_cards [wasteActiveIndex].tableauPeek = true;
        deck_cards [wasteActiveIndex].foundation = (card_foundation)(pileIndex + 3);
        deck_cards [wasteActiveIndex].wasteActivePos = -1;
        deck_cards [wasteActiveIndex].tableauFaceUp = true;

        ActivateWasteCard ();

        AddCardToUndo (cc);

        UpdateScore (deltaScore);

        IsGameCompleted ();

        return true;
      }
    }
    return false;
  }

  bool PlayWasteActiveToPile (int wasteActiveIndex, wasteActive wa, int oi = -1)
  {
    int deltaScore = 0;

    if (deck_cards [wasteActiveIndex].rank != 12 && deck_cards [wasteActiveIndex].wasteActivePos == wa.wasteActiveIndex) { // Not K
      int init = (oi == -1) ? 0 : oi;
      int end = (oi == -1) ? deck_card_left : oi + 1;

      // Check if this card can be moved to a pile
      for (int j = init; j < end; j++) {
        if (deck_cards [j].foundation != deck_cards [wasteActiveIndex].foundation &&
        IsOnTableau (j) &&
        deck_cards [j].tableauPeek &&
        MatchColor (deck_cards [j].suit, deck_cards [wasteActiveIndex].suit) &&
        deck_cards [j].rank == deck_cards [wasteActiveIndex].rank + 1) {
          AddUndoStep ();

          // Save the current status but not add to undo list yet
          card_class cc = new card_class ();
          cc.suit = deck_cards [wasteActiveIndex].suit;
          cc.rank = deck_cards [wasteActiveIndex].rank;
          cc.foundation = deck_cards [wasteActiveIndex].foundation;
          cc.wasteActivePos = deck_cards [wasteActiveIndex].wasteActivePos;
          cc.tableauFaceUp = deck_cards [wasteActiveIndex].tableauFaceUp;
          cc.tableauPeek = deck_cards [wasteActiveIndex].tableauPeek;
          cc.tableauIndex = deck_cards [wasteActiveIndex].tableauIndex;
          cc.destroyGOOnUndo = true;

          // Create new card
          Vector3 nt;
          Vector3 newPos;

          if (oi != -1) {
            // Dragging
            nt = GhostWasteActive_Image.transform.localPosition;
          } else {
            // Click
            nt = board.transform.InverseTransformPoint (wa.gameObject.transform.position);
          }
          newPos = new Vector3 (nt.x, nt.y, 0);
          deck_cards [wasteActiveIndex].tableauKCard = CreateCard (deck_cards [wasteActiveIndex].suit, deck_cards [wasteActiveIndex].rank, true, board.transform, newPos);


          deltaScore += Waste_To_Board;
          //Vector3 scorepos = new Vector3(wa.gameObject.transform.position.x + 0.5F, wa.gameObject.transform.position.y, wa.gameObject.transform.position.z);
          Vector3 scorePos = new Vector3 (deck_cards [j].tableauKCard.transform.position.x, deck_cards [j].tableauKCard.transform.position.y, deck_cards [j].tableauKCard.transform.position.z);
          CreateScoreEvent (scorePos, Waste_To_Board.ToString (scoreFormat), current_score + deltaScore);

          // Move to the pile
          Vector3 newPos2 = new Vector3 (deck_cards [j].tableauKCard.transform.localPosition.x, deck_cards [j].tableauKCard.transform.localPosition.y - tableau_normal_offset, deck_cards [j].tableauKCard.transform.localPosition.z);
          StartCoroutine (MoveCardTo (deck_cards [wasteActiveIndex].tableauKCard, deck_cards [wasteActiveIndex].tableauKCard.transform.localPosition, newPos2, animationSpeedTime, -1, MenuManager.SoundNamesEnum.TAKE_CARD_SOUND));

          // Selected card
          deck_cards [wasteActiveIndex].tableauIndex = GetPeekIndex (deck_cards [j].foundation) + 1;
          Vector3 newPos3 = new Vector3 (deck_cards [wasteActiveIndex].tableauKCard.transform.localPosition.x, deck_cards [wasteActiveIndex].tableauKCard.transform.localPosition.y, -deck_cards [wasteActiveIndex].tableauIndex);
          deck_cards [wasteActiveIndex].tableauKCard.transform.localPosition = newPos3;
          deck_cards [wasteActiveIndex].tableauPeek = true;
          deck_cards [wasteActiveIndex].foundation = deck_cards [j].foundation;
          deck_cards [wasteActiveIndex].wasteActivePos = -1;
          deck_cards [wasteActiveIndex].tableauFaceUp = true;
          //Debug.Log("Clicked Card => " + CardString(deck_cards[i]));


          // Target card
          AddCardToUndo (j, false);
          deck_cards [j].tableauPeek = false;

          ActivateWasteCard ();

          AddCardToUndo (cc);

          UpdateScore (deltaScore);

          IsGameCompleted ();

          return true;
        }
      }
    }

    return false;
  }


  public void OnWasteActiveClicked (wasteActive wa)
  {
    if (player_can_move && Input.touches.Length <= 1) {
      int wasteActiveIndex = GetCardOnActiveWaste ();

      if (wasteActiveIndex != -1) {
        if (!PlayWasteActiveToFoundation (wasteActiveIndex, wa)) {
          if (!PlayWasteActiveToFreePile (wasteActiveIndex, wa)) {
            if (!PlayWasteActiveToPile (wasteActiveIndex, wa)) {
              WrongCard (waste_cards [deck_cards [wasteActiveIndex].wasteActivePos].gameObject, true);
            }
          }
        }
      }

      initDrag = Vector2.zero;
      initDraggedCard = null;
      if (initDraggedWasteActive) {
        initDraggedWasteActive.gameObject.GetComponent<Image> ().enabled = true;
      }
      initDraggedWasteActive = null;
      GhostWasteActive_Image.gameObject.SetActive (false);
      draggedCards.Clear ();
    }
  }

  void PlayDeckToWasteActive (bool canUndoDeckAction)
  {
    int availableDeckCardsCount = 0;
    int[] availabeDeckCardIndexes = GetAvailabeCardsOnDeck (ref availableDeckCardsCount);

    int activeWasteCardsCount = 0;
    int[] activeCardIndexes = GetActiveCardsOnWaste (ref activeWasteCardsCount);
    //   0   1   2 
    //  []  []  []

    bool undo = activeWasteCardsCount != 0 && canUndoDeckAction;

    switch (availableDeckCardsCount) {
    case 0:
      {
        bool cardsOnWaste = false;

        // Reset deck
        for (int x = 0; x < deck_card_left; x++) {
          if (deck_cards [x].foundation == card_foundation.waste_active || deck_cards [x].foundation == card_foundation.waste) {
            if (!cardsOnWaste) {
              if (undo) {
                AddUndoStep ();
              }
              cardsOnWaste = true;
            }

            if (undo) {
              AddCardToUndo (x, false);
            }
            deck_cards [x].foundation = card_foundation.deck;
            deck_cards [x].wasteActivePos = -1;
          }
        }

        if (cardsOnWaste) {
          StartCoroutine (UpdateActiveWasteSprites (0, true));
        }

        deckResetCount++;
        if (deckResetCount == 2) {
          int val = PlayerData.GetInstance ().GetValueAsInt ("klondikeTips0");
          if (val == 0) {
            KlondikeMenu.GetInstance ().OpenPopup (3);
          }
        }

        break;
      }
    case 1:
      {
        MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.NEW_CARD_SOUND);

        if (undo) {
          AddUndoStep ();

          // Add card to undo list
          AddCardToUndo (availabeDeckCardIndexes [0], false);
        }


        if (activeWasteCardsCount == 3) {
          // Update card foundations and start animations
          for (int x = 0; x < activeWasteCardsCount; x++) {
            // Add cards to undo list
            if (undo) {
              AddCardToUndo (activeCardIndexes [x], false);
            }

            if (deck_cards [activeCardIndexes [x]].wasteActivePos == 0) {
              deck_cards [activeCardIndexes [x]].foundation = card_foundation.waste;
              deck_cards [activeCardIndexes [x]].wasteActivePos = -1;
            } else if (deck_cards [activeCardIndexes [x]].wasteActivePos == 1) {
              deck_cards [activeCardIndexes [x]].foundation = card_foundation.waste_active;
              deck_cards [activeCardIndexes [x]].wasteActivePos = 0;
              StartCoroutine (MoveDeckCard (deck_cards [activeCardIndexes [x]].suit, deck_cards [activeCardIndexes [x]].rank, false, 1, 0));
            } else if (deck_cards [activeCardIndexes [x]].wasteActivePos == 2) {
              deck_cards [activeCardIndexes [x]].foundation = card_foundation.waste_active;
              deck_cards [activeCardIndexes [x]].wasteActivePos = 1;
              StartCoroutine (MoveDeckCard (deck_cards [activeCardIndexes [x]].suit, deck_cards [activeCardIndexes [x]].rank, false, 2, 1));
            }
          }
        }

        // Update card foundation and animation
        deck_cards [availabeDeckCardIndexes [0]].foundation = card_foundation.waste_active;
        deck_cards [availabeDeckCardIndexes [0]].wasteActivePos = ((activeWasteCardsCount == 3) ? 2 : activeWasteCardsCount);
        StartCoroutine (MoveDeckCard (deck_cards [availabeDeckCardIndexes [0]].suit, deck_cards [availabeDeckCardIndexes [0]].rank, true, 0, (activeWasteCardsCount == 3) ? 2 : activeWasteCardsCount));

        UpdateDeckBackImage ();

        StartCoroutine (UpdateActiveWasteSprites (animationSpeedTime));

        break;
      }
    case 2:
      {
        MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.NEW_THREE_CARD_SOUND);

        if (undo) {
          AddUndoStep ();
        }

        if (activeWasteCardsCount == 0) {
          // Just move the 2 cards
          for (int x = 0; x < availableDeckCardsCount; x++) {

            // Add card to undo list
            if (undo) {
              AddCardToUndo (availabeDeckCardIndexes [x], false);
            }

            // Update card foundation and animation
            deck_cards [availabeDeckCardIndexes [x]].foundation = card_foundation.waste_active;
            deck_cards [availabeDeckCardIndexes [x]].wasteActivePos = x;
            StartCoroutine (MoveDeckCard (deck_cards [availabeDeckCardIndexes [x]].suit, deck_cards [availabeDeckCardIndexes [x]].rank, true, 0, x, timeBetwenDeckCardAnimations * x));
          }
        } else {

          if (activeWasteCardsCount == 2) {
            // Update card foundations and start animations
            for (int x = 0; x < activeWasteCardsCount; x++) {
              // Add cards to undo list
              if (undo) {
                AddCardToUndo (activeCardIndexes [x], false);
              }

              if (deck_cards [activeCardIndexes [x]].wasteActivePos == 0) {
                deck_cards [activeCardIndexes [x]].foundation = card_foundation.waste;
                deck_cards [activeCardIndexes [x]].wasteActivePos = -1;
              } else if (deck_cards [activeCardIndexes [x]].wasteActivePos == 1) {
                deck_cards [activeCardIndexes [x]].foundation = card_foundation.waste_active;
                deck_cards [activeCardIndexes [x]].wasteActivePos = 0;
                StartCoroutine (MoveDeckCard (deck_cards [activeCardIndexes [x]].suit, deck_cards [activeCardIndexes [x]].rank, false, 1, 0));
              }
            }
          } else if (activeWasteCardsCount == 3) {
            // Update card foundations and start animations
            for (int x = 0; x < activeWasteCardsCount; x++) {
              // Add cards to undo list
              if (undo) {
                AddCardToUndo (activeCardIndexes [x], false);
              }

              if (deck_cards [activeCardIndexes [x]].wasteActivePos == 0) {
                deck_cards [activeCardIndexes [x]].foundation = card_foundation.waste;
                deck_cards [activeCardIndexes [x]].wasteActivePos = -1;
              } else if (deck_cards [activeCardIndexes [x]].wasteActivePos == 1) {
                deck_cards [activeCardIndexes [x]].foundation = card_foundation.waste;
                deck_cards [activeCardIndexes [x]].wasteActivePos = -1;
                StartCoroutine (MoveDeckCard (deck_cards [activeCardIndexes [x]].suit, deck_cards [activeCardIndexes [x]].rank, false, 1, 0));
              } else if (deck_cards [activeCardIndexes [x]].wasteActivePos == 2) {
                deck_cards [activeCardIndexes [x]].foundation = card_foundation.waste_active;
                deck_cards [activeCardIndexes [x]].wasteActivePos = 0;
                StartCoroutine (MoveDeckCard (deck_cards [activeCardIndexes [x]].suit, deck_cards [activeCardIndexes [x]].rank, false, 2, 0));
              }
            }
          }


          // Move the 2 cards
          for (int x = availableDeckCardsCount - 1; x >= 0; x--) {

            // Add card to undo list
            if (undo) {
              AddCardToUndo (availabeDeckCardIndexes [x], false);
            }

            // Update card foundation and animation
            deck_cards [availabeDeckCardIndexes [x]].foundation = card_foundation.waste_active;
            deck_cards [availabeDeckCardIndexes [x]].wasteActivePos = x + 1;
            StartCoroutine (MoveDeckCard (deck_cards [availabeDeckCardIndexes [x]].suit, deck_cards [availabeDeckCardIndexes [x]].rank, true, 0, x + 1, timeBetwenDeckCardAnimations * x));
          }
        }

        UpdateDeckBackImage ();
        StartCoroutine (UpdateActiveWasteSprites (animationSpeedTime + (timeBetwenDeckCardAnimations * (availableDeckCardsCount - 1))));

        break;
      }
    default: //3+
      {
        MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.NEW_THREE_CARD_SOUND);

        if (undo) {
          AddUndoStep ();
        }

        // Update card foundations and start animations
        for (int x = 0; x < activeWasteCardsCount; x++) {
          // Add cards to undo list
          if (undo) {
            AddCardToUndo (activeCardIndexes [x], false);
          }

          if (deck_cards [activeCardIndexes [x]].wasteActivePos == 0) {
            deck_cards [activeCardIndexes [x]].foundation = card_foundation.waste;
            deck_cards [activeCardIndexes [x]].wasteActivePos = -1;
          } else if (deck_cards [activeCardIndexes [x]].wasteActivePos == 1) {
            deck_cards [activeCardIndexes [x]].foundation = card_foundation.waste;
            deck_cards [activeCardIndexes [x]].wasteActivePos = -1;
            StartCoroutine (MoveDeckCard (deck_cards [activeCardIndexes [x]].suit, deck_cards [activeCardIndexes [x]].rank, false, 1, 0));
          } else if (deck_cards [activeCardIndexes [x]].wasteActivePos == 2) {
            deck_cards [activeCardIndexes [x]].foundation = card_foundation.waste;
            deck_cards [activeCardIndexes [x]].wasteActivePos = -1;
            StartCoroutine (MoveDeckCard (deck_cards [activeCardIndexes [x]].suit, deck_cards [activeCardIndexes [x]].rank, false, 2, 0));
          }
        }

        // Move the 3 cards
        for (int x = availableDeckCardsCount - 1; x >= 0; x--) {

          // Add card to undo list
          if (undo) {
            AddCardToUndo (availabeDeckCardIndexes [x], false);
          }

          // Update card foundation and animation
          deck_cards [availabeDeckCardIndexes [x]].foundation = card_foundation.waste_active;
          deck_cards [availabeDeckCardIndexes [x]].wasteActivePos = x;
          StartCoroutine (MoveDeckCard (deck_cards [availabeDeckCardIndexes [x]].suit, deck_cards [availabeDeckCardIndexes [x]].rank, true, 0, x, timeBetwenDeckCardAnimations * x));
        }


        UpdateDeckBackImage ();
        StartCoroutine (UpdateActiveWasteSprites (animationSpeedTime + (timeBetwenDeckCardAnimations * (availableDeckCardsCount - 1))));

        break;
      }
    }

    //Debug.Log("new");
    //PrintDeckCardsArray();
    //UpdateDeckBackImage();
    IsGameCompleted ();
  }

  void OnMouseDown ()// Deck => Waste Active
  {
    if (player_can_move && Input.touches.Length <= 1) {
      PlayDeckToWasteActive (true);
    }
  }

  void UpdateDeckBackImage ()
  {
    int navailableDeckCardsCount = 0;
    int[] navailabeDeckCardIndexes = GetAvailabeCardsOnDeck (ref navailableDeckCardsCount);
    this.gameObject.GetComponent<Image> ().enabled = (navailableDeckCardsCount >= 1);
  }

  public IEnumerator MoveUndoDeckCard (int suit, int rank, bool fromwaste, int startIndex, int endIndex, float initWait = 0.0F)
  {
    player_can_move = false;
    animationsCount++;

    if (initWait != 0) {
      yield return new WaitForSeconds (initWait);
    }

    // From waste active to deck
    if (fromwaste) {

      switch (startIndex) {
      case 0:
        {
          //waste_card0.gameObject.SetActive(false); 
          FixGOS ();
          new_card_anim_sprite0.sprite = card_deck_sprite [suit, rank];
          new_card_anim0.gameObject.SetActive (true);
          new_card_anim0.Play ("undo_to_deck_card_anim");

          break;
        }
      case 1:
        {

          //waste_card1.gameObject.SetActive(false);
          FixGOS ();
          new_card_anim_sprite1.sprite = card_deck_sprite [suit, rank];
          new_card_anim1.gameObject.SetActive (true);
          new_card_anim1.Play ("undo_to_deck_card_anim");
          break;
        }
      case 2:
        {
          //waste_card2.gameObject.SetActive(false);
          FixGOS ();
          new_card_anim_sprite2.sprite = card_deck_sprite [suit, rank];
          new_card_anim2.gameObject.SetActive (true);
          new_card_anim2.Play ("undo_to_deck_card_anim");
          break;
        }
      }

      yield return new WaitForSeconds (animationSpeedTime - 0.01f);
    }
		// From waste to waste_active  to waste_active
		else {
      switch (startIndex) {
      case 0:
        {
          if (endIndex == 1) {
            //waste_card0.gameObject.SetActive(false);
            waste_card1.gameObject.SetActive (false);
            waste_card_anim_sprite1.sprite = card_deck_sprite [suit, rank];
            waste_card_anim1.gameObject.SetActive (true);
            waste_card_anim1.Play ("deck_card_0_to_1");

            break;
          } else if (endIndex == 2) {
            //waste_card0.gameObject.SetActive(false);
            waste_card2.gameObject.SetActive (false);
            waste_card_anim_sprite2.sprite = card_deck_sprite [suit, rank];
            waste_card_anim2.gameObject.SetActive (true);
            waste_card_anim2.Play ("deck_card_0_to_2");

          }
          break;
        }
      case 1:
        {
          if (endIndex == 0) {
            waste_card1.gameObject.SetActive (false);
            waste_card0.gameObject.SetActive (false);
            waste_card_anim_sprite1.sprite = card_deck_sprite [suit, rank];
            waste_card_anim1.gameObject.SetActive (true);
            waste_card_anim1.Play ("deck_card_1_to_0");

          } else if (endIndex == 2) {
            waste_card1.gameObject.SetActive (false);
            waste_card2.gameObject.SetActive (false);
            waste_card_anim_sprite2.sprite = card_deck_sprite [suit, rank];
            waste_card_anim2.gameObject.SetActive (true);
            waste_card_anim2.Play ("deck_card_1_to_2");
          }
          break;
        }
      case 2:
        {
          if (endIndex == 0) {
            waste_card2.gameObject.SetActive (false);
            waste_card0.gameObject.SetActive (false);
            waste_card_anim_sprite2.sprite = card_deck_sprite [suit, rank];
            waste_card_anim2.gameObject.SetActive (true);
            waste_card_anim2.Play ("deck_card_2_to_0");
          } else if (endIndex == 1) {
            waste_card2.gameObject.SetActive (false);
            waste_card1.gameObject.SetActive (false);
            waste_card_anim_sprite2.sprite = card_deck_sprite [suit, rank];
            waste_card_anim2.gameObject.SetActive (true);
            waste_card_anim2.Play ("deck_card_2_to_1");
          }

          break;
        }
      }

      yield return new WaitForSeconds (animationSpeedTime);
      waste_cards [endIndex].sprite = card_deck_sprite [suit, rank];
      waste_cards [endIndex].gameObject.SetActive (true);
    }


    animationsCount--;
    CheckPlayerCanMove ();

    yield return null;
  }

  public void FixGOS ()
  {
    bool[] found = new bool[3] { false, false, false };

    for (int i = 0; i < deck_card_left; i++) {
      if (deck_cards [i].foundation == card_foundation.waste_active && deck_cards [i].wasteActivePos != -1) {
        waste_cards [deck_cards [i].wasteActivePos].sprite = card_deck_sprite [deck_cards [i].suit, deck_cards [i].rank];
        found [deck_cards [i].wasteActivePos] = true;
      }
    }

    for (int j = 0; j < 3; j++) {
      waste_cards [j].gameObject.SetActive (found [j]);
    }
  }

  public IEnumerator UpdateActiveWasteSprites (float initWait = 0.0F, bool fixGOS = false)
  {
    if (initWait > 0) {
      yield return new WaitForSeconds (initWait /* - 0.02f*/);
    }

    if (fixGOS) {
      FixGOS ();
    }

    UpdateUndoButton ();
    UpdateDeckBackImage ();
  }

  void UpdateUndoButton ()
  {
    undo_button.gameObject.SetActive (undo_list.Count > 0);
  }

  public IEnumerator MoveDeckCard (int suit, int rank, bool fromdeck, int startIndex, int endIndex, float initDelay = 0.0F)
  {
    player_can_move = false;
    animationsCount++;

    if (initDelay != 0) {
      yield return new WaitForSeconds (initDelay);
    }

    // From deck to waste
    if (fromdeck) {

      switch (endIndex) {
      case 0:
        {
          new_card_anim_sprite0.sprite = card_deck_sprite [suit, rank];
          new_card_anim0.gameObject.SetActive (true);
          new_card_anim0.Play ("new_card_anim");
          break;
        }
      case 1:
        {
          new_card_anim_sprite1.sprite = card_deck_sprite [suit, rank];
          new_card_anim1.gameObject.SetActive (true);
          new_card_anim1.Play ("new_card_anim");
          break;
        }
      case 2:
        {
          new_card_anim_sprite2.sprite = card_deck_sprite [suit, rank];
          new_card_anim2.gameObject.SetActive (true);
          new_card_anim2.Play ("new_card_anim");
          break;
        }
      }

      yield return new WaitForSeconds (animationSpeedTime);
      waste_cards [endIndex].sprite = card_deck_sprite [suit, rank];
      waste_cards [endIndex].gameObject.SetActive (true);
    }
		// From waste to waste
		else {
      if (startIndex == 0) {
        if (endIndex == 1) {
          waste_card0.gameObject.SetActive (false);
          waste_card_anim_sprite1.sprite = card_deck_sprite [suit, rank];
          waste_card_anim1.gameObject.SetActive (true);
          waste_card_anim1.Play ("deck_card_0_to_1");
        }
      } else if (startIndex == 1) {
        if (endIndex == 0) {
          waste_card1.gameObject.SetActive (false);
          waste_card_anim_sprite1.sprite = card_deck_sprite [suit, rank];
          waste_card_anim1.gameObject.SetActive (true);
          waste_card_anim1.Play ("deck_card_1_to_0");
        } else if (endIndex == 2) {
          waste_card2.gameObject.SetActive (false);
          waste_card_anim_sprite2.sprite = card_deck_sprite [suit, rank];
          waste_card_anim2.gameObject.SetActive (true);
          waste_card_anim2.Play ("deck_card_1_to_2");
        }
      } else if (startIndex == 2) {
        if (endIndex == 0) {
          waste_card2.gameObject.SetActive (false);
          waste_card_anim_sprite2.sprite = card_deck_sprite [suit, rank];
          waste_card_anim2.gameObject.SetActive (true);
          waste_card_anim2.Play ("deck_card_2_to_0");
        } else if (endIndex == 1) {
          waste_card2.gameObject.SetActive (false);
          waste_card_anim_sprite2.sprite = card_deck_sprite [suit, rank];
          waste_card_anim2.gameObject.SetActive (true);
          waste_card_anim2.Play ("deck_card_2_to_1");
        }
      }

      yield return new WaitForSeconds (animationSpeedTime - 0.01f);
      waste_cards [endIndex].sprite = card_deck_sprite [suit, rank];
      waste_cards [endIndex].gameObject.SetActive (true);
    }


    animationsCount--;
    CheckPlayerCanMove ();

    yield return null;
  }

  void PrintTime ()
  {
    int ctime = Mathf.CeilToInt (left_time);
    if (ctime != int_time) {
      int_time = ctime;

      int seconds = ctime % 60;
      int minutes = ctime / 60;
      bonus_text.text = string.Format ("{0}:{1:00}", minutes, seconds); //minutes + ":" + seconds;
    }
  }

  // Update is called once per frame
  void Update ()
  {
    if (left_time > 0 && !game_end && !timer_paused) {
      left_time -= Time.deltaTime;

      PrintTime ();

      if (left_time <= 0) {
        left_time = 0;
        StartCoroutine (GameOver (1));
      }
    }

    if (draggedCards.Count != 0) {
      if (initDraggedCard) {
        Vector2 mousePos = MouseInBoard ();
        for (int w = 0; w < draggedCards.Count; w++) {
          if (deck_cards [draggedCards [w].index].tableauKCard) {
            deck_cards [draggedCards [w].index].tableauKCard.transform.localPosition = new Vector3 (mousePos.x - initOffset.x, mousePos.y - initOffset.y - (tableau_normal_offset * w), deck_cards [draggedCards [w].index].tableauKCard.transform.localPosition.z);
          }
        }
      } else if (initDraggedWasteActive) {
        Vector2 mousePos = MouseInBoard ();
        if (initDraggedWasteActive) {
          GhostWasteActive_Image.transform.localPosition = new Vector3 (mousePos.x - initOffset.x, mousePos.y - initOffset.y, GhostWasteActive_Image.transform.localPosition.z);
        }
      }
    } else if (initDrag != Vector2.zero) {
      // Find card in deck_cards
      if (initDraggedCard) {
        Vector2 mousePos = MouseInBoard ();
        if (CanDrag (mousePos)) {
          for (int i = 0; i < deck_card_left; i++) {
            if (deck_cards [i].tableauKCard == initDraggedCard) {
              Vector2 cpos = new Vector2 (deck_cards [i].tableauKCard.transform.localPosition.x, deck_cards [i].tableauKCard.transform.localPosition.y);
              initOffset = mousePos - cpos;

              dragInfo info = new dragInfo ();
              info.initialPos = new Vector3 (deck_cards [i].tableauKCard.transform.localPosition.x, deck_cards [i].tableauKCard.transform.localPosition.y, deck_cards [i].tableauKCard.transform.localPosition.z);
              info.index = i;
              deck_cards [i].tableauKCard.transform.SetAsLastSibling ();
              draggedCards.Add (info);

              // Move the cards under this card, save indexes in a list for update values after make other operations.
              List<int> indexes = new List<int> ();
              if (!deck_cards [i].tableauPeek) {
                // Card in tableau, find cards under this one
                for (int k = 0; k < deck_card_left; k++) {
                  if (k != i && deck_cards [k].foundation == deck_cards [i].foundation && deck_cards [k].tableauIndex > deck_cards [i].tableauIndex) {
                    indexes.Add (k);
                  }
                }

                // Very important, sort because MoveCardTo puts the cards on top of hierarchy
                indexes.Sort ((a, b) => deck_cards [a].tableauIndex.CompareTo (deck_cards [b].tableauIndex));

                for (int l = 0; l < indexes.Count; l++) {
                  int underCardIndex = indexes [l];

                  dragInfo info2 = new dragInfo ();
                  info2.initialPos = new Vector3 (deck_cards [underCardIndex].tableauKCard.transform.localPosition.x, deck_cards [underCardIndex].tableauKCard.transform.localPosition.y, deck_cards [underCardIndex].tableauKCard.transform.localPosition.z);
                  info2.index = underCardIndex;
                  deck_cards [underCardIndex].tableauKCard.transform.SetAsLastSibling ();
                  draggedCards.Add (info2);
                }
              }

              break;
            }
          }
        }
      } else if (initDraggedWasteActive) {
        Vector2 mousePos = MouseInBoard ();
        if (CanDrag (mousePos)) {
          Vector3 nt = board.transform.InverseTransformPoint (initDraggedWasteActive.gameObject.transform.position);
          Vector3 localPos = new Vector3 (nt.x, nt.y, 0);

          Vector2 cpos = new Vector2 (localPos.x, localPos.y);
          initOffset = mousePos - cpos;

          GhostWasteActive_Image.sprite = initDraggedWasteActive.gameObject.GetComponent<Image> ().sprite;
          GhostWasteActive_Image.gameObject.SetActive (true);
          GhostWasteActive_Image.transform.localPosition = localPos;
          GhostWasteActive_Image.transform.SetAsLastSibling ();

          initDraggedWasteActive.gameObject.GetComponent<Image> ().enabled = false;

          dragInfo info = new dragInfo ();
          info.initialPos = localPos;
          info.index = -1;
          draggedCards.Add (info);
        }
      }
    }
  }

  public Vector2 MouseInBoard ()
  {
    Vector2 localPoint = new Vector2 ();
    Vector2 mousePos;
    if (Input.touches.Length >= 1) {
      mousePos = Input.touches [0].position;
    } else {
      mousePos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
    }
    RectTransformUtility.ScreenPointToLocalPointInRectangle (board.gameObject.GetComponent<RectTransform> (), mousePos, Camera.main, out localPoint);
    return localPoint;
  }

  public Vector2 MouseInDeck ()
  {
    Vector2 localPoint = new Vector2 ();
    Vector2 mousePos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
    RectTransformUtility.ScreenPointToLocalPointInRectangle (this.gameObject.GetComponent<RectTransform> (), mousePos, Camera.main, out localPoint);
    return localPoint;
  }

  int GetCardsOnFoundations ()
  {
    int num = 0;
    for (int x = 0; x < deck_card_left; x++) {
      if (IsOnFoundation (x)) {
        num++;
      }
    }
    return num;
  }

  bool IsGameCompleted ()
  {
    for (int x = 0; x < deck_card_left; x++) {
      if (deck_cards [x].foundation == card_foundation.deck || deck_cards [x].foundation == card_foundation.waste) {
        return false;
      } else if (IsOnTableau (x) && !deck_cards [x].tableauFaceUp) {
        return false;
      }
    }

    // All cards on waste_active or in tableau faced up.
    undo_list.Clear ();
    UpdateUndoButton ();
    StartCoroutine (AutoPlay ());
    return true;
  }

  public IEnumerator AutoPlay ()
  {
    // Game Completed, start auto-play.
    MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.WIN_SOUND, false, 1);
    game_end = true;
    player_can_move = false;

    yield return new WaitForSeconds (0.3f);

    NextAutoPlaySequence ();
  }



  void NextAutoPlaySequence ()
  {
    //Debug.Log("NEXTAUTOPLAYSEQUENCE");
    bool found = false;
    int suitCycle = 0;
    while (!found && suitCycle < 4) {
      int targetRankIndex = GetRankOnFoundation (nextSuit);
      int targetRank = (targetRankIndex != -1) ? deck_cards [targetRankIndex].rank : -1;

      int activeWasteCardsCount = 0;
      int[] activeCardIndexes = GetActiveCardsOnWaste (ref activeWasteCardsCount);

      if (targetRank != 12) {
        for (int x = 0; x < deck_card_left; x++) {
          if (x != targetRankIndex && deck_cards [x].suit == nextSuit && targetRank == (deck_cards [x].rank - 1)) {
            if (deck_cards [x].foundation == card_foundation.waste_active /*&& deck_cards[x].wasteActivePos <= activeWasteCardsCount - 1*/) {
              int deltaScore = 0;

              Image img = waste_cards [deck_cards [x].wasteActivePos];

              //bool wasIF = deck_cards[x].cardWasInFoundation;

              // Selected card
              deck_cards [x].tableauIndex = -1;
              deck_cards [x].tableauPeek = false;
              deck_cards [x].foundation = (card_foundation)(deck_cards [x].suit + 10);
              deck_cards [x].wasteActivePos = -1;
              deck_cards [x].tableauFaceUp = true;
              deck_cards [x].cardWasInFoundation = true;

              // Create new card
              Vector3 nt;
              Vector3 newPos;

              // Click
              nt = board.transform.InverseTransformPoint (img.gameObject.transform.position);
              newPos = new Vector3 (nt.x, nt.y, 0);

              deck_cards [x].tableauKCard = CreateCard (deck_cards [x].suit, deck_cards [x].rank, true, board.transform, newPos);
              img.gameObject.SetActive (false);
              //deck_cards[wasteActiveIndex].tableauKCard.EnableCollider(false);

              //if (!wasIF)
              {
                int ttf = GetScoreByRank (deck_cards [x].rank) + Waste_To_Foundation;
                deltaScore += ttf;
                Vector3 scorepos = new Vector3 (foundations [deck_cards [x].suit].transform.position.x, foundations [deck_cards [x].suit].transform.position.y - 1.05F, foundations [deck_cards [x].suit].transform.position.z);
                CreateScoreEvent (scorepos, ttf.ToString (scoreFormat), current_score + deltaScore);
              }

              // Move to foundation
              Vector3 nt2 = board.transform.InverseTransformPoint (foundations [deck_cards [x].suit].transform.position);
              Vector3 newPos2 = new Vector3 (nt2.x, nt2.y, 0);
              StartCoroutine (AutoPlayMoveCardTo (deck_cards [x].tableauKCard, deck_cards [x].tableauKCard.transform.localPosition, newPos2));

              UpdateScore (deltaScore);

              UpdateFoundationColliders (deck_cards [x].suit);

              found = true;
              break;

            } else if (IsOnTableau (x) /*&& deck_cards[x].tableauPeek*/) {
              int deltaScore = 0;

              //if (!deck_cards[i].cardWasInFoundation)
              {
                int ttf = GetScoreByRank (deck_cards [x].rank);
                deltaScore += ttf;
                //Vector3 scorepos = new Vector3(deck_cards[i].tableauKCard.transform.position.x, deck_cards[i].tableauKCard.transform.position.y + 0.6f, deck_cards[i].tableauKCard.transform.position.z);
                Vector3 scorepos = new Vector3 (foundations [deck_cards [x].suit].transform.position.x, foundations [deck_cards [x].suit].transform.position.y - 1.05F, foundations [deck_cards [x].suit].transform.position.z);
                CreateScoreEvent (scorepos, ttf.ToString (scoreFormat), current_score + deltaScore);
              }

              Vector3 nt2 = board.transform.InverseTransformPoint (foundations [deck_cards [x].suit].transform.position);
              Vector3 newPos2 = new Vector3 (nt2.x, nt2.y, 0);
              StartCoroutine (AutoPlayMoveCardTo (deck_cards [x].tableauKCard, deck_cards [x].tableauKCard.transform.localPosition, newPos2));

              UpdateScore (deltaScore);

              // Selected card
              deck_cards [x].tableauIndex = -1;
              deck_cards [x].tableauPeek = false;
              deck_cards [x].foundation = (card_foundation)(deck_cards [x].suit + 10);
              deck_cards [x].wasteActivePos = -1;
              deck_cards [x].tableauFaceUp = true;
              deck_cards [x].cardWasInFoundation = true;

              UpdateFoundationColliders (deck_cards [x].suit);

              found = true;
              break;
            } else {
              // Not in waste_active or in tableau. Game Over.
              break;
            }
          }
        }
      }


      nextSuit++;
      if (nextSuit > 3)
        nextSuit = 0;

      suitCycle++;
    }


    if (!found) {
      StartCoroutine (GameOver (0));
    }
  }

  public IEnumerator AutoPlayMoveCardTo (kcard _kcard, Vector3 start_point, Vector3 end_point, float duration = 0.08F)
  {
    _kcard.transform.SetAsLastSibling ();

    _kcard.transform.localPosition = start_point;

    //animation
    float time = 0;

    bool playedSound = false;
    while (time < 1) {
      time += Time.deltaTime / duration;
      Vector3 newpos = Vector3.Lerp (start_point, end_point, time);
      newpos.z = _kcard.transform.localPosition.z;
      _kcard.transform.localPosition = newpos;

      if (time > 0.5f && !playedSound) {
        //MenuManager.GetInstance().PlaySound(MenuManager.SoundNamesEnum.TAKE_CARD_SOUND);
        playedSound = true;

      }

      yield return null;
    }

    NextAutoPlaySequence ();
  }

  public IEnumerator GameOver (int endMode)
  {
    //endMode => 0 = normal (no more cards on board), 1 = time, 2 = aborted (by buttons).

    int final_bonus_score_val = 0;
    int final_score_val = 0;

    final_score.text = current_score.ToString (scoreFormat);
    final_bonus_score.text = "0";
    final_total_score.text = current_score.ToString (scoreFormat);

    final_bonus_score_val = 4 * ((int)(left_time * GetCardsOnFoundations () / 52));
    final_score_val = current_score + final_bonus_score_val;

    if (GameTaco.TacoSetup.Instance.IsTournamentPlayed ()) {
      GameTaco.TacoSetup.Instance.TacoPostScore (final_score_val);
      yield break;
    }

    //if (!game_end)
    {
      game_end = true;
      player_can_move = false;
      KlondikeMenu.GetInstance ().ClosePopups ();
      KlondikeMenu.GetInstance ().OpenBlackFade ();
      if (endMode == 0) {
        //MenuManager.GetInstance().PlaySound(MenuManager.SoundNamesEnum.WIN_SOUND,false,1);
      }
      yield return new WaitForSeconds (show_win_or_lose_screen_after_delay);


      KlondikeMenu.GetInstance ().OpenPopup (1);
      if (KlondikeMenu.GetInstance ().endDialog_moveAnimator.gameObject.activeSelf)
        popup_animation_duration = KlondikeMenu.GetInstance ().endDialog_moveAnimator.GetCurrentAnimatorClipInfo (0).Length;
      yield return new WaitForSeconds (popup_animation_duration + 0.5f);

      if (final_bonus_score_val > 0) {
        MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.SCORE_TALLY_SOUND, true, 0);
      }

      float time = 0;
      float displayEvery = 1.0F;
      while (time < 1) {
        float delta = Time.smoothDeltaTime / final_score_grow_time;
        time += delta;
        displayEvery += delta;
        if (displayEvery > 0.04F) {
          final_bonus_score.text = Mathf.Lerp (0, final_bonus_score_val, time).ToString (scoreFormat);
          final_total_score.text = Mathf.Lerp (current_score, final_score_val, time).ToString (scoreFormat);
          int ctime = Mathf.CeilToInt (Mathf.Lerp (0, left_time, 1.0f - time));
          int seconds = ctime % 60;
          int minutes = ctime / 60;
          bonus_text.text = string.Format ("{0}:{1:00}", minutes, seconds);

          displayEvery = 0.0F;
        }

        yield return null;
      }

      final_score.text = current_score.ToString (scoreFormat);
      final_bonus_score.text = final_bonus_score_val.ToString (scoreFormat);
      final_total_score.text = final_score_val.ToString (scoreFormat);
      bonus_text.text = "0:00";

      MenuManager.GetInstance ().StopSound (0);

      UpdateScore (final_bonus_score_val);

      yield return new WaitForSeconds (hide_win_or_lose_screen_after_delay);

      MenuManager.GetInstance ().StopAll ();

//#if UNITY_EDITOR
      SceneManager.LoadScene (MenuManager.SceneNames [(int)MenuManager.SceneNamesEnum.MAIN_SCENE]);
/*#else
                SceneManager.LoadScene(MenuManager.SceneNames[(int)MenuManager.SceneNamesEnum.SKILLZ_SCENE]);
                if (SkillzSDK.Api.IsTournamentInProgress)
                {
                    SkillzSDK.Api.FinishTournament(final_score_val);
                }
#endif*/
    }

    yield return null;
  }



  public void CreateScoreEvent (Vector3 position, string text, int score, float initDelay = 0.0f)
  {
    StartCoroutine (MoveGhostScore (position, text, score, initDelay));
  }

  public IEnumerator MoveGhostScore (Vector3 position, string text, int score, float initDelay = 0.0f, float duration = 0.55f)
  {
    yield return new WaitForSeconds (initDelay);

    GameObject gst = (GameObject)Instantiate (Resources.Load ("Prefabs/GhostScore_Dummy"), GameObject.Find ("Canvas").transform);
    //gst.transform.SetParent(GameObject.Find("Canvas").transform);
    //Debug.Log(gst.transform.localPosition);
    //gst.GetComponent<RectTransform>().anchoredPosition.Set(20, 10);
    gst.transform.position = position;
    //gst.transform.localPosition = transform.localPosition;
    gst.transform.localScale = new Vector3 (1.0F, 1.0F, 1.0F);
    gst.transform.SetSiblingIndex (GameObject.Find ("Black_Panel").transform.GetSiblingIndex ());

    Transform gstt = gst.transform.GetChild (0);

    gstt.localScale = new Vector3 (1.3f, 1.3f, 1);
    gstt.gameObject.GetComponent<Text> ().text = text;

    Vector3 start_pos = gst.transform.position;
    //animation
    float time = 0;

    yield return new WaitForSeconds (0.12f);

    while (time < 1) {
      time += Time.deltaTime / duration;
      //gst.transform.position = Vector3.Lerp(start_pos,ghostScoreFinalPos.position,time);
      gst.transform.position = Vector3.Lerp (start_pos, score_text.transform.position, time);
      yield return null;
    }

    yield return new WaitForSeconds (0.2f);

    score_text.text = score.ToString (scoreFormat);
  }

  public void UpdateScore (int add_to_score)
  {
    if (add_to_score != 0) {
      if (undo_list.Count > 0) {
        undo_list [undo_list.Count - 1].undo_previous_score = add_to_score;
      }

      current_score += add_to_score;
      current_score = Mathf.Max (0, current_score);

      if (game_end) {
        score_text.text = current_score.ToString (scoreFormat);
      }
      GameTaco.TacoSetup.Instance.ScoreNow = current_score;
      /*if (SkillzSDK.Api.IsTournamentInProgress)
			{
					SkillzSDK.Api.UpdatePlayerScore(current_score);
			}*/
    }
  }

  public IEnumerator ShakeCard (GameObject go)//show the miss move
  {
    player_can_move = false;
    animationsCount++;

    Transform t = go.transform;
    Vector3 start_position = t.position;
    float min_magnitude = -0.03f;
    float max_magnitude = 0.03f;
    bool right = true;
    int n_shakes = 5;
    float duration = 0.018f;
    float time = 0;

    while (n_shakes > 0) {
      while (time < 1) {

        time += Time.deltaTime / duration;


        if (right) {
          t.position = new Vector3 (Mathf.Lerp (t.position.x, start_position.x + max_magnitude, time), t.position.y, t.position.z);
        } else {
          t.position = new Vector3 (Mathf.Lerp (start_position.x + min_magnitude, t.position.x, 1.0F - time), t.position.y, t.position.z);
        }


        yield return null;
      }

      right = !right;
      time = 0;
      n_shakes--;

      yield return null;
    }

    t.position = start_position;

    animationsCount--;
    CheckPlayerCanMove ();

  }

  public void WrongCard (GameObject go, bool playSound)
  {
    if (playSound) {
      MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.ERROR_SOUND);
    }

    StartCoroutine (ShakeCard (go));
  }
}
