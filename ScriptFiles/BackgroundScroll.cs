using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float scrollSpeed = 0.008f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - scrollSpeed, transform.position.z);
    }
}
