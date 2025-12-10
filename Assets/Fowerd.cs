using UnityEditor;
using UnityEngine;

public class Fowerd : MonoBehaviour
{
    [SerializeField] private SphereCollider searchArea = default;
    [SerializeField] private float serchAngle = 45f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0.0f, -serchAngle, 0f) * transform.forward, serchAngle * 2f, searchArea.radius);
    }
#endif
}
