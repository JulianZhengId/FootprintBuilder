using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageRecognition : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;

    private void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var e in eventArgs.added)
        {
            GameData.gameData.SpawnedObjects.Add(e.gameObject);
            var length = e.size.x;
            var width = e.size.y;
            var buildingBase = e.gameObject.transform.Find("Base");
            buildingBase.localScale = new Vector3(length, 0.05f, width);
            var child = buildingBase.Find("Body");
            var baseAttributes = child.GetComponent<BaseAttributes>();
            baseAttributes.SetLength(length * 50f);
            baseAttributes.SetWidth(width * 50f);
            baseAttributes.SetObjectMaterial(BaseAttributes.ObjectMaterial.Wood);
        }

        foreach (var e in eventArgs.updated)
        {
            if (e.trackingState == TrackingState.Limited)
            {
                e.gameObject.SetActive(false);

            }
            else if (e.trackingState == TrackingState.Tracking)
            {
                e.gameObject.SetActive(true);
            } 
            else
            {
                e.gameObject.SetActive(false);
            }
        }
    }
}

