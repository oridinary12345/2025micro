public interface IGameState
{
	void OnStateEnter(GameController gameController);

	void OnStateUpdate(GameController gameController);

	void OnStateExit(GameController gameController);

	void OnMessage(GameController gameController, GameStateMessage message);
}