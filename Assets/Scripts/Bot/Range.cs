using UnityEditor;
using UnityEngine;

public class Range : MonoBehaviour
{
    [SerializeField] private SphereCollider searchArea = default;
   
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
        Handles.color = new Color(1,0,0,0.1f);
        Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, 360f, 0f) * transform.forward, 360, searchArea.radius);
       /* Handles.color = new Color(0, 1, 0, 0.1f);
        Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, 360f, 0f) * transform.forward, 360, searchArea.radius / 5);*/
    }
#endif
}
