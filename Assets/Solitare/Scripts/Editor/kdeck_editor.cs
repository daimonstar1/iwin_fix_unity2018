using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(kdeck))]
public class kdeck_editor : Editor {

    public override void OnInspectorGUI()
    {
        Sprites();
        Advanced();
    }

    void Sprites()
    {
        kdeck my_target = (kdeck)target;
        EditorGUI.BeginChangeCheck ();
        Undo.RecordObject(my_target, "edit_sprite");

        my_target.editor_show_sprites = EditorGUILayout.Foldout(my_target.editor_show_sprites, "Sprites");
        if (my_target.editor_show_sprites)
        {
            EditorGUI.indentLevel++;
            if (!my_target.card_back_sprite)
                GUI.color = Color.red;
            else
                GUI.color = Color.white;
            my_target.card_back_sprite = EditorGUILayout.ObjectField("back", my_target.card_back_sprite, typeof(Sprite), false) as Sprite;
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

                    my_target.clubs[i] = EditorGUILayout.ObjectField(Sprite_name(i), my_target.clubs[i], typeof(Sprite), false) as Sprite;
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

                    my_target.spades[i] = EditorGUILayout.ObjectField(Sprite_name(i), my_target.spades[i], typeof(Sprite), false) as Sprite;
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

                    my_target.diamonds[i] = EditorGUILayout.ObjectField(Sprite_name(i), my_target.diamonds[i], typeof(Sprite), false) as Sprite;
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

                    my_target.hearts[i] = EditorGUILayout.ObjectField(Sprite_name(i), my_target.hearts[i], typeof(Sprite), false) as Sprite;
                    GUI.color = Color.white;
                }
                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel--;
        }

        if (EditorGUI.EndChangeCheck ())
            EditorUtility.SetDirty(my_target);

        EditorGUILayout.Space();
    }

    void Advanced()
    {
        kdeck my_target = (kdeck)target;
        EditorGUI.BeginChangeCheck ();
        Undo.RecordObject(my_target, "edit_advanced");


            EditorGUI.indentLevel++;

        my_target.ghostScoreFinalPos = EditorGUILayout.ObjectField("Ghost Score Final Pos", my_target.ghostScoreFinalPos , typeof(Transform), true) as Transform;

        my_target.waste_card0 =  EditorGUILayout.ObjectField("waste_card0", my_target.waste_card0 , typeof(Image), true) as Image;
        my_target.waste_card1 =  EditorGUILayout.ObjectField("waste_card1", my_target.waste_card1 , typeof(Image), true) as Image;
        my_target.waste_card2 =  EditorGUILayout.ObjectField("waste_card2", my_target.waste_card2 , typeof(Image), true) as Image;

        my_target.new_card_anim0 =  EditorGUILayout.ObjectField("new_card_anim0", my_target.new_card_anim0 , typeof(Animation), true) as Animation;
        my_target.new_card_anim1 =  EditorGUILayout.ObjectField("new_card_anim1", my_target.new_card_anim1 , typeof(Animation), true) as Animation;
        my_target.new_card_anim2 =  EditorGUILayout.ObjectField("new_card_anim2", my_target.new_card_anim2 , typeof(Animation), true) as Animation;

        my_target.new_card_anim_sprite0 =  EditorGUILayout.ObjectField("new_card_anim_sprite0", my_target.new_card_anim_sprite0 , typeof(Image), true) as Image;
        my_target.new_card_anim_sprite1 =  EditorGUILayout.ObjectField("new_card_anim_sprite1", my_target.new_card_anim_sprite1 , typeof(Image), true) as Image;
        my_target.new_card_anim_sprite2 =  EditorGUILayout.ObjectField("new_card_anim_sprite2", my_target.new_card_anim_sprite2 , typeof(Image), true) as Image;
        my_target.GhostWasteActive_Image =  EditorGUILayout.ObjectField("Ghost Waste Active Image", my_target.GhostWasteActive_Image , typeof(Image), true) as Image;

        my_target.waste_card_anim1 =  EditorGUILayout.ObjectField("waste_card_anim1", my_target.waste_card_anim1 , typeof(Animation), true) as Animation;
        my_target.waste_card_anim2 =  EditorGUILayout.ObjectField("waste_card_anim2", my_target.waste_card_anim2 , typeof(Animation), true) as Animation;

        my_target.waste_card_anim_sprite1 =  EditorGUILayout.ObjectField("waste_card_anim_sprite1", my_target.waste_card_anim_sprite1 , typeof(Image), true) as Image;
        my_target.waste_card_anim_sprite2 =  EditorGUILayout.ObjectField("waste_card_anim_sprite2", my_target.waste_card_anim_sprite2 , typeof(Image), true) as Image;
           

        my_target.undo_button = EditorGUILayout.ObjectField("undo button", my_target.undo_button, typeof(Button), true) as Button;
        my_target.maxUndoSteps = EditorGUILayout.IntField("max undos", my_target.maxUndoSteps);
        my_target.board =  EditorGUILayout.ObjectField("board", my_target.board , typeof(Transform), true) as Transform;
        my_target.boardPile0 = EditorGUILayout.ObjectField("Board pile 0", my_target.boardPile0, typeof(Transform), true) as Transform;
        my_target.boardPile1 = EditorGUILayout.ObjectField("Board pile 1", my_target.boardPile1, typeof(Transform), true) as Transform;
        my_target.boardPile2 = EditorGUILayout.ObjectField("Board pile 2", my_target.boardPile2, typeof(Transform), true) as Transform;
        my_target.boardPile3 = EditorGUILayout.ObjectField("Board pile 3", my_target.boardPile3, typeof(Transform), true) as Transform;
        my_target.boardPile4 = EditorGUILayout.ObjectField("Board pile 4", my_target.boardPile4, typeof(Transform), true) as Transform;
        my_target.boardPile5 = EditorGUILayout.ObjectField("Board pile 5", my_target.boardPile5, typeof(Transform), true) as Transform;
        my_target.boardPile6 = EditorGUILayout.ObjectField("Board pile 6", my_target.boardPile6, typeof(Transform), true) as Transform;

        my_target.foundation_clubs = EditorGUILayout.ObjectField("Foundation clubs", my_target.foundation_clubs, typeof(Transform), true) as Transform;
        my_target.foundation_spades = EditorGUILayout.ObjectField("Foundation spades", my_target.foundation_spades, typeof(Transform), true) as Transform;
        my_target.foundation_hearts = EditorGUILayout.ObjectField("Foundation hearts", my_target.foundation_hearts, typeof(Transform), true) as Transform;
        my_target.foundation_diamonds = EditorGUILayout.ObjectField("Foundation diamonds", my_target.foundation_diamonds, typeof(Transform), true) as Transform;

        my_target.tableau_back_offset = EditorGUILayout.FloatField("TABLEAU BACK OFFSET", my_target.tableau_back_offset);
		my_target.tableau_normal_offset = EditorGUILayout.FloatField("TABLEAU NORMAL OFFSET", my_target.tableau_normal_offset);


        my_target.final_score = EditorGUILayout.ObjectField("Final Score", my_target.final_score, typeof(Text), true) as Text;
        my_target.final_bonus_score = EditorGUILayout.ObjectField("Final Bonus Score", my_target.final_bonus_score, typeof(Text), true) as Text;
        my_target.final_total_score = EditorGUILayout.ObjectField("Final Total Score", my_target.final_total_score, typeof(Text), true) as Text;
        my_target.bonus_text = EditorGUILayout.ObjectField("bonus text", my_target.bonus_text, typeof(Text), true) as Text;
        my_target.bonus_time = EditorGUILayout.FloatField("Bonus time (secs)", my_target.bonus_time);  
        my_target.score_text = EditorGUILayout.ObjectField("score text", my_target.score_text, typeof(Text), true) as Text;

        my_target.CAce_Board_To_Foundation = EditorGUILayout.IntField("Ace in board to foundation", my_target.CAce_Board_To_Foundation);  
        my_target.C2_Board_To_Foundation = EditorGUILayout.IntField("2 in board to foundation", my_target.C2_Board_To_Foundation); 
        my_target.C3_Board_To_Foundation = EditorGUILayout.IntField("3 in board to foundation", my_target.C3_Board_To_Foundation); 
        my_target.C4_Board_To_Foundation = EditorGUILayout.IntField("4 in board to foundation", my_target.C4_Board_To_Foundation); 
        my_target.C5_Board_To_Foundation = EditorGUILayout.IntField("5 in board to foundation", my_target.C5_Board_To_Foundation); 
        my_target.C6_Board_To_Foundation = EditorGUILayout.IntField("6 in board to foundation", my_target.C6_Board_To_Foundation); 
        my_target.C7_Board_To_Foundation = EditorGUILayout.IntField("7 in board to foundation", my_target.C7_Board_To_Foundation); 
        my_target.C8_Board_To_Foundation = EditorGUILayout.IntField("8 in board to foundation", my_target.C8_Board_To_Foundation); 
        my_target.C9_Board_To_Foundation = EditorGUILayout.IntField("9 in board to foundation", my_target.C9_Board_To_Foundation); 
        my_target.C10_Board_To_Foundation = EditorGUILayout.IntField("10 in board to foundation", my_target.C10_Board_To_Foundation); 
        my_target.CJ_Board_To_Foundation = EditorGUILayout.IntField("J in board to foundation", my_target.CJ_Board_To_Foundation); 
        my_target.CQ_Board_To_Foundation = EditorGUILayout.IntField("Q in board to foundation", my_target.CQ_Board_To_Foundation); 
        my_target.CK_Board_To_Foundation = EditorGUILayout.IntField("K in board to foundation", my_target.CK_Board_To_Foundation); 
        my_target.Exposed_Card = EditorGUILayout.IntField("Exposed card in board", my_target.Exposed_Card); 
        my_target.Waste_To_Foundation = EditorGUILayout.IntField("Waste to foundation", my_target.Waste_To_Foundation); 
        my_target.Waste_To_Board = EditorGUILayout.IntField("Waste to board", my_target.Waste_To_Board);
        my_target.Foundation_To_Board = EditorGUILayout.IntField("Foundation to board", my_target.Foundation_To_Board);

        my_target.show_win_or_lose_screen_after_delay = EditorGUILayout.FloatField("show gameOver screen after",my_target.show_win_or_lose_screen_after_delay);
        my_target.hide_win_or_lose_screen_after_delay = EditorGUILayout.FloatField("exit gameOver screen after",my_target.hide_win_or_lose_screen_after_delay);



            EditorGUI.indentLevel--;

        EditorGUILayout.Space();


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
