using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class ButtonGroupManager : MonoBehaviour
{
    [SerializeField] FocusType focusType;

    public FocusType GetFocusType()
    {
        return focusType;
    }
}