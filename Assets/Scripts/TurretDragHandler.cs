using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TurretDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [Header("References")]
    [SerializeField] private GameObject turretPrefab; 


    [Header("Attributes")]
    [SerializeField] private Color validColor = Color.green;
    [SerializeField] private Color invalidColor = Color.red;

    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 initialPosition;
    private GameObject dragImage;
   
    

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        initialPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Create a new Image GameObject for dragging
        dragImage = new GameObject("DragImage", typeof(Image), typeof(CanvasGroup));
        dragImage.transform.SetParent(canvas.transform, false);

        // Copy the sprite from the original Image
        Image originalImage = GetComponent<Image>();
        Image dragImageComponent = dragImage.GetComponent<Image>();
        dragImageComponent.sprite = originalImage.sprite;

        // Match the size of the original image
        dragImageComponent.rectTransform.sizeDelta = originalImage.rectTransform.sizeDelta;

        // Set the drag image properties
        CanvasGroup dragCanvasGroup = dragImage.GetComponent<CanvasGroup>();
        dragCanvasGroup.alpha = 0.6f;
        dragCanvasGroup.blocksRaycasts = false;

        // Set the initial position of the drag image
        dragImage.transform.position = originalImage.transform.position;

        canvasGroup.blocksRaycasts = true; // Keep the original icon interactable
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragImage != null)
        {
            RectTransform dragRectTransform = dragImage.GetComponent<RectTransform>();
            dragRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

            // Check if the current position is valid
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bool validPosition = IsValidPlacement(worldPosition);

            // Debug statement to check position validity
            Debug.Log("Position Valid: " + validPosition);

            // Change the color based on validity
            dragImage.GetComponent<Image>().color = validPosition ? validColor : invalidColor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragImage != null)
        {
            // Convert screen point to world point
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (IsValidPlacement(worldPosition))
            {
                PlaceTurret(worldPosition);
            }

            // Destroy the drag image
            Destroy(dragImage);
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    private bool IsValidPlacement(Vector2 position)
    {
        // Check if the mouse pointer is over any UI element
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return false;
        }

        // Check if the position is over the path or another turret
        Collider2D hitCollider = Physics2D.OverlapPoint(position);
        if (hitCollider != null)
        {
            if (hitCollider.CompareTag("Path") || hitCollider.CompareTag("Turret"))
            {
                return false;
            }
        }

        // Check if the position is too close to another turret
        float minDistanceBetweenTurrets = 1f; // Adjust as necessary
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, minDistanceBetweenTurrets);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Turret"))
            {
                return false;
            }
        }

        return true;
    }

    private void PlaceTurret(Vector2 position)
    {
        Instantiate(turretPrefab, position, Quaternion.identity);
    }
}
