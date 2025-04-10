using System.Collections;
using UnityEngine;

public class CurvedProjectile : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float heightY = 3f;
    private void Start()
    {
        Vector3 playerPos = Player2D.Instance.transform.position;

        StartCoroutine(ProjectileCurveRoutine(transform.position, playerPos));
    }

    private IEnumerator ProjectileCurveRoutine(Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0f;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearTime = timePassed / duration;
            float heightTime = animCurve.Evaluate(linearTime);
            float height = Mathf.Lerp(0f, heightY, heightTime);

            transform.position = Vector2.Lerp(startPosition, endPosition, linearTime) + new Vector2(0f, height);

            yield return null;
        }

        //Instantiate(splatterPrefab, transform.position, Quaternion.identity); // explosion
        Destroy(gameObject);
    }
}
