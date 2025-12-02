using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RenderChange : MonoBehaviour
{
  
    void Start()
    {
       GetComponent<DecalProjector>().material.SetColor("_BaseColor", new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
