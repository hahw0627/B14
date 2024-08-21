public interface IDamageable
{
    void TakeDamage(int damage, bool isSkillDamage = false, bool isPetAttack = false);
}