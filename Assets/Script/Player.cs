using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;
using System;
using UnityEditor;
using UnityEngine.Assertions.Must;

public class Player : MonoBehaviour
{

    private Vector2 startTouchPosition;
    private Vector2 currentPosition;
    private Vector2 endTouchPosition;
    private bool stopTouch = false;

    private float swipeRange;
    private float tapRange;

    private float moveSpeed = 10f;
    public Transform groundCheckUp;
    public float groundCheckDistanceUp;
    public LayerMask WhatIsGround;

    public Transform groundCheckDown;
    public float groundCheckDistanceDown;


    public Transform groundCheckLeft;
    public float groundCheckDistanceLeft;


    public Transform groundCheckRight;
    public float groundCheckDistanceRight;


    public bool isGroundedUp;
    public bool isGroundedDown;
    public bool isGroundedLeft;
    public bool isGroundedRight;

    private bool isMovingUp = false;
    private bool isMovingDown = false;
    private bool isMovingLeft = false;
    private bool isMovingRight = false;

    private bool isMoving = false;
    public List<GameObject> listBricks = new List<GameObject>();

    private UiChoi uiChoi;

    Player player;

    void Start()
    {
        GetComponent<Rigidbody>().useGravity = false;
        player = GetComponent<Player>();
        uiChoi = FindObjectOfType<UiChoi>();
    }

    void Update()
    {

        if (!isMoving)
        {
            Swipes();
        }
        if (!isMoving)
        {
            Swipe();
        }
        #region Moves
        isGroundedUp = IsGroundDetectedUP();
        isGroundedDown = IsGroundDetectedDown();
        isGroundedLeft = IsGroundDetectedLeft();
        isGroundedRight = IsGroundDetectedRight();

        if (isMovingUp)
        {
            if (!isGroundedUp)
            {
                isMovingUp = false;
                isMoving = false; 
            }
            else
            {
                isMoving = true;
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
        }

        if (isMovingDown)
        {
            if (!isGroundedDown)
            {
                isMovingDown = false;
                isMoving = false;
            }
            else
            {
                isMoving = true;
                transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
            }
        }

        if (isMovingLeft)
        {
            if (!isGroundedLeft)
            {
                isMovingLeft = false;
                isMoving = false;
            }
            else
            {
                isMoving = true;
                transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            }
        }

        if (isMovingRight)
        {
            if (!isGroundedRight)
            {
                isMovingRight = false;
                isMoving = false;
            }
            else
            {
                isMoving = true;
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            }
        }
        #endregion


    }

    public void Swipes()
    {
        if (Application.isEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startTouchPosition = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                currentPosition = Input.mousePosition;
                Vector2 distance = (Vector2)currentPosition - startTouchPosition;
                if (!stopTouch)
                {
                    if (distance.x < -swipeRange && isGroundedLeft && !isMovingLeft)
                    {
                        Debug.Log("left");
                        isMovingLeft = true;
                        stopTouch = true;
                    }
                    else if (distance.x > swipeRange && isGroundedRight && !isMovingRight)
                    {
                        Debug.Log("right");
                        isMovingRight = true;
                        stopTouch = true;
                    }
                    else if (distance.y > swipeRange && isGroundedUp && !isMovingUp)
                    {
                        Debug.Log("up");
                        isMovingUp = true;
                        stopTouch = true;
                    }
                    else if (distance.y < -swipeRange && isGroundedDown && !isMovingDown)
                    {
                        Debug.Log("down");
                        isMovingDown = true;
                        stopTouch = true;
                    }
                    else if(Mathf.Abs(distance.x) < tapRange && Mathf.Abs(distance.y) < tapRange)
                    {
                        stopTouch = true;
                        return;
                    }
                }
                stopTouch = false;
            }
        }
    }


    public void Swipe()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouchPosition = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            currentPosition = Input.GetTouch(0).position;
            Vector2 distance = currentPosition - startTouchPosition;

            if (!stopTouch)
            {
                if (distance.x < -swipeRange && isGroundedLeft && !isMovingLeft)
                {
                    Debug.Log("left");
                    isMovingLeft = true;
                    stopTouch = true;
                }
                else if (distance.x > swipeRange && isGroundedRight && !isMovingRight)
                {
                    Debug.Log("right");
                    isMovingRight = true;
                    stopTouch = true;
                }
                else if (distance.y > swipeRange && isGroundedUp && !isMovingUp)
                {
                    Debug.Log("up");
                    isMovingUp = true;
                    stopTouch = true;
                }
                else if (distance.y < -swipeRange && isGroundedDown && !isMovingDown)
                {
                    Debug.Log("down");
                    isMovingDown = true;
                    stopTouch = true;
                }
                else if (Mathf.Abs(distance.x) < tapRange && Mathf.Abs(distance.y) < tapRange)
                {
                    stopTouch = true;
                    return;
                }
            }
            stopTouch = false;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            stopTouch = false;
        }
    }

    #region Collision
    public virtual bool IsGroundDetectedUP() => Physics.Raycast(groundCheckUp.position,
    Vector2.down, groundCheckDistanceUp, WhatIsGround);

    public virtual bool IsGroundDetectedDown() => Physics.Raycast(groundCheckDown.position,
    Vector2.down, groundCheckDistanceDown, WhatIsGround);


    public virtual bool IsGroundDetectedLeft() => Physics.Raycast(groundCheckLeft.position,
    Vector2.down, groundCheckDistanceLeft, WhatIsGround);

    public virtual bool IsGroundDetectedRight() => Physics.Raycast(groundCheckRight.position,
    Vector2.down, groundCheckDistanceRight, WhatIsGround);


    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheckUp.position, new Vector3(groundCheckUp.position.x,
            groundCheckUp.position.y - groundCheckDistanceUp, groundCheckUp.position.z));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheckDown.position, new Vector3(groundCheckDown.position.x,
            groundCheckDown.position.y - groundCheckDistanceDown, groundCheckDown.position.z));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheckLeft.position, new Vector3(groundCheckLeft.position.x,
            groundCheckLeft.position.y - groundCheckDistanceLeft, groundCheckLeft.position.z));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheckRight.position, new Vector3(groundCheckRight.position.x,
            groundCheckRight.position.y - groundCheckDistanceRight, groundCheckRight.position.z));
    }
    #endregion




    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Brick"))
        {
            Animator anim = player.GetComponentInChildren<Animator>();
            collider.transform.SetParent(transform);

            listBricks.Add(collider.gameObject);
            
            float brickHeight = 0.4f; 
            collider.transform.localPosition = new Vector3(0, -(listBricks.Count * brickHeight), 0);


            float heightIncrease = 0.4f; 
            Vector3 currentPosition = transform.position;
            transform.position = new Vector3(currentPosition.x, currentPosition.y + heightIncrease, currentPosition.z);
            anim.SetTrigger("Nhay");
        }
        if (collider.gameObject.CompareTag("AnBrick"))
        {
            Animator anim = player.GetComponentInChildren<Animator>();

            if (listBricks.Count > 0)
            {
                
                GameObject lastBrick = listBricks[listBricks.Count - 1];
                listBricks.RemoveAt(listBricks.Count - 1);
                Destroy(lastBrick);
                /*Đoạn mã này lấy viên gạch cuối cùng trong danh sách listBricks và sau đó xóa nó 
                khỏi danh sách. Cuối cùng, nó gọi Destroy(lastBrick) để hủy đối tượng viên gạch khỏi game.*/

                float heightDecrease = 0.4f;
                Vector3 currentPosition = transform.position;
                transform.position = new Vector3(currentPosition.x, currentPosition.y - heightDecrease, currentPosition.z);

                anim.SetTrigger("Nhay");
            }
            else
            {
                Debug.Log("dfs");
                isMoving = false;
            }
        }
        if (collider.gameObject.CompareTag("Xoahet"))
        {
            int diem = listBricks.Count;

            Debug.Log("xoa");
            listBricks.Clear();
            Transform[] allChildren = GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                if (child.gameObject.name == "dimian")
                {
                    Destroy(child.gameObject); 
                }
            }

            Vector3 contactPoint = collider.transform.position;
            Vector3 newPosition = new Vector3(contactPoint.x, contactPoint.y , contactPoint.z + 0.5f);
            transform.position = newPosition;

            uiChoi.Find();
            uiChoi.diem.text = "Gạch: " + diem.ToString();
            
        }
        if (collider.gameObject.CompareTag("Find"))
        {
            Animator anim = player.GetComponentInChildren<Animator>();
            anim.SetTrigger("Find");
        }
    }





    /*    private enum DraggedDirection
        {
            Up,
            Down,
            Right,
            Left
        }
        private DraggedDirection GetDragDirection(Vector3 dragVector)
        {
            float positiveX = Mathf.Abs(dragVector.x);
            float positiveY = Mathf.Abs(dragVector.y);
            DraggedDirection draggedDir;
            if (positiveX > positiveY)
            {
                draggedDir = (dragVector.x > 0) ? DraggedDirection.Right : DraggedDirection.Left;
            }
            else
            {
                draggedDir = (dragVector.y > 0) ? DraggedDirection.Up : DraggedDirection.Down;
            }
            Debug.Log(draggedDir);
            return draggedDir;
        }*/

}
