using UnityEngine;

public class City : MonoBehaviour {
    public GameManager gameManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.gameObject.name == "explosion")
        {
            gameManager.KillCity(gameObject);
        }
    }
}
