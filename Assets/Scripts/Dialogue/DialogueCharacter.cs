using UnityEngine;

[CreateAssetMenu(fileName = "DialogueCharacter", menuName = "DialogueCharacter", order = 0)]
public class DialogueCharacter : ScriptableObject
{
    public string characterName;
    public Sprite mouthClosedSprite;
    public Sprite mouthOpenSprite;
}