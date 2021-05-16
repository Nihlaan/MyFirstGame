using UnityEngine;

public class BuildManager : MonoBehaviour
{
    private GameObject turretToBuild;

    public static BuildManager instance;

    public GameObject standardTurretPrefab;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("The BuildManager is already existing.");
            return;
        }

        instance = this;
    }

    private void Start()
    {
        turretToBuild = standardTurretPrefab;
    }

    public GameObject GetTurretToBuild()
    {
        return turretToBuild;
    }
}