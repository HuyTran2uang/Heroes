using UnityEngine;

public class PreviewController : MonoBehaviour
{
    [SerializeField] private bool _isStartingMode;
    [SerializeField] private Transform _model;
    private Vector3 _position;

    public void StartModePreview()
    {
        _isStartingMode = true;
    }

    private void Update()
    {
        if (!_isStartingMode) return;

        if (Input.GetMouseButtonDown(0))
        {
            _position = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 position = Input.mousePosition;
            if (position.x > _position.x)
            {
                _model.Rotate(Vector3.down);
            }
            if (position.x < _position.x)
            {
                _model.Rotate(Vector3.up);
            }
            _position = position;
        }

        if(Input.GetMouseButtonUp(0))
        {
            _isStartingMode = false;
        }
    }
}
