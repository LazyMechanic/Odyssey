using Leopotam.Ecs;
using UnityEngine;
using VRTK;

namespace Client {
    [EcsIgnoreInFilter]
    sealed class JoystickBehaviour : VRTK_InteractableObject
    {
    //    public Action OnUseStarted;
    //    public Action OnUseStopped;

    //    [Header("Joystick")]
    //    [SerializeField]
    //    Transform _handle;

    //    [SerializeField]
    //    float _maxXAngle = 45;

    //    [SerializeField]
    //    float _maxZAngle = 45;

    //    [SerializeField]
    //    [Range(0, 1)]
    //    float _vibrationStrenght = .1f;

    //    [SerializeField]
    //    [Range(0, 1)]
    //    float _vibrationInterval = .05f;

    //    public float XPercentage;
    //    public float ZPercentage;

    //    VRTK_ControllerActions _controllerActions;

    //    bool _grabbing;

    //    override protected void Update()
    //    {
    //        base.Update();

    //        var angleX = _handle.localRotation.eulerAngles.x;
    //        if (angleX > 180)
    //            angleX -= 360;

    //        var angleZ = _handle.localRotation.eulerAngles.z;
    //        if (angleZ > 180)
    //            angleZ -= 360;

    //        XPercentage = angleX / _maxXAngle;
    //        ZPercentage = angleZ / _maxZAngle;
    //    }

    //    public override void StartTouching(GameObject currentTouchingObject)
    //    {
    //        base.StartTouching(currentTouchingObject);
    //        _controllerActions = currentTouchingObject.GetComponent<VRTK_ControllerActions>();
    //        Vibrate(.5f);
    //    }

    //    public override void StopTouching(GameObject previousTouchingObject)
    //    {
    //        base.StopTouching(previousTouchingObject);
    //        _controllerActions = previousTouchingObject.GetComponent<VRTK_ControllerActions>();
    //        Vibrate(.5f);
    //    }

    //    override public void Grabbed(GameObject grabbingObject)
    //    {
    //        base.Grabbed(grabbingObject);
    //        _controllerActions = grabbingObject.GetComponent<VRTK_ControllerActions>();
    //        Vibrate(.5f);
    //        _grabbing = true;

    //        StartCoroutine(DetectJoystickMovement());
    //    }

    //    override public void Ungrabbed(GameObject previousGrabbingObject)
    //    {
    //        base.Ungrabbed(previousGrabbingObject);

    //        _grabbing = false;
    //    }

    //    IEnumerator DetectJoystickMovement()
    //    {
    //        var currentXPercentage = XPercentage;
    //        var currentZPercentage = ZPercentage;
    //        while (_grabbing)
    //        {
    //            if (Mathf.Abs(currentXPercentage - XPercentage) > _vibrationInterval)
    //            {
    //                Vibrate(_vibrationStrenght);
    //                currentXPercentage = XPercentage;
    //            }
    //            else if (Mathf.Abs(currentZPercentage - ZPercentage) > _vibrationInterval)
    //            {
    //                Vibrate(_vibrationStrenght);
    //                currentZPercentage = ZPercentage;
    //            }

    //            yield return null;
    //        }
    //    }

    //    public override void StartUsing(GameObject currentUsingObject)
    //    {
    //        base.StartUsing(currentUsingObject);

    //        if (OnUseStarted != null)
    //            OnUseStarted();
    //    }

    //    public override void StopUsing(GameObject previousUsingObject)
    //    {
    //        base.StopUsing(previousUsingObject);

    //        if (OnUseStopped != null)
    //            OnUseStopped();
    //    }

    //    public void Vibrate(float vibrationStrength)
    //    {
    //        if (_controllerActions != null)
    //            _controllerActions.TriggerHapticPulse(vibrationStrength);
    //    }
    }
}