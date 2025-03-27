public abstract class CharacterStateMessage
{
	public Character From;

	public Character To;

	protected CharacterStateMessage(Character from, Character to)
	{
		From = from;
		To = to;
	}
}