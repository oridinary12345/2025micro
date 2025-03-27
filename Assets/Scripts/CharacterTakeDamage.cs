public class CharacterTakeDamage : CharacterStateMessage
{
	public int Damage;

	public CharacterTakeDamage(Character from, Character to, int damage)
		: base(from, to)
	{
		Damage = damage;
	}
}