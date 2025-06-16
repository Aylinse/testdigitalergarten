using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class DraggableFood : MonoBehaviour
{
    private Camera cam;
    private Vector3 offset;
    private bool dragging = false;
    private Vector3 startPos;

    public bool hasGrain;                // Set in inspector or code
    public string currentBasketTag = ""; // Set when dropped

    private bool placedInBasket = false; // Once placed, can't drag again

    void Start()
    {
        cam = Camera.main;
        startPos = transform.position;
    }

    void Update()
    {
        if (placedInBasket) return; // No dragging after placed

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

    bool IsTouchingThisObject(Vector3 touchPos)
    {
        Collider2D hit = Physics2D.OverlapPoint(touchPos);
        return hit != null && hit.gameObject == gameObject;
    }

    void DetectBasketDrop(Vector3 dropPos)
    {
        Collider2D hit = Physics2D.OverlapPoint(dropPos);

        if (hit != null && (hit.CompareTag("CorrectBasket") || hit.CompareTag("WrongBasket")))
        {
            currentBasketTag = hit.tag;
            placedInBasket = true;
            // Snap to basket position
            transform.position = hit.transform.position;
            return;
        }

        // Not dropped on basket, return to start pos
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
