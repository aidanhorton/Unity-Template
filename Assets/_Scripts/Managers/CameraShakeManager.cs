using System.Collections;
using UnityEngine;

public class CameraShakeManager : Singleton<CameraShakeManager>
{
    private Vector3 startingLocation;

    private void Start() => this.startingLocation = transform.localPosition;

    public void Shake(float duration, float magnitude) => this.StartCoroutine(this.ShakeRoutine(duration, magnitude));

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        var elapsed = 0.0f;
        
        while (elapsed < duration)
        {
            while (Time.timeScale == 0) 
            {
                yield return null;
            }

            var x = Random.Range(-1f, 1f) * magnitude;
            var y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(this.startingLocation.x + x, this.startingLocation.y + y, this.startingLocation.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = this.startingLocation;
    }
}