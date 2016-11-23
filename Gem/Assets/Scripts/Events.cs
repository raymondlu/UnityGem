public abstract class GameEvent{
}

public class GameEventStartLoading : GameEvent{
}

public class GameEventUpdateLoading : GameEvent{
	public int percent;
}

public class GameEventFinishLoading : GameEvent{
}

public class GameEventGoToMainMenuScene : GameEvent{
}