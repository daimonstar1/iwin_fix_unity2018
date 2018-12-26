using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class deck : MonoBehaviour
{
  //Rules
  public bool show_unused_card_when_player_take_a_new_card_from_the_deck;
//if true, when player ask a new card and miss to use the current, the miss move shake
  public int give_an_useful_card_not_less_than_n_new_card_from_the_deck;
//because deck card are random, in order to avoid to have a long sequence of useless cards by accident, you can set here the max lenght of the useless sequence. After that the deck will give an useful for sure. If you don't want give this help to the player, just put here a number greater than total card
  int unused_card_count;
//the remaining deck card when the player win. It is used to give the bonus score "score_for_each_deck_card_spared"

  //Combo
  int combo_count;
  int previous_combo_count;
//for undo
  public Text combo_text;

  //Score
  public int current_score;
  public Text score_text;
  public int normal_card_score;
  public int undo_score;
  public int expose_card_score;
  public float bonus_time;
  public float left_time;
  public int int_time;
  public bool timer_paused = false;
  public Button gpend_button;
  public string scoreFormat = "N0";

  public Transform ghostScoreFinalPos;

  public bool game_started;
//game autosetup itself, then set game_started = true and player_can_move = true so player can play
  public bool game_end;
//true no more moves or win condition reached
  [HideInInspector]
  public bool player_can_move;
//it is false before game start, after game end and through card animations

  static deck instance = null;
//this script
  float animation_duration = 0.25f;
  float undo_animation_duration = 0.25f;
  float popup_animation_duration = 0.25f;

  public int deck_card_left;
//how many card the player can ask
  public Text deck_card_left_text;
  int current_deck_position;
//the current position in the deck array
  int current_target_deck_position;
//the current position in the target deck array

  //the card in the target deck
  public int current_card_suit = -99;
  public int current_card_rank = -99;
  //int previous_suit = -99;
  int previus_rank = -99;

  [System.Serializable]
  public class card_class
  {
    public int suit = -99;
    public int rank = -99;
  }

  public card_class[] deck_cards;
//store the cards in the deck
  public card_class[] target_deck_cards;
//keep trak of the card put in the target deck from deck and board

  public Image target_new_card;
//the new current card to show
  public Image new_card_anim_sprite;
//the card to show in the animantion from deck to target deck
  public Animation new_card_anim;
//animation from deck to target deck

  public Sprite card_back_sprite;

  //black
  public Sprite[] clubs = new Sprite[13];
  public Sprite[] spades = new Sprite[13];

  //red
  public Sprite[] diamonds = new Sprite[13];
  public Sprite[] hearts = new Sprite[13];

  public bool combo_enabled;
  public int[] combo_scores = new int[7];
  public int[] combo_triggers = new int[7];
  public string[] combo_texts = new string[7];

  public Sprite[,] card_deck_sprite;
//suit,rank

  public Transform board;
//the child of this trasnform are the card on board
  public int current_total_card_on_board;
  int total_card_on_board;

  //these variables aim to avoid to have too many cards with the same rank
  public int max_number_of_card_with_the_same_rank_on_board;
  public int[] card_on_board_by_rank;

  card[] cards_on_board;

  // Undo
  public Button undo_button;
  public int maxUndoSteps;

  public enum moveType
  {
    ask_new_card_from_deck = 0,
    take_card_from_board
  }

  public class UndoInfo
  {
    public GameObject card;
    public moveType move = moveType.ask_new_card_from_deck;
    public int undo_previous_score = 0;
  }

  public List<UndoInfo> undo_list = new List<UndoInfo> ();

  public Transform ghost_card;
//it is use to show an animation when a card pass from board to target deck or conversely

  //GUI screen
  public Text final_score;
  public Text final_total_score;
  public Text final_bonus_score;
  public Text bonus_text;
  public float show_win_or_lose_screen_after_delay;
  public float hide_win_or_lose_screen_after_delay;
  private float final_score_grow_time = 1.5F;

  // Sfx

  // Editor (theso bool open and close the menus of this script in inspector
  public bool editor_show_sprites;
  public bool editor_show_sprites_normal;
  public bool editor_show_sprites_normal_clubs;
  public bool editor_show_sprites_normal_spades;
  public bool editor_show_sprites_normal_diamonds;
  public bool editor_show_sprites_normal_hearts;
  public bool editor_show_advanced;
  public bool editor_show_gui;
  private bool mustEnableEndButton = false;
  private bool mustEnableDeckImage = false;

  int animationsCount = 0;

  public static deck GetInstance ()
  {
    return instance;
  }

  void Awake ()//varaibles to initiate only one time for scene
  {

    instance = this;//this script
    left_time = bonus_time;
    int_time = -1;

    GenerateCardDeckSprite ();//fuse clubs[], spades[], diamonds[] and hearts[] in one array

    //setup deck sprite
    GetComponent<Image> ().sprite = card_back_sprite;
    new_card_anim.transform.GetChild (0).transform.GetChild (0).GetComponent<Image> ().sprite = card_back_sprite;

    animation_duration = new_card_anim.GetComponent<Animation> ().GetClip ("new_card_anim").length;
    undo_animation_duration = new_card_anim.GetComponent<Animation> ().GetClip ("undo_to_deck_card_anim").length;
    if (PyramidMenu.GetInstance ().endDialog_moveAnimator.gameObject.activeSelf)
      popup_animation_duration = PyramidMenu.GetInstance ().endDialog_moveAnimator.GetCurrentAnimatorClipInfo (0).Length;

    gpend_button.interactable = false;
    mustEnableEndButton = false;
    mustEnableDeckImage = false;
    total_card_on_board = 0;

    //Debug.Log ("board cards = " + total_card_on_board + " ...max same rank = " + max_number_of_card_with_the_same_rank_on_board);
  }

  void Start ()
  {
    Restart ();
  }

  public void EnableCards (bool val)
  {
    for (int i = 0; i < cards_on_board.Length; i++) {
      cards_on_board [i].gameObject.GetComponent<BoxCollider2D> ().enabled = val;
    }

    gameObject.GetComponent<BoxCollider2D> ().enabled = val;
  }

  public void Restart ()
  {
    //reset variables
    game_started = false;
    game_end = false;
    player_can_move = false;
    current_total_card_on_board = 0;
    current_deck_position = 0;
    current_target_deck_position = 0;
    unused_card_count = 0;
    animationsCount = 0;

    //combo
    previous_combo_count = 0;
    combo_count = 0;

    //score
    current_score = 0;
    score_text.text = current_score.ToString ();

    this.gameObject.GetComponent<Image> ().enabled = true;
    target_new_card.gameObject.SetActive (false);
    //game_screen.SetActive(true);
    UpdateUndoButton ();

    card_on_board_by_rank = new int[13];
    left_time = bonus_time;
    int_time = -1;
    timer_paused = true;
    gpend_button.interactable = false;
    mustEnableEndButton = false;
    mustEnableDeckImage = false;

    PrintTime ();

    FillDeck ();//crate the contents of the deck

    int val = PlayerData.GetInstance ().GetValueAsInt ("pyramidHTP");
    if (val == 0) {
      PyramidMenu.GetInstance ().OpenPopup (2);
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


    StartCoroutine (NewCurrentCard (deck_cards [current_deck_position].suit, deck_cards [current_deck_position].rank, true, false));//show the first card of the deck

    animationsCount--;
    timer_paused = false;
    CheckPlayerCanMove ();
  }

  void FillDeck ()//decide what card put in the deck, but some can change if give_an_useful_card_not_less_than_n_new_card_from_the_deck is trigger
  {
    //for (int i = board.childCount-1 ; i >= 0 ; i--)
    for (int i = 0; i < board.childCount; i++) {
      //Levels
      Transform levelObj = board.GetChild (i).transform;

      for (int j = 0; j < levelObj.childCount; j++) {
        // Cards
        total_card_on_board++;
      }
    }

    deck_card_left = 52 - total_card_on_board;

    deck_cards = new card_class[deck_card_left];
    cards_on_board = new card[total_card_on_board];

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

      deck_cards [i].suit = s;
      deck_cards [i].rank = r;
    }

    // Fill cards on board
    int w = 0;
    //for (int i = board.childCount-1 ; i >= 0 ; i--)
    for (int i = 0; i < board.childCount; i++) {
      //Levels
      Transform levelObj = board.GetChild (i).transform;

      for (int j = 0; j < levelObj.childCount; j++) {
        // Cards


        cards_on_board [w] = levelObj.GetChild (j).GetComponent<card> ();//put all board card script in an array to quick reference
        cards_on_board [w].level = i;
        //Debug.Log(i);
        w++;
      }
    }

    for (int i = 0; i < total_card_on_board; i++) {
      int sindex = Util.GetRandomBetween0And (suits.Count - 1);
      int s = suits [sindex];

      int rindex = Util.GetRandomBetween0And (ranks [s].Count - 1);
      int r = ranks [s] [rindex];

      ranks [s].RemoveAt (rindex);
      if (ranks [s].Count == 0) {
        suits.RemoveAt (sindex);
      }
      cards_on_board [i].Init (s, r);
    }

    current_total_card_on_board = total_card_on_board;

    target_deck_cards = new card_class[deck_card_left + current_total_card_on_board + 1];
    for (int i = 0; i < target_deck_cards.Length; i++) {
      target_deck_cards [i] = new card_class ();
    }

    max_number_of_card_with_the_same_rank_on_board = Mathf.CeilToInt (total_card_on_board * 0.25f);//decide the max number of card with the same rank to put on board

    deck_card_left_text.text = (deck_card_left - 1).ToString ();
  }

  void GenerateCardDeckSprite ()//fuse the card sprite arrays in one
  {
    card_deck_sprite = new Sprite[4, 13];
    //gold_card_deck_sprite = new Sprite[4,13];

    Sprite[] temp_rank = new Sprite[13];
    //Sprite[] temp_rank_gold = new Sprite[13];

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

  public void AddUndoStep (GameObject card, moveType move, int ups)
  {
    if (game_started) {
      if (undo_list.Count >= maxUndoSteps) {
        undo_list.RemoveAt (0);
      }

      UndoInfo undoInfo = new UndoInfo ();
      undoInfo.card = card;
      undoInfo.move = move;
      undoInfo.undo_previous_score = ups;
      undo_list.Add (undoInfo);
    }
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


  void OnMouseDown () //Deck
  {
    if (player_can_move && Input.touches.Length <= 1) {
      if (deck_card_left > 1) {//if there is a card to give
        //update deck count
        deck_card_left--;
        deck_card_left_text.text = (deck_card_left - 1).ToString ();
        current_deck_position++;

        StartCoroutine (NewCurrentCard (deck_cards [current_deck_position].suit, deck_cards [current_deck_position].rank, true, false));//show animation

        if (deck_card_left == 1) {//if this was the last card
          mustEnableDeckImage = false;
          this.gameObject.GetComponent<Image> ().enabled = false;
          StartCoroutine (EnableGPEndButton ());
          //Debug.Log ("No more cards");
        }
      } else {// no more card, the game end
        //no card
        current_card_suit = -99;
        current_card_rank = -99;
      }
    }
  }

  public void CheckPlayerCanMove ()
  {
    if (animationsCount == 0 && !game_end && !timer_paused) {
      player_can_move = true;
    }
  }

  // NOT TESTED WITH COMBO SCORES!!
  public void UndoLastMove ()
  {
    if (undo_list.Count > 0 && player_can_move) {//if player can ask an undo
      current_target_deck_position--;//remove last card from target deck

      UndoInfo undoInfo = undo_list [undo_list.Count - 1];

      if (undoInfo.move == moveType.ask_new_card_from_deck) {//return last card to deck
        deck_card_left++;
        deck_card_left_text.text = (deck_card_left - 1).ToString ();
        current_deck_position--;

        StartCoroutine (NewCurrentCard (target_deck_cards [current_target_deck_position].suit, target_deck_cards [current_target_deck_position].rank, true, true));


        if (!this.gameObject.GetComponent<Image> ().enabled) {
          StartCoroutine (EnableDeckImage ());
        }

        mustEnableEndButton = false;
        gpend_button.interactable = false;
      } else if (undoInfo.move == moveType.take_card_from_board) { //return last card to board
        //Debug.Log("last_move.take_card_from_board");
        current_total_card_on_board++;

        StartCoroutine (MoveTo (undoInfo.card, target_new_card.transform, undoInfo.card.transform, true));

        StartCoroutine (NewCurrentCard (target_deck_cards [current_target_deck_position].suit, target_deck_cards [current_target_deck_position].rank, false, true));

      }

      combo_count = previous_combo_count;
    }
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
  }

  public void UpdateUndoButton ()
  {
    undo_button.gameObject.SetActive (undo_list.Count > 0);
  }

  public IEnumerator EnableGPEndButton ()
  {
    mustEnableEndButton = true;
    yield return new WaitForSeconds (0.4f);
    if (mustEnableEndButton) {
      gpend_button.interactable = true;
      mustEnableEndButton = false;

      int val = PlayerData.GetInstance ().GetValueAsInt ("pyramidTips0");
      if (val == 0) {
        PyramidMenu.GetInstance ().OpenPopup (4);
      }
    }
  }

  public IEnumerator ComboScore (int combo_score)
  {
    yield return new WaitForSeconds (0.4f);
    deck.GetInstance ().CreateScoreEvent (PyramidMenu.GetInstance ().combo_score_event.transform.position, combo_score.ToString (deck.GetInstance ().scoreFormat), current_score + combo_score, 0.2f);
  }

  public IEnumerator EnableDeckImage ()
  {
    mustEnableDeckImage = true;
    yield return new WaitForSeconds (undo_animation_duration);
    if (mustEnableDeckImage) {
      this.gameObject.GetComponent<Image> ().enabled = true;
      mustEnableDeckImage = false;
    }
  }

  public IEnumerator MoveTo (GameObject go, Transform start_point, Transform end_point, bool refresh_board)//move the ghost card
  {

    player_can_move = false;
    animationsCount++;

    ghost_card.position = start_point.position;
    ghost_card.rotation = start_point.rotation;
    ghost_card.gameObject.SetActive (true);

    //animation
    float duration = animation_duration * 1.3F;
    float time = 0;
    float currentAngle;
    if (start_point.rotation.z < 0) {
      currentAngle = 360.0F - start_point.rotation.eulerAngles.z;
    } else {
      currentAngle = (360.0F - start_point.rotation.eulerAngles.z) - 360.0F;
    }

    if (currentAngle == 360.0F) {
      currentAngle = 0;
    }

    float angle;

    if (currentAngle < 360.0F) {

      angle = 360.0f - currentAngle; // Degree per time unit
    } else {
      angle = -currentAngle;
    }


    Vector3 axis = Vector3.back; // Rotation axis, here it the yaw axis

    while (time < 1) {
      float deltaT = Time.smoothDeltaTime / duration;

      if (time + deltaT > 1) {
        deltaT = 1.0F - time;
        time = 1;
      } else {
        time += deltaT;
      }

      ghost_card.position = Vector3.Lerp (start_point.position, end_point.position, time);
      ghost_card.transform.RotateAround (ghost_card.transform.position, axis, angle * deltaT);
      yield return null;
    }


    if (refresh_board) {
      StartCoroutine (RefreshCardRotation ());
    }

    ghost_card.gameObject.SetActive (false);

    animationsCount--;
    CheckPlayerCanMove ();
  }

  public IEnumerator NewCurrentCard (int suit, int rank, bool from_deck, bool undo)//show the new current card in target deck
  {

    player_can_move = false;
    animationsCount++;

    previus_rank = current_card_rank;

    current_card_suit = suit;
    current_card_rank = rank;


    if (from_deck) {//the new card is give from the deck
      MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.NEW_CARD_SOUND);

      if (undo) {
        target_new_card.sprite = card_deck_sprite [current_card_suit, current_card_rank];

        UpdateScore (undo_score);
        CreateScoreEvent (undo_button.transform.position, undo_score.ToString (scoreFormat), current_score);

        new_card_anim.Play ("undo_to_deck_card_anim");
        yield return new WaitForSeconds (undo_animation_duration);

        RemoveUndoStep ();
        UpdateUndoButton ();
        unused_card_count--;

        //Update_combo_text();
        new_card_anim_sprite.sprite = card_deck_sprite [current_card_suit, current_card_rank];
      } else {
        current_target_deck_position++;

        if (game_started) {
          unused_card_count++;
          if (show_unused_card_when_player_take_a_new_card_from_the_deck) {
            int avaible_move = CheckAvaibleMoves (previus_rank);
            if (avaible_move >= 0) { //if there is a move
            }

          }
        }

        target_deck_cards [current_target_deck_position] = new card_class ();
        target_deck_cards [current_target_deck_position].suit = current_card_suit;
        target_deck_cards [current_target_deck_position].rank = current_card_rank;


        new_card_anim_sprite.sprite = card_deck_sprite [current_card_suit, current_card_rank];

        new_card_anim.Play ("new_card_anim");
        yield return new WaitForSeconds (animation_duration);

        previous_combo_count = combo_count;
        combo_count = 0;
        
        AddUndoStep (null, moveType.ask_new_card_from_deck, current_score);
        UpdateUndoButton ();

        target_new_card.sprite = card_deck_sprite [current_card_suit, current_card_rank];
      }

      if (!game_started) {
        game_started = true;
        target_new_card.gameObject.SetActive (true);
      }
    } else { //from board
      MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.TAKE_CARD_SOUND);

      if (undo) {
        target_new_card.sprite = card_deck_sprite [current_card_suit, current_card_rank];

        int deltaScore = -undo_list [undo_list.Count - 1].undo_previous_score + undo_score;
        UpdateScore (deltaScore);
        CreateScoreEvent (undo_button.transform.transform.position, deltaScore.ToString (scoreFormat), current_score);

        yield return new WaitForSeconds (animation_duration);//wait the end of the animation

        undo_list [undo_list.Count - 1].card.SetActive (true);

        RemoveUndoStep ();
        UpdateUndoButton ();

        RefreshCardRotation ();
      } else { //card take from board and this not is an undo move
        yield return new WaitForSeconds (animation_duration);//wait the end of the animation
        current_target_deck_position++;

        target_deck_cards [current_target_deck_position] = new card_class ();
        target_deck_cards [current_target_deck_position].suit = current_card_suit;
        target_deck_cards [current_target_deck_position].rank = current_card_rank;

        target_new_card.sprite = card_deck_sprite [current_card_suit, current_card_rank];

        unused_card_count = 0;

        current_total_card_on_board--;




        if (current_total_card_on_board == 0) {
          StartCoroutine (GameOver (0));

        }
      }

    }

    animationsCount--;
    CheckPlayerCanMove ();
  }

  public int AddCombo ()
  {
    previous_combo_count = combo_count;
    combo_count++;

    for (int i = 6; i >= 0; i--) {
      if (combo_count == combo_triggers [i]) {
        combo_text.text = combo_texts [i];
        PyramidMenu.GetInstance ().OpenPopup (3);
        return combo_scores [i];
      }
    }

    return 0;
  }

  public void CreateScoreEvent (Vector3 position, string text, int score, float initDelay = 0.0f)
  {
    StartCoroutine (MoveGhostScore (position, text, score, initDelay));
  }

  public IEnumerator MoveGhostScore (Vector3 position, string text, int score, float initDelay = 0.0f, float duration = 0.55f)
  {
    yield return new WaitForSeconds (initDelay);

    GameObject gst = (GameObject)Instantiate (Resources.Load ("Prefabs/GhostScore_Dummy"), GameObject.Find ("Canvas").transform);
    gst.transform.position = position;
    gst.transform.localScale = new Vector3 (1.0F, 1.0F, 1.0F);
    gst.transform.SetSiblingIndex (GameObject.Find ("ComboInfo_Image").transform.GetSiblingIndex ());

    Transform gstt = gst.transform.GetChild (0);

    gstt.localScale = new Vector3 (1.3f, 1.3f, 1);
    gstt.gameObject.GetComponent<Text> ().text = text;

    Vector3 start_pos = gst.transform.position;
    //animation
    float time = 0;

    yield return new WaitForSeconds (0.12f);

    while (time < 1) {
      time += Time.deltaTime / duration;
      gst.transform.position = Vector3.Lerp (start_pos, score_text.transform.position, time);
      yield return null;
    }

    yield return new WaitForSeconds (0.2f);

    score_text.text = score.ToString (scoreFormat);
  }

  int CheckAvaibleMoves (int target_rank)
  {
    int return_this = -1;
    for (int i = 0; i < cards_on_board.Length; i++) {
      if (cards_on_board [i].ThisCardIsFree () && cards_on_board [i].gameObject.activeSelf == true && cards_on_board [i].face_up) { //if this card is active

        if ((cards_on_board [i].my_rank > 0) &&
        (cards_on_board [i].my_rank < 12)) {
          if ((target_rank + 1 == cards_on_board [i].my_rank)
          || (target_rank - 1 == cards_on_board [i].my_rank)) {
            return_this = i;
            break;
          }
        } else if (cards_on_board [i].my_rank == 12) {
          if ((target_rank == 0)
          || (target_rank + 1 == cards_on_board [i].my_rank)) {
            return_this = i;
            break;
          }
        } else if (cards_on_board [i].my_rank == 0) {
          if ((target_rank - 1 == cards_on_board [i].my_rank)
          || (target_rank == 12)) {
            return_this = i;
            break;
          }
        }

      }

    }

    return return_this;

  }

  public IEnumerator ShakeCard (card this_card)//show the miss move
  {
    if (!this_card.shaking) {
      this_card.shaking = true;
      Transform t = this_card.gameObject.transform;
      Vector3 start_position = t.position;
      float min_magnitude = -0.03f;
      float max_magnitude = 0.03f;
      bool right = true;
      int n_shakes = 5;
      float duration = 0.018f;
      float time = 0;

      while (n_shakes > 0) {
        while (time < 1) {

          time += Time.smoothDeltaTime / duration;

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
      this_card.shaking = false;
    }
  }


  public IEnumerator RefreshCardRotation ()//turn face up all free cards on board
  {
    yield return new WaitForSeconds (0.05f); // Fail if == 0.0F

    for (int i = 0; i < cards_on_board.Length; i++) {
      if (cards_on_board [i].gameObject.activeSelf == true) {
        //if this card is active
        cards_on_board [i].TurnThisCard (); //check if it can be turn up
      }
    }

    //yield return null;
  }

  public IEnumerator RefreshCardRotationAndAddScore (GameObject go, int baseScore)
  {
    yield return new WaitForSeconds (0.05f); // Fail if == 0.0F

    int totalScore = baseScore;
    int flippledCards = 0;
    for (int i = 0; i < cards_on_board.Length; i++) {
      if (cards_on_board [i].gameObject.activeSelf == true) {
        //if this card is active
        if (cards_on_board [i].TurnThisCard ()) {
          totalScore += this.expose_card_score;
          flippledCards++;
          CreateScoreEvent (cards_on_board [i].transform.position, deck.GetInstance ().expose_card_score.ToString (deck.GetInstance ().scoreFormat), deck.GetInstance ().current_score + totalScore, 0.1f * flippledCards);

        }//check if it can be turn up
      }
    }

    UpdateScore (totalScore);

    deck.GetInstance ().AddUndoStep (go, deck.moveType.take_card_from_board, totalScore);
    deck.GetInstance ().UpdateUndoButton ();

  }

  public void UpdateScore (int add_to_score)
  {
    current_score += add_to_score;
    current_score = Mathf.Max (0, current_score);

    if (game_end) {
      score_text.text = current_score.ToString (scoreFormat);
    }

    GameTaco.TacoSetup.Instance.ScoreNow = current_score;

  }

  public IEnumerator GameOver (int endMode)
  {
    int final_bonus_score_val = 0;
    int final_score_val = 0;

    final_score.text = current_score.ToString (scoreFormat);
    final_bonus_score.text = "0";
    final_total_score.text = current_score.ToString (scoreFormat);

    final_bonus_score_val = 4 * ((int)(left_time * (cards_on_board.Length - current_total_card_on_board) / cards_on_board.Length));
    final_score_val = current_score + final_bonus_score_val;

    if (GameTaco.TacoSetup.Instance.IsTournamentPlayed ()) {
      GameTaco.TacoSetup.Instance.TacoPostScore (final_score_val);
      yield break;
    }

    if (!game_end) {
      game_end = true;
      player_can_move = false;
      PyramidMenu.GetInstance ().ClosePopups ();
      PyramidMenu.GetInstance ().OpenBlackFade ();

      if (endMode == 0) {
        MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.WIN_SOUND, false, 1);
      }
      yield return new WaitForSeconds (show_win_or_lose_screen_after_delay);

      PyramidMenu.GetInstance ().OpenPopup (1);
      if (PyramidMenu.GetInstance ().endDialog_moveAnimator.gameObject.activeSelf)
        popup_animation_duration = PyramidMenu.GetInstance ().endDialog_moveAnimator.GetCurrentAnimatorClipInfo (0).Length;
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

      SceneManager.LoadScene (MenuManager.SceneNames [(int)MenuManager.SceneNamesEnum.MAIN_SCENE]);
    }
  }
}
