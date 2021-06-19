using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    /*public float zoomSpeed = 100f;
    public float zoomTime = 0.1f;

    public float maxHeight = 100f;
    public float minHeight = 20f;

    public float focusHeight = 10f;
    public float focusDistance = 60f;

    public int panBorder = 25;
    public float dragPanSpeed = 25f;
    public float edgePanSpeed = 25f;
    public float keyPanSpeed = 25f;

    private float zoomVelocity = 0f;
    private float targetHeight;

    void Start()
    {
        // Start zoomed out
        targetHeight = maxHeight;
        //_zoom = _camera.fieldOfView;
    }*/

    /*
     * private float _zoom;
     * float smoothTime = 0.15f;
     * public float zoomSensitivity = 50f;
     * public float scrollSpeed = 40f;
     */

    private Camera _camera;
    private Vector3 _startPosition;

    public float panSpeed = 30f;
    public float panBorderThickness = 40f;
    public Vector2 panLimit;

    // Zoom parameters
    private float _scrollSpeed = 10000f;
    private float _yAfterZoom;

    // Zoom with SmoothDamp
    private float _yVelocity = 0f;
    private float _smoothTime = 0.1f;

    // Zoom with manual lerp
    private float _zoomLerpDuration = 0.5f;
    private bool _isZooming = false;

    private float _elapsedTime = 0f;
    private float _startTime = 0f;
    private float _startY = 0f;
    private float _endY = 0f;

    // Zoom limits
    public float minY = 10f;
    public float maxY = 80f;

	private void Start()
    {
        _camera = GetComponent<Camera>();
        _startPosition = transform.position;
        _yAfterZoom = transform.position.y;
        _startY = transform.position.y;
        _endY = _startY;
    }

    private void Update()
    {
        HandleMovement();
        // HandleRotation();
        HandleZoom();
        // HandleZoomWithManualLerp();
    }

    private void HandleMovement()
    {
        Vector3 cameraPosition = transform.position;

        if (Input.GetKey(KeyCode.Z) || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            cameraPosition.z += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= panBorderThickness)
        {
            cameraPosition.z -= panSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            cameraPosition.x += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Q) || Input.mousePosition.x <= panBorderThickness)
        {
            cameraPosition.x -= panSpeed * Time.deltaTime;
        }

        cameraPosition.x = Mathf.Clamp(cameraPosition.x, _startPosition.x - panLimit.x, _startPosition.x + panLimit.x);
        cameraPosition.z = Mathf.Clamp(cameraPosition.z, _startPosition.z - panLimit.y, _startPosition.z + panLimit.y * 2f);

        transform.position = cameraPosition;
    }

    private void HandleRotation()
    {
        Vector3 cameraPosition = transform.position;
    }

    private void HandleZoom()
    {
        var cameraPosition = transform.position;

        // We cannot use the current position of the camera as we enter this function before 
        // the scroll movement has finished from the previous one (because of SmoothDamp which runs
        // in a defined duration). This is why targetHeight needs to be a global variable.
        // var targetHeight = cameraPosition.y;
        // var yAfterZoom = cameraPosition.y;

        // One notch of scroll wheel
        var scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        if (scrollWheel != 0f)
		{
            // Debug.Log($"Mouse ScrollWheel: {scrollWheel}");
            _elapsedTime = 0f;
            _startTime = Time.time;
            _startY = transform.position.y;
            _endY = _endY - scrollWheel *_scrollSpeed * Time.deltaTime;
            _endY = Mathf.Clamp(_endY, minY, maxY);
        }

        // First, calculate the height we want the camera to be at
        _yAfterZoom -= Input.GetAxis("Mouse ScrollWheel") * _scrollSpeed * Time.deltaTime;
        _yAfterZoom = Mathf.Clamp(_yAfterZoom, minY, maxY);

        // Then, interpolate smoothly towards that height
        // cameraPosition.y = Mathf.SmoothDamp(transform.position.y, _yAfterZoom, ref _yVelocity, _smoothTime);
        // cameraPosition.y = Mathf.Lerp(transform.position.y, _yAfterZoom, Time.deltaTime * 80f);
        var elapsedTime = Time.time - _startTime;
        var lerpTime = _elapsedTime / _zoomLerpDuration;

        lerpTime = 1f - Mathf.Pow((1f - lerpTime), 3f);
        cameraPosition.y = Mathf.Lerp(_startY, _endY, lerpTime);

        if (lerpTime < 1f)
		{
            // Debug.Log($"Lerp time: {lerpTime}");
        }

        _elapsedTime += Time.deltaTime;

        //Debug.Log($"Interpolate index: {Time.deltaTime * 80f}");

        //var focusPosition = new Vector3(cameraPosition.x, 5f, cameraPosition.z + 60f);

        transform.position = cameraPosition;
        //transform.LookAt(focusPosition);
    }

    private void HandleZoomWithManualLerp()
    {
        var scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        // TODO Check if it returns the same value as the above one.
        // var scrollDelta = Input.mouseScrollDelta;

        /*float timeSinceStart = Time.time - startTime;
        var x = timeSinceStart / duration;

        cameraPosition.y = Mathf.Lerp(transform.position.y, _yAfterZoom, x);*/

        if (scrollWheel == 0f)
        {
            return;
        }

        if (!_isZooming)
        {
            StartCoroutine(RunZoomWithLerp(scrollWheel));
        }
    }

    private IEnumerator RunZoomWithLerp(float scrollWheel)
    {
        _isZooming = true;

        var elapsedTime = 0f;
        var yAfterZoom = transform.position.y;
        var startPosition = transform.position.y;
        yAfterZoom -= scrollWheel * _scrollSpeed * Time.deltaTime;
        yAfterZoom = Mathf.Clamp(yAfterZoom, minY, maxY);

        while (elapsedTime < _zoomLerpDuration)
        {
            var cameraPosition = transform.position;
            cameraPosition.y = Mathf.Lerp(startPosition, yAfterZoom, elapsedTime / _zoomLerpDuration);
            transform.position = cameraPosition;

            elapsedTime += Time.deltaTime;

            // Waits until the next frame to continue
            yield return null;
        }

        // transform.position = new Vector3(transform.position.x, yAfterZoom, transform.position.z);

        _isZooming = false;
    }

    /*private void HandleZoom()
    {
        Vector3 cameraPosition = transform.position;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        _zoom -= scroll * zoomSensitivity;
        Debug.Log($"Mouse ScrollWheel: {_zoom}");
        _zoom = Mathf.Clamp(_zoom, minY, maxY);
        cameraPosition.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;

        cameraPosition.y = Mathf.Clamp(cameraPosition.y, minY, maxY);

        transform.position = cameraPosition;

        //_camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, _zoom, Time.deltaTime * scrollSpeed);
        // _camera.fieldOfView = Mathf.SmoothDamp(_camera.fieldOfView, _zoom, ref yVelocity, smoothTime);

        // SCRIPT 

        var newPosition = transform.position;

        // First, calculate the height we want the camera to be at
        targetHeight += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * -1f;
        targetHeight = Mathf.Clamp(targetHeight, minHeight, maxHeight);

        // Then, interpolate smoothly towards that height
        newPosition.y = Mathf.SmoothDamp(transform.position.y, targetHeight, ref zoomVelocity, zoomTime);

        var focusPosition = new Vector3(newPosition.x, focusHeight, newPosition.z + focusDistance);

        transform.position = newPosition;
        transform.LookAt(focusPosition);
    }*/

    /*private bool isMovementAllowed = true;

    public float panSpeed = 30f;
    public float panBorderThickness = 10f;

    public float scrollSpeed = 5f;
    public float minY = 10f;
    public float maxY = 80f;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isMovementAllowed = !isMovementAllowed;
        }

        if (!isMovementAllowed)
        {
            return;
        }

        if (Input.GetKey("z") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey("q") || Input.mousePosition.x <= panBorderThickness)
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        Vector3 pos = transform.position;

        pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }*/
}
