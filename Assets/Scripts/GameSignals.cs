public class StartGameSignal { }

public class EndGameSignal { }

public class WobbleDestroyedSignal { }

public class OuterLayerTouchedSignal { }

public class InnerLayerTouchedSignal { }

public class LoseLifeSignal { }


public class MoveWobblesSignal
{
    public bool[] wobbleGroups = new bool[4];
}