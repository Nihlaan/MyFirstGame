using UnityEngine;

public class BuildManager : MonoBehaviour
{
    private TurretBlueprint turretToBuild;

    public static BuildManager instance;

    public GameObject standardTurretPrefab;
    public GameObject missileLauncherPrefab;

    public bool CanBuild => turretToBuild != null;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("The BuildManager is already existing.");
            return;
        }

        instance = this;
    }

    public void BuildTurretOn(Node node)
	{
        if (PlayerStats.Money < turretToBuild.cost)
		{
            Debug.Log("Not enough money to build that turret.");
            return;
		}

        PlayerStats.Money -= turretToBuild.cost;

        GameObject turret = Instantiate(turretToBuild.prefab, node.GetBuildPosition(), Quaternion.identity);
        node.turret = turret;

        Debug.Log($"Turret build! Money left: {PlayerStats.Money}.");
    }

    public void SelectTurretToBuild(TurretBlueprint turret)
	{
        turretToBuild = turret;
	}
}