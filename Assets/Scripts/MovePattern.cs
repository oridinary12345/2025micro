using System.Collections.Generic;
using Utils;

public class MovePattern
{
	private List<MoveDirection> DirectionsForward;

	private List<MoveDirection> DirectionsBackward;

	public static MovePattern FromString(string patternForward, string patternBackward)
	{
		string[] collection = patternForward.Split(',');
		List<MoveDirection> directionsForward = new List<string>(collection).ConvertAll((string m) => Enum.TryParse(m.ToUpperFirst(), MoveDirection.Center));
		string[] collection2 = patternBackward.Split(',');
		List<MoveDirection> directionsBackward = new List<string>(collection2).ConvertAll((string m) => Enum.TryParse(m.ToUpperFirst(), MoveDirection.Center));
		MovePattern movePattern = new MovePattern();
		movePattern.DirectionsForward = directionsForward;
		movePattern.DirectionsBackward = directionsBackward;
		return movePattern;
	}

	public List<MoveDirection> GetDirections(bool forward)
	{
		return (!forward) ? DirectionsBackward : DirectionsForward;
	}
}