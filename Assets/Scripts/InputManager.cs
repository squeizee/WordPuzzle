using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Action<Vector3> UserButtonDown;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UserButtonDown?.Invoke(Input.mousePosition);
        }
    }
    
    public bool GetGameObjectUnderMouse(LayerMask layerMask, out GameObject underMouse, out RaycastHit hit)
    {
        if (Camera.main != null)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                underMouse = hit.transform.gameObject;
                return true;
            }
        }

        hit = new RaycastHit();
        underMouse = null;
        return false;
    }
}