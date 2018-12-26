using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(card))]
internal class card_editor : Editor {

    public override void OnInspectorGUI()
    {
        card my_target = (card)target;
        EditorGUI.BeginChangeCheck ();
        Undo.RecordObject(my_target, "edit_card");

        my_target.my_back = EditorGUILayout.ObjectField("back sprite",my_target.my_back, typeof(Sprite), false) as Sprite;

        my_target.my_setup_selected = (card.my_setup)EditorGUILayout.EnumPopup("setup",my_target.my_setup_selected);
        if (my_target.my_setup_selected == card.my_setup.manual)
        {
            EditorGUI.indentLevel++;
            
            my_target.card_suit_selected = (card.card_suit)EditorGUILayout.EnumPopup("suit",my_target.card_suit_selected);
			if (my_target.card_suit_selected != card.card_suit.random)
			{
				my_target.my_suit = (int)my_target.card_suit_selected;
			}

            my_target.card_rank_selected = (card.card_rank)EditorGUILayout.EnumPopup("rank",my_target.card_rank_selected);
			if (my_target.card_rank_selected != card.card_rank.random)
			{
				my_target.my_rank = (int)my_target.card_rank_selected;
			}

            my_target.always_face_up = EditorGUILayout.Toggle("always face up", my_target.always_face_up);

            EditorGUI.indentLevel--;
        }
        else //automatic
        {
            my_target.card_suit_selected = card.card_suit.random;
            my_target.card_rank_selected = card.card_rank.random;
            my_target.face_up = false;
        }


        if (EditorGUI.EndChangeCheck ())
            EditorUtility.SetDirty(my_target);

    }

}
