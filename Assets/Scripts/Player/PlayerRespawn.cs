using UnityEngine;
using UnityEngine.Events;

public class PlayerRespawn : MonoBehaviour
{
    private Transform currentCheckpoint; // To store the player's last checkpoint

	public void Respawn()
    {
        transform.position = currentCheckpoint.position; // Move player to checkpoint position
        // Reset player animation
    }

	// Activate checkpoints
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.tag == "Checkpoint")
		{
			currentCheckpoint = collision.transform; // Store the checkpoint that we activated as the current one
			//collision.GetComponent<Collider2D>().enabled = false; //Deactivate checkpoint collider
		}
	}
}
