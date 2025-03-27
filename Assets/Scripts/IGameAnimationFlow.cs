using System.Collections.Generic;

public interface IGameAnimationFlow
{
	bool Finished
	{
		get;
	}

	void StartFlow(List<Monster> monsters);
}