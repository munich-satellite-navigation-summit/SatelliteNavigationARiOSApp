using System.Collections;

using Helpers;

using UnityEngine;

using Views;

namespace Controllers.ARScene
{
    public class PlacementState : StateControlBase
    {
        [SerializeField] private ARFocusSquareControl _ARFocusSquareControl;
        //[SerializeField] private PlacementARModeMenuController _placementArModeMenuController;
        [SerializeField] private PlaceDetectedMenuControl _placeDetectedMenuControl;
        [SerializeField] private ModelsControl _modelsController;
        //[SerializeField] private RotateModelControl _rotateModel;

        public new void Init()
        {
            base.Init();
            Debug.Log("PlacementState  Init Start");
            //_rotateModel.Init();
            _ARFocusSquareControl.Init();
            Debug.Log("PlacementState  Init");
            _ARFocusSquareControl.ChangedFocusState += state =>
            {
                if (state != ARFocusSquareControl.FocusState.Initializing && state != ARFocusSquareControl.FocusState.Stoped)
                {
                    if (state == ARFocusSquareControl.FocusState.Found)
                    {
                        _modelsController.Show(ModelView.ModelType.Square);
                        Debug.Log(" _ARFocusSquareControl.ChangedFocusState FOUND ");
                    }
                    else
                    {
                        _modelsController.Hide(ModelView.ModelType.Square);
                    }
                }
            };
            _ARFocusSquareControl.FoundPlace += (position, rotation) =>
            {
                _modelsController.Move(ModelView.ModelType.Square, position);
                //TODO uncomment if not work
                _modelsController.Rotate(ModelView.ModelType.Square, rotation);
                //Debug.Log (" _placeDetectedMenuControl.AddOnClickSetPointListener ");
                //_ARFocusSquareControl.StopTracking ( );
                //_modelsController.Hide ( );
                //_modelsController.Show (ModelView.ModelType.Earth, position, rotation);
                //_rotateModel.Enable();

                //Debug.Log(_placeDetectedMenuControl);
                //_placeDetectedMenuControl.Disable();
            };

            _ARFocusSquareControl.FoundPlace += _placeDetectedMenuControl.OnFoundPlace;
            _ARFocusSquareControl.LoseFocuse += _placeDetectedMenuControl.OnLoseFocuse;

            _placeDetectedMenuControl.Init();
            _placeDetectedMenuControl.AddOnClickSetPointListener((position, rotation) =>
            {
                Debug.Log(" _placeDetectedMenuControl.AddOnClickSetPointListener ");
                _ARFocusSquareControl.StopTracking();
                _modelsController.Hide();
                _modelsController.Show(ModelView.ModelType.Earth, position, rotation);
                 //_rotateModel.Enable();

                 Debug.Log(_placeDetectedMenuControl);
                _placeDetectedMenuControl.Disable();
                 //ActiveState<ModelState>();
             });
            Debug.Log("PlacementState  Init End");
            //_placementArModeMenuController.Init();
        }

        public IEnumerator EnterState()
        {
            yield return null;
            _ARFocusSquareControl.Enable();
            _ARFocusSquareControl.StartTracking();
            Debug.Log("PlacementState  EnterState");
            //_placementArModeMenuController.Enable();
        }

        public override IEnumerator ExitState()
        {
            _ARFocusSquareControl.Disable();
            _ARFocusSquareControl.StopTracking();
            //_placementArModeMenuController.Disable();
            yield return base.ExitState();
        }

        protected override void OnStartProcessing()
        {

        }
    }
}
