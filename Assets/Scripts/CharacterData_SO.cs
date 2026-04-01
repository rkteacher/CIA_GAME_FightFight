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
    public Animation idleAnimation;
    public Animation moveAnimation;
    public Animation attackAnimation;
    public Animation jumpAnimation;
    public Animation fallAnimation;
    public Animation deathAnimation;

}
