using Unity.VisualScripting;
using UnityEngine;
public interface IModeTransitionRequester
{
    void RequestTransition(Mode mode);
}

// Add only the modes that outside systems may request via RequestTransition().
public enum Mode { Normal, Build, SetSpawnPoint }

public class PlayerStateMachine : IModeTransitionRequester
{
    public ModeBase CurrentMode { get; private set; }

    private ModeBase normalMode;
    private ModeBase buildMode;
    private ModeBase spawnPositionSetMode;

    public PlayerStateMachine(NormalMode normalMode, BuildMode buildMode, SetPositionMode spawnPositionSetMode)
    {
        this.normalMode = normalMode;
        this.buildMode = buildMode;
        this.spawnPositionSetMode = spawnPositionSetMode;
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
        }
    }
}