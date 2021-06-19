using UnityEngine;

public class BuildManager : MonoBehaviour
{
    private GameObject turretToBuild;

    public static BuildManager instance;

    public GameObject standardTurretPrefab;
    public GameObject anotherTurretPrefab;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("The BuildManager is already existing.");
            return;
        }

        instance = this;
    }

    public GameObject GetTurretToBuild()
    {
        return turretToBuild;
    }

    public void SetTurretToBuild(GameObject turret)
	{
        turretToBuild = turret;
	}
}