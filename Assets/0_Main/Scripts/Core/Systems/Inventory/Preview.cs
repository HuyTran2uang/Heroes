using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory
{
    public class Preview : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private PreviewController _previewController;

        private void StartPreview()
        {
            _previewController.StartModePreview();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            StartPreview();
        }
    }
}
