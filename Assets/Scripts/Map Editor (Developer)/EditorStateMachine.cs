using Unity.VisualScripting;
using UnityEngine;

public class EditorStateMachine : IModeTransitionRequester
{
    public ModeBase CurrentMode { get; private set; }

    private readonly ModeBase selectMode;
    private readonly ModeBase buildMode;


    public EditorStateMachine(SelectMode selectMode, BuildMode buildMode)
    {
        this.selectMode = selectMode;
        this.buildMode = buildMode;

        selectMode.Setup(this);
        buildMode.Setup(this);
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
                SetMode(selectMode);
                break;
            case Mode.Build:
                SetMode(buildMode);
                break;
        }
    }
}
