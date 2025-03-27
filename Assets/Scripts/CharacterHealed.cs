public class CharacterHealed : CharacterStateMessage
{
	public CharacterHealed(Character from, Character to)
		: base(from, to)
	{
	}
}