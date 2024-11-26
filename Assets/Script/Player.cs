using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private bool isTouching = false;

    private float swipeThreshold = 50f;
    private float moveSpeed = 10f;

    public Transform groundCheckUp, groundCheckDown, groundCheckLeft, groundCheckRight;
    public float groundCheckDistance = 0.5f;
    public LayerMask groundLayer;

    public List<GameObject> listBricks = new List<GameObject>();
    private UiChoi uiChoi;

    private bool isMoving = false;
    private Direction currentDirection = Direction.None;
    public Transform followPoint;
    public Transform lookAtPoint;


    private enum Direction { None, Up, Down, Left, Right }

    private void Start()
    {
        GetComponent<Rigidbody>().useGravity = false;
        uiChoi = FindObjectOfType<UiChoi>();
        SetupCamera();
    }

    private void Update()
    {
        if (!isMoving )
        {
            DetectSwipe();
        }
        MovePlayer();
    }

    private void DetectSwipe()
    {
        if (Application.isEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startTouchPosition = Input.mousePosition;
                isTouching = true;
            }
            else if (Input.GetMouseButtonUp(0) && isTouching)
            {
                currentTouchPosition = Input.mousePosition;
                HandleSwipe(currentTouchPosition - startTouchPosition);
                isTouching = false;
            }
        }
        else if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
                isTouching = true;
            }
            else if (touch.phase == TouchPhase.Ended && isTouching)
            {
                HandleSwipe(touch.position - startTouchPosition);
                isTouching = false;
            }
        }
    }

    private void HandleSwipe(Vector2 swipeDelta)
    {
        if (swipeDelta.magnitude < swipeThreshold) return;

        // Xác định hướng vuốt
        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            currentDirection = swipeDelta.x > 0 ? Direction.Right : Direction.Left;
        }
        else
        {
            currentDirection = swipeDelta.y > 0 ? Direction.Up : Direction.Down;
        }

        if (CanMove(currentDirection))
        {
            isMoving = true;
        }
    }

    private bool CanMove(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up: return IsGrounded(groundCheckUp);
            case Direction.Down: return IsGrounded(groundCheckDown);
            case Direction.Left: return IsGrounded(groundCheckLeft);
            case Direction.Right: return IsGrounded(groundCheckRight);
            default: return false;
        }
    }

    private void MovePlayer()
    {
        if (!isMoving) return;

        Vector3 moveDirection = Vector3.zero;

        switch (currentDirection)
        {
            case Direction.Up: moveDirection = Vector3.forward; break;
            case Direction.Down: moveDirection = Vector3.back; break;
            case Direction.Left: moveDirection = Vector3.left; break;
            case Direction.Right: moveDirection = Vector3.right; break;
        }

        if (moveDirection != Vector3.zero)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            if (!CanMove(currentDirection))
            {
                isMoving = false;
                currentDirection = Direction.None;
            }
        }
    }

    private bool IsGrounded(Transform groundCheck)
    {
        return Physics.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, groundLayer);
    }

    private void SetupCamera()
    {
        CameraSetup.GanCamera(this);
    }

    private void OnTriggerEnter(Collider collider)
    {
        switch (collider.tag)
        {
            case "Brick":
                CollectBrick(collider.gameObject);
                break;
            case "AnBrick":
                RemoveBrick();
                break;
            case "Xoahet":
                ClearBricks();
                break;
            case "Find":
                TriggerAnimation("Find");
                break;
        }
    }

    private void CollectBrick(GameObject brick)
    {
        listBricks.Add(brick);
        brick.transform.SetParent(transform);

        float brickHeight = 0.4f;
        brick.transform.localPosition = new Vector3(0, -listBricks.Count * brickHeight, 0);

        AdjustHeight(brickHeight);
        TriggerAnimation("Nhay");
    }

    private void RemoveBrick()
    {
        if (listBricks.Count == 0)
        {
            Debug.Log("Không còn gạch nào");
            StopMoving();
            return;
        }

        GameObject lastBrick = listBricks[^1];
        listBricks.RemoveAt(listBricks.Count - 1);
        Destroy(lastBrick);

        AdjustHeight(-0.4f);
        TriggerAnimation("Nhay");
    }

    private void ClearBricks()
    {
        int diem = listBricks.Count;
        listBricks.Clear();
        transform.position = new Vector3(transform.position.x, 2.84f, transform.position.z);
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child.name == "dimian") Destroy(child.gameObject);
        }
        uiChoi.Find();
        uiChoi.diem.text = "diem: " + diem.ToString();
    }

    private void StopMoving()
    {
        isMoving = false;
        currentDirection = Direction.None;
    }

    private void AdjustHeight(float heightChange)
    {
        transform.position += new Vector3(0, heightChange, 0);
    }

    private void TriggerAnimation(string animationName)
    {
        Animator anim = GetComponentInChildren<Animator>();
        anim.SetTrigger(animationName);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        DrawRay(groundCheckUp);
        DrawRay(groundCheckDown);
        DrawRay(groundCheckLeft);
        DrawRay(groundCheckRight);
    }

    private void DrawRay(Transform groundCheck)
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
    }
}
