using UnityEngine.UI;
using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(deck))]
internal class deck_editor : Editor {

    public override void OnInspectorGUI()
    {
        Rules ();
        Score ();
        Sprites ();
        Sfx ();
        Advanced();

    }

    void Rules()
    {
        deck my_target = (deck)target;
        EditorGUI.BeginChangeCheck ();
        Undo.RecordObject(my_target, "edit_rules");

        EditorGUILayout.LabelField("Deck");
        //my_target.give_an_useful_card_not_less_than_n_new_card_from_the_deck = EditorGUILayout.IntField("Max number of useless card to discover before find a good card", my_target.give_an_useful_card_not_less_than_n_new_card_from_the_deck);
		my_target.maxUndoSteps = EditorGUILayout.IntField("Max undo steps", my_target.maxUndoSteps);

        EditorGUI.indentLevel--;

        if (EditorGUI.EndChangeCheck ())
            EditorUtility.SetDirty(my_target);

        EditorGUILayout.Space();
    }

    void Score()
    {
        deck my_target = (deck)target;
        EditorGUI.BeginChangeCheck ();

        EditorGUILayout.LabelField("Score");
        EditorGUI.indentLevel++;
        my_target.ghostScoreFinalPos = EditorGUILayout.ObjectField("Ghost Score Final Pos", my_target.ghostScoreFinalPos , typeof(Transform), true) as Transform;
        my_target.normal_card_score = EditorGUILayout.IntField("Normal card", my_target.normal_card_score);
  
        my_target.expose_card_score = EditorGUILayout.IntField("Expose card", my_target.expose_card_score);  
        my_target.undo_score = EditorGUILayout.IntField("Undo", my_target.undo_score);    

        my_target.bonus_time = EditorGUILayout.FloatField("Bonus time (secs)", my_target.bonus_time);  
        EditorGUI.indentLevel--;

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Combo");
        EditorGUI.indentLevel++;

        my_target.combo_enabled =  EditorGUILayout.Toggle("Combos enabled", my_target.combo_enabled);  

        for (int i = 0; i < 7; i++)
        {
            my_target.combo_texts[i] = EditorGUILayout.TextField("Combo Text " + (i+1).ToString(), my_target.combo_texts[i]);
            my_target.combo_scores[i] = EditorGUILayout.IntField("Combo Score " + (i+1).ToString(), my_target.combo_scores[i]);
            my_target.combo_triggers[i] = EditorGUILayout.IntField("Combo Trigger " + (i+1).ToString(), my_target.combo_triggers[i]);
        }
            
        EditorGUI.indentLevel--;

        if (EditorGUI.EndChangeCheck ())
            EditorUtility.SetDirty(my_target);

        EditorGUILayout.Space();
    }


    void Sprites()
    {
        deck my_target = (deck)target;
        EditorGUI.BeginChangeCheck ();
        Undo.RecordObject(my_target, "edit_sprite");

        my_target.editor_show_sprites = EditorGUILayout.Foldout(my_target.editor_show_sprites, "Sprites");
        if (my_target.editor_show_sprites)
        {

            EditorGUI.indentLevel++;
            my_target.editor_show_sprites_normal = EditorGUILayout.Foldout(my_target.editor_show_sprites_normal, "normal");
            if (my_target.editor_show_sprites_normal)
            {
                EditorGUI.indentLevel++;
                if (!my_target.card_back_sprite)
                    GUI.color = Color.red;
                else
                    GUI.color = Color.white;
                my_target.card_back_sprite = EditorGUILayout.ObjectField("back",my_target.card_back_sprite, typeof(Sprite), false) as Sprite;
                GUI.color = Color.white;

                my_target.editor_show_sprites_normal_clubs = EditorGUILayout.Foldout(my_target.editor_show_sprites_normal_clubs, "clubs");
                if (my_target.editor_show_sprites_normal_clubs)
                {
                    EditorGUI.indentLevel++;
                    for (int i = 0; i < 13; i++)
                    {
                        if (!my_target.clubs[i])
                            GUI.color = Color.red;
                        else
                            GUI.color = Color.white;

                        my_target.clubs[i] = EditorGUILayout.ObjectField(Sprite_name(i),my_target.clubs[i], typeof(Sprite), false) as Sprite;
                        GUI.color = Color.white;
                    }
                    EditorGUI.indentLevel--;
                }

                my_target.editor_show_sprites_normal_spades = EditorGUILayout.Foldout(my_target.editor_show_sprites_normal_spades, "spades");
                if (my_target.editor_show_sprites_normal_spades)
                {
                    EditorGUI.indentLevel++;
                    for (int i = 0; i < 13; i++)
                    {
                        if (!my_target.spades[i])
                            GUI.color = Color.red;
                        else
                            GUI.color = Color.white;

                        my_target.spades[i] = EditorGUILayout.ObjectField(Sprite_name(i),my_target.spades[i], typeof(Sprite), false) as Sprite;
                        GUI.color = Color.white;
                    }
                    EditorGUI.indentLevel--;
                }

                my_target.editor_show_sprites_normal_diamonds = EditorGUILayout.Foldout(my_target.editor_show_sprites_normal_diamonds, "diamonds");
                if (my_target.editor_show_sprites_normal_diamonds)
                {
                    EditorGUI.indentLevel++;
                    for (int i = 0; i < 13; i++)
                    {
                        if (!my_target.diamonds[i])
                            GUI.color = Color.red;
                        else
                            GUI.color = Color.white;

                        my_target.diamonds[i] = EditorGUILayout.ObjectField(Sprite_name(i),my_target.diamonds[i], typeof(Sprite), false) as Sprite;
                        GUI.color = Color.white;
                    }
                    EditorGUI.indentLevel--;
                }

                my_target.editor_show_sprites_normal_hearts = EditorGUILayout.Foldout(my_target.editor_show_sprites_normal_hearts, "hearts");
                if (my_target.editor_show_sprites_normal_hearts)
                {
                    EditorGUI.indentLevel++;
                    for (int i = 0; i < 13; i++)
                    {
                        if (!my_target.hearts[i])
                            GUI.color = Color.red;
                        else
                            GUI.color = Color.white;

                        my_target.hearts[i] = EditorGUILayout.ObjectField(Sprite_name(i),my_target.hearts[i], typeof(Sprite), false) as Sprite;
                        GUI.color = Color.white;
                    }
                    EditorGUI.indentLevel--;
                }

                EditorGUI.indentLevel--;
            }
				
            EditorGUI.indentLevel--;
        }

        if (EditorGUI.EndChangeCheck ())
            EditorUtility.SetDirty(my_target);

        EditorGUILayout.Space();
    }

    void Sfx()
    {
        deck my_target = (deck)target;
        EditorGUI.BeginChangeCheck ();
        Undo.RecordObject(my_target, "edit_sfx");


        if (EditorGUI.EndChangeCheck ())
            EditorUtility.SetDirty(my_target);

        EditorGUILayout.Space();
    }

    void Advanced()
    {
        deck my_target = (deck)target;
        EditorGUI.BeginChangeCheck ();
        Undo.RecordObject(my_target, "edit_advanced");

        my_target.editor_show_advanced = EditorGUILayout.Foldout(my_target.editor_show_advanced, "Advanced");
        if (my_target.editor_show_advanced)
        {
            EditorGUI.indentLevel++;
            my_target.editor_show_gui = EditorGUILayout.Foldout(my_target.editor_show_gui, "GUI");
            if (my_target.editor_show_gui)
            {
                EditorGUI.indentLevel++;
                my_target.show_win_or_lose_screen_after_delay = EditorGUILayout.FloatField("show gameOver screen after",my_target.show_win_or_lose_screen_after_delay);
                my_target.hide_win_or_lose_screen_after_delay = EditorGUILayout.FloatField("exit gameOver screen after",my_target.hide_win_or_lose_screen_after_delay);

                EditorGUI.indentLevel++;
                	my_target.score_text = EditorGUILayout.ObjectField("score text", my_target.score_text, typeof(Text), true) as Text;
                	my_target.bonus_text = EditorGUILayout.ObjectField("bonus text", my_target.bonus_text, typeof(Text), true) as Text;
                	my_target.combo_text = EditorGUILayout.ObjectField("combo text", my_target.combo_text, typeof(Text), true) as Text;
                	my_target.deck_card_left_text = EditorGUILayout.ObjectField("deck card left text", my_target.deck_card_left_text, typeof(Text), true) as Text;
                	my_target.undo_button = EditorGUILayout.ObjectField("undo button", my_target.undo_button, typeof(Button), true) as Button;
                	my_target.gpend_button = EditorGUILayout.ObjectField("gpend button", my_target.gpend_button, typeof(Button), true) as Button;
                EditorGUI.indentLevel--;


                EditorGUI.indentLevel++;
                	my_target.final_score = EditorGUILayout.ObjectField("Final Score", my_target.final_score, typeof(Text), true) as Text;
                	my_target.final_bonus_score = EditorGUILayout.ObjectField("Final Bonus Score", my_target.final_bonus_score, typeof(Text), true) as Text;
                	my_target.final_total_score = EditorGUILayout.ObjectField("Final Total Score", my_target.final_total_score, typeof(Text), true) as Text;
                EditorGUI.indentLevel--;

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();
            my_target.ghost_card =  EditorGUILayout.ObjectField("ghost card", my_target.ghost_card , typeof(Transform), true) as Transform;
            my_target.board =  EditorGUILayout.ObjectField("board", my_target.board , typeof(Transform), true) as Transform;
            my_target.target_new_card =  EditorGUILayout.ObjectField("target_new_card", my_target.target_new_card , typeof(Image), true) as Image;
            my_target.new_card_anim_sprite =  EditorGUILayout.ObjectField("new_card_anim_sprite", my_target.new_card_anim_sprite , typeof(Image), true) as Image;
            my_target.new_card_anim =  EditorGUILayout.ObjectField("new_card_anim", my_target.new_card_anim , typeof(Animation), true) as Animation;


            EditorGUI.indentLevel--;
        }

        if (EditorGUI.EndChangeCheck ())
            EditorUtility.SetDirty(my_target);

    }

    string Sprite_name(int card_value)
    {
        string return_this = "";

        if (card_value == 0)
            return_this = "A";
        else if (card_value == 10)
            return_this = "J";
        else if (card_value == 11)
            return_this = "Q";
        else if (card_value == 12)
            return_this = "K";
        else
            return_this = (card_value + 1).ToString();

        return return_this;
    }
}
