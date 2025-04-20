using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject currencyPrefab;
    [SerializeField] private GameObject powerUpPrefab;
    [SerializeField] private int amountDropped;

    public void DropItems()
    {
        if(powerUpPrefab != null)
        {
            Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        }

        for(int i = 0; i < amountDropped; i++)
        {
            Instantiate(currencyPrefab, transform.position, Quaternion.identity);
        }
    }
}
