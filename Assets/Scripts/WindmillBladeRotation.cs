using UnityEngine;

public class WindmillBladeRotation : MonoBehaviour
{
    [SerializeField] float spinningSpeed = 1f;

    void Start()
    {
        
    }

    void Update()
    {
        RotateBlades();
    }

    void RotateBlades()
    {
        GetComponent<Transform>().Rotate(0, 0, spinningSpeed * Time.deltaTime);
    }
}
