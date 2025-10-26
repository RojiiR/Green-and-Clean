using UnityEngine;

public class TrashBin : MonoBehaviour
{
    public GameObject pressEText;   // UI teks "Tekan E untuk buang"

    private PickupItem currentTrash; // referensi sampah yang lagi dibawa

    private void Start()
    {
        if (pressEText != null)
            pressEText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Deteksi player
        if (other.CompareTag("Player"))
        {
            // Cek apakah player lagi bawa sampah
            PickupItem heldItem = FindHeldTrash(other.transform);
            if (heldItem != null)
            {
                currentTrash = heldItem;
                if (pressEText != null) pressEText.SetActive(true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && currentTrash != null && currentTrash.isHeld)
        {
            if (pressEText != null && !pressEText.activeSelf)
                pressEText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (pressEText != null) pressEText.SetActive(false);
                Destroy(currentTrash.gameObject);
                currentTrash = null;
                Debug.Log("Sampah dibuang!");

                ScoreManager.instance.AddScore(10);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (pressEText != null) pressEText.SetActive(false);
            currentTrash = null;
        }
    }

    // Cari sampah yang sedang dipegang player
    private PickupItem FindHeldTrash(Transform player)
    {
        PickupItem[] allTrash = FindObjectsOfType<PickupItem>();
        foreach (var t in allTrash)
        {
            if (t.isHeld) return t;
        }
        return null;
    }
}
