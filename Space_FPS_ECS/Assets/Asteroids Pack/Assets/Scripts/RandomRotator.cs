using UnityEngine;

public class RandomRotator : MonoBehaviour
{
    [SerializeField]
    private float tumble = default;

    void Start()
    {
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
    }
}