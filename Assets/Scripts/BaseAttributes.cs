using UnityEngine;

public class BaseAttributes : MonoBehaviour
{
    public enum ObjectMaterial
    {
        Blank,
        Brick,
        Wood,
        Steel,
        Concrete
    }

    private ObjectMaterial objectMaterial;
    private float length;
    private float width;
    private bool hasSolar;
    private bool doorActive;
    private bool roofActive;
    private float co2;
    private float cost;

    [SerializeField] private GameObject door;
    [SerializeField] private GameObject top;
    [SerializeField] private GameObject roof;
    [SerializeField] private GameObject solarPanels;

    public ObjectMaterial GetObjectMaterial()
    {
        return objectMaterial;
    }

    public void SetObjectMaterial(ObjectMaterial objectMaterial)
    {
        this.objectMaterial = objectMaterial;
    }

    public void SetLength(float value)
    {
        length = value;
    }

    public void SetWidth(float value)
    {
        width = value;
    }

    public float GetLength()
    {
        return length;
    }

    public float GetWidth()
    {
        return width;
    }

    public void SetSolar(bool value)
    {
        hasSolar = value;
    }

    public bool GetSolar()
    {
        return hasSolar;
    }

    public float GetCO2()
    {
        return co2;
    }

    public float GetCost()
    {
        return cost;
    }

    public void AddCO2(float value)
    {
        this.co2 += value;
    }

    public void AddCost(float value)
    {
        this.cost += value;
    }

    public void SetCO2(float value)
    {
        this.co2 = value;
    }

    public void SetCost(float value)
    {
        this.cost = value;
    }

    public GameObject GetDoor()
    {
        return door;
    }

    public GameObject GetTop()
    {
        return top;
    }

    public bool GetDoorActive()
    {
        return doorActive;
    }

    public void SetDoorActive(bool value)
    {
        doorActive = value;
    }

    public bool GetRoofActive()
    {
        return roofActive;
    }

    public void SetRoofActive(bool value)
    {
        roofActive = value;
    }

    public GameObject GetRoof()
    {
        return roof;
    }

    public GameObject GetSolarPanels()
    {
        return solarPanels;
    }
}
