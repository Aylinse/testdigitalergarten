using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableFood : MonoBehaviour
{
    private Camera cam;
    private Vector3 offset;
    private bool dragging = false;
    private Vector3 startPos;

    public bool hasGrain;                
    public string currentBasketTag = ""; 
    private bool placedInBasket = false; 

    void Start()
    {
        cam = Camera.main;
        startPos = transform.position;
    }

    void Update()
    {
#if UNITY_EDITOR
        HandleMouseInput();
#else
        HandleTouchInput();
#endif
    }

    void HandleTouchInput()
    {
        if (placedInBasket) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchWorldPos = cam.ScreenToWorldPoint(touch.position);
            touchWorldPos.z = 0;

            if (touch.phase == TouchPhase.Began)
            {
                if (IsTouchingThisObject(touchWorldPos))
                {
                    dragging = true;
                    offset = transform.position - touchWorldPos;
                }
            }
            else if (touch.phase == TouchPhase.Moved && dragging)
            {
                transform.position = touchWorldPos + offset;
            }
            else if (touch.phase == TouchPhase.Ended && dragging)
            {
                dragging = false;
                DetectBasketDrop(touchWorldPos);
            }
        }
    }

    void HandleMouseInput()
    {
        if (placedInBasket) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;

            if (IsTouchingThisObject(mouseWorldPos))
            {
                dragging = true;
                offset = transform.position - mouseWorldPos;
            }
        }
        else if (Input.GetMouseButton(0) && dragging)
        {
            Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;
            transform.position = mouseWorldPos + offset;
        }
        else if (Input.GetMouseButtonUp(0) && dragging)
        {
            dragging = false;
            Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;
            DetectBasketDrop(mouseWorldPos);
        }
    }

    bool IsTouchingThisObject(Vector3 pos)
    {
        Collider2D hit = Physics2D.OverlapPoint(pos);
        return hit != null && hit.gameObject == gameObject;
    }

    void DetectBasketDrop(Vector3 dropPos)
    {
        Collider2D hit = Physics2D.OverlapPoint(dropPos);

        if (hit != null && (hit.CompareTag("CorrectBasket") || hit.CompareTag("WrongBasket")))
        {
            currentBasketTag = hit.tag;
            placedInBasket = true;
            transform.position = hit.transform.position;
            return;
        }

        // Not on basket
        transform.position = startPos;
        currentBasketTag = "";
    }

    public void ResetPosition()
    {
        transform.position = startPos;
        currentBasketTag = "";
        placedInBasket = false;
    }
}
