using UnityEngine;

public enum ToolType
{
    Shovel,
    WaterCan
}

public class ToolManager : MonoBehaviour
{
    public ToolType currentTool;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentTool = ToolType.Shovel;
            Debug.Log("Switched to Shovel");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentTool = ToolType.WaterCan;
            Debug.Log("Switched to Water Can");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            UseCurrentTool();
        }
    }

    void UseCurrentTool()
    {
        switch (currentTool)
        {
            case ToolType.Shovel:
                GetComponent<ShovelTool>().UseShovel();
                break;
            case ToolType.WaterCan:
                GetComponent<WaterCanTool>().UseWaterCan();
                break;
        }
    }
}
