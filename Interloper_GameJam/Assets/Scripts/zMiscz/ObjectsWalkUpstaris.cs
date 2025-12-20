using UnityEngine;

public class ObjectsWalkUpstaris : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.transform.position += new Vector3(0,1,0);
    }
}
