using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject currencyPrefab;
    [SerializeField] private int amountDropped;

    public void DropItems()
    {
        for(int i = 0; i < amountDropped; i++)
        {
            Instantiate(currencyPrefab, transform.position, Quaternion.identity);
        }
    }
}
