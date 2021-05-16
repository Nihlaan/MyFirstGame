using UnityEngine;

public class Node : MonoBehaviour
{
    private Renderer rend;
    private Color startColor;

    private GameObject turret;

    public Color hoverColor;
    public Vector3 positionOffset;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }

    private void OnMouseDown()
    {
        if (turret != null)
        {
            Debug.Log("Can't build there!");
            return;
        }

        GameObject turretToBuild = BuildManager.instance.GetTurretToBuild();
        turret = Instantiate(turretToBuild, transform.position + positionOffset, transform.rotation);
    }

    private void OnMouseEnter()
    {
        rend.material.color = hoverColor;
    }

    private void OnMouseExit()
    {
        rend.material.color = startColor;
    }
}
