using System.Collections.Generic;

public interface IModeTransitionRequester
{
    void RequestTransition(Mode mode);
}

// Add only the modes that outside systems may request via RequestTransition().
public enum Mode { Normal, Build, SetSpawnPoint, SelectTarget }

public class PlayerStateMachine : IModeTransitionRequester
{
    public ModeBase CurrentMode { get; private set; }

    private readonly ModeBase normalMode;
    private readonly ModeBase buildMode;
    private readonly ModeBase spawnPositionSetMode;
    private readonly ModeBase selectTargetMode;

    public PlayerStateMachine(NormalMode normalMode, BuildMode buildMode, SetPositionMode spawnPositionSetMode, SelectTargetMode selectTargetMode)
    {
        this.normalMode = normalMode;
        this.buildMode = buildMode;
        this.spawnPositionSetMode = spawnPositionSetMode;
        this.selectTargetMode = selectTargetMode;
        
        normalMode.Setup(this);
        buildMode.Setup(this);
        spawnPositionSetMode.Setup(this);
        selectTargetMode.Setup(this);
    }

    public void SetMode(ModeBase newMode)
    {
        CurrentMode?.Exit();
        CurrentMode = newMode;
        CurrentMode.Enter();
    }

    public void Update()
    {
        CurrentMode.Update();
    }

    public void HandleInput()
    {
        CurrentMode.HandleInput();
    }

    public void RequestTransition(Mode mode)
    {
        switch (mode)
        {
            case Mode.Normal:
                SetMode(normalMode);
                break;
            case Mode.Build:
                SetMode(buildMode);
                break;
            case Mode.SetSpawnPoint:
                SetMode(spawnPositionSetMode);
                break;
            case Mode.SelectTarget:
                SetMode(selectTargetMode);
                break;
        }
    }
}