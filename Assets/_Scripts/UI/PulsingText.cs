using UnityEngine;

public class PulsingText : MonoBehaviour
{
    public float Rate = 3.0f;
    public float MidScale = 1.0f;
    public float Ratio = 1.05f;

    private Vector3 scale;

    private void Update()
    {
        float scaleComponent = this.MidScale * Mathf.Pow(this.Ratio, Mathf.Sin(Time.time * this.Rate));
        for (int i = 0; i < 3; i++)
        {
            this.scale = scaleComponent * Vector3.one;
            this.transform.localScale = this.scale;
        }
    }
}