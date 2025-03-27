public interface ICharacterState
{
	void OnStateEnter(Character character);

	void OnStateUpdate(Character character);

	void OnStateExit(Character character);

	void OnMessage(CharacterStateMessage message);
}