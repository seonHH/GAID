using UnityEngine;
using UnityEngine.SceneManagement;

public class GazePointScript : MonoBehaviour
{
    public GameObject gazePoint;     // UI element
    public GameObject targetObject;  // World space object

    private RectTransform gazeRectTransform;
    private Collider2D targetCollider;
    private Animator targetAnimator;

    public GameObject congratsMessage;

    void Start()
    {
        // Get the RectTransform of the gazePoint UI element
        gazeRectTransform = gazePoint.GetComponent<RectTransform>();

        // Get the Collider2D and Animator components of the target object
        targetCollider = targetObject.GetComponent<Collider2D>();
        targetAnimator = targetObject.GetComponent<Animator>();

        congratsMessage.SetActive(false);
    }

    void Update()
    {
        // Check if the gazePoint is active before proceeding
        if (!gazePoint.activeInHierarchy)
        {
            targetAnimator.enabled = false;
            return;
        }

        // Convert the gazePoint's position to world space
        Vector3 worldPoint;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(gazeRectTransform, gazeRectTransform.position, Camera.main, out worldPoint))
        {
            // Check if the world point is within the bounds of the target's collider
            if (targetCollider.OverlapPoint(worldPoint))
            {
                targetAnimator.enabled = true;
                Debug.Log("Gaze point is on target");
            }
            else
            {
                targetAnimator.enabled = false;
                Debug.Log("Gaze point is not on target");
            }
        }
        else
        {
            targetAnimator.enabled = false;
            Debug.Log("Failed to convert gaze point to world point");
        }

        // Check if the animation has finished a cycle
        AnimatorStateInfo stateInfo = targetAnimator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.loop && stateInfo.normalizedTime >= 1.0f)
        {
            OnAnimationCycleEnd();
        }
    }

    public void OnAnimationCycleEnd()
    {
        targetAnimator.enabled = false;
        Debug.Log("Animation cycle ended, animator disabled");
        congratsMessage.SetActive(true);

        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            // Change the scene to MainScene
            SceneManager.LoadScene("MainScene");
        }
    }
}
