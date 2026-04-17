using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "CharacterData/CharacterData")]
public class CharacterData_SO : ScriptableObject
{
    [Header("Base Stats")]
    public string characterName;
    public float maxHealth;
    public float movementSpeed;
    public int attackPower;

    [Header("Animation Attributes")]
    public AnimationClip idleAnimation;
    public AnimationClip moveAnimation;
    public AnimationClip attackAnimation;
    public AnimationClip jumpAnimation;
    public AnimationClip fallAnimation;
    public AnimationClip deathAnimation;


}
