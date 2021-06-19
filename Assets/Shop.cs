using UnityEngine;

public class Shop : MonoBehaviour
{
    BuildManager buildManager;

    // Start is called before the first frame update
    void Start()
    {
        buildManager = BuildManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PurchaseStandardTurret()
	{
        Debug.Log("Purchase Standard Turret");
        buildManager.SetTurretToBuild(buildManager.standardTurretPrefab);
	}

    public void PurchaseAnotherTurret()
    {
        Debug.Log("Purchase Another Turret");
        buildManager.SetTurretToBuild(buildManager.anotherTurretPrefab);
    }
}
