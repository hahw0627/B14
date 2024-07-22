using UnityEngine;

[CreateAssetMenu(fileName = "NewPetData", menuName = "ScriptableObjects/PetDataSO", order = 3)]
public class PetDataSO : ScriptableObject
{
    public int petNumber;
    public int damage;
    public float attackSpeed;
}
