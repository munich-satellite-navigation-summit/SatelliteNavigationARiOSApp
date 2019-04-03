using UnityEngine;
/// <summary>
/// Look an object at the main camera.
/// </summary>
public class LookAtCamera : MonoBehaviour
{
    private Transform _cameraTransform;
    private Transform _transform;
    private float _cameraYPos;

	private void Awake()
	{
		var camera = Camera.current;
		if (camera == null)
		{
			camera = Camera.main;
		}
		_cameraTransform = camera.transform;
		_transform = transform;
        _cameraYPos = _cameraTransform.position.y;
	}

	private void LateUpdate()
	{
        Vector3 vector = new Vector3(_cameraTransform.position.x, this.transform.position.y,
            _cameraTransform.transform.position.z);
		_transform.LookAt(vector);
    }
}

