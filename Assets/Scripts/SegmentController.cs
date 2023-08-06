using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SegmentController : MonoBehaviour
{
    Segment segmentInHand;
    [SerializeField] LayerMask segmentLayer, portionLayer;
    [SerializeField] Transform cursor;
    public bool disabled = false;


    private void Update()
    {
        if(disabled)
        {
            return;
        }
        cursor.position = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        if (Input.GetMouseButtonDown(0))
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            if (segmentInHand == null)
            {
                PickUpSegment();
            }

            else
            {
                PlacePortin();
            }
        }

        if(segmentInHand != null)
        {
            ToggleUIButtonInteractivity(false);
            RotateSegmentInHand();
        }
        else
        {
            ToggleUIButtonInteractivity(true);
        }
    }

    private void PlacePortin()
    {
        RaycastHit2D ray = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, 10000f, portionLayer);
        if (ray.collider != null && ray.collider.GetComponent<Portion>())
        {
            var portionClicked = ray.collider.GetComponent<Portion>();

            if (portionClicked.transform.childCount > 0)
            {
                SwapWithPopulatedPortion(portionClicked);
            }
            else
            {
                AttachToPortion(portionClicked);
            }
        }
        else
        {
            ReturnToHotbar();
        }
    }

    private void PickUpSegment()
    {
        RaycastHit2D ray = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, 10000f, segmentLayer);
        if (ray.collider != null && ray.collider.GetComponent<Segment>())
        {
            FindObjectOfType<SFXManager>().PlayAudioEvent("event:/SFX/Segment/Segment_Pickup");
            segmentInHand = ray.collider.GetComponent<Segment>();
            AssignSegmentInHandToTransform(cursor);
        }
    }

    private void SwapWithPopulatedPortion(Portion portionClicked)
    {
        FindObjectOfType<SFXManager>().PlayAudioEvent("event:/SFX/Segment/Segment_Swap");
        var segmentToSwapOut = portionClicked.transform.GetChild(0).GetComponent<Segment>();
        AssignSegmentInHandToTransform(portionClicked.transform);
        segmentInHand = segmentToSwapOut;
        AssignSegmentInHandToTransform(cursor);
    }

    private void AttachToPortion(Portion portionClicked)
    {
        FindObjectOfType<SFXManager>().PlayAudioEvent("event:/SFX/Segment/Segment_Place");
        AssignSegmentInHandToTransform(portionClicked.transform);
        segmentInHand = null;
    }

    private void ReturnToHotbar()
    {
        FindObjectOfType<SFXManager>().PlayAudioEvent("event:/SFX/Segment/Segment_Reset");
        AssignSegmentInHandToTransform(segmentInHand.originalParent);
        segmentInHand = null;
    }

    private void ToggleUIButtonInteractivity(bool val)
    {
        foreach(Button button in FindObjectsOfType<Button>())
        {
            button.interactable = val;
        }
    }

    private void RotateSegmentInHand()
    {
        Portion portionHoveredOver;
        RaycastHit2D ray = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, 10000f, portionLayer);
        if (ray.collider != null)
        {
            if (ray.collider.GetComponent<Portion>())
            {
                portionHoveredOver = ray.collider.GetComponent<Portion>();
                segmentInHand.transform.eulerAngles = portionHoveredOver.transform.eulerAngles;
            }
        }
        else
        {
            portionHoveredOver = null;
        }
    }

    private void AssignSegmentInHandToTransform(Transform transformParent)
    {
        if(transformParent == cursor) { segmentInHand.UpdateSpriteLayer("HeldSegment"); }
        else { segmentInHand.UpdateSpriteLayer("PlacedSegment"); }

        segmentInHand.transform.parent = transformParent;
        segmentInHand.transform.localPosition = Vector3.zero;
        segmentInHand.transform.localEulerAngles = Vector3.zero;
        segmentInHand.CheckParent();
    }
}
