using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class CreateAnimation : EditorWindow
{
    // The selected folder containing the sprites
    private Object spriteFolder;

    // The list of sprites in the selected folder
    private List<Sprite> sprites;

    [MenuItem("Window/Sprite Animation Creator")]
    public static void ShowWindow()
    {
        // Show the editor window
        EditorWindow.GetWindow(typeof(CreateAnimation));
    }

    private void OnGUI()
    {
        // Create a field for the user to select the sprite folder
        spriteFolder = EditorGUILayout.ObjectField("Sprite Folder", spriteFolder, typeof(Object), false);

        // If the sprite folder has been selected
        if (spriteFolder != null)
        {
            // Get the path of the selected folder
            string spriteFolderPath = AssetDatabase.GetAssetPath(spriteFolder);

            // Load all of the sprites in the selected folder
            sprites = new List<Sprite>(AssetDatabase.LoadAllAssetsAtPath(spriteFolderPath).OfType<Sprite>().ToArray());

            // Display the list of sprites in the selected folder
            foreach (Sprite sprite in sprites)
            {
                EditorGUILayout.LabelField(sprite.name);
            }

            // Create a button to create the animation
            if (GUILayout.Button("Create Animation"))
            {
                CreateAnimationClip();
            }
        }
    }

    private void CreateAnimationClip()
    {
        // Create an animation clip
        AnimationClip clip = new AnimationClip();

        // Create an animation curve for each sprite in the list
        foreach (Sprite sprite in sprites)
        {
            // Create a keyframe for each sprite
            Keyframe key = new Keyframe(sprites.IndexOf(sprite), sprites.IndexOf(sprite));
            AnimationCurve curve = new AnimationCurve(key);

            // Set the sprite as the value of the curve
            curve.preWrapMode = WrapMode.Default;
            curve.postWrapMode = WrapMode.Default;
            clip.SetCurve("", typeof(SpriteRenderer), "m_Sprite", curve);
        }

        // Save the animation clip
        AssetDatabase.CreateAsset(clip, "Assets/New Animation.anim");
        AssetDatabase.SaveAssets();
    }
}