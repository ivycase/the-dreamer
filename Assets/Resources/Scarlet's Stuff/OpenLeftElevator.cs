using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject[] doors;
    public GameObject wall;
    public Renderer lightTube;
    public int materialIndex = 0;
    public Material newMaterial;
    public Light pointLight;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (GameObject door in doors)
                if (door != null) door.SetActive(false);

            if (wall != null) wall.SetActive(false);

            if (lightTube != null && newMaterial != null)
            {
                Material[] mats = lightTube.materials;
                mats[materialIndex] = newMaterial;
                lightTube.materials = mats;
            }

            if (pointLight != null) pointLight.enabled = true;
        }
    }
}