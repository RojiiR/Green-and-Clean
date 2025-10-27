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

                // 🔍 Cek apakah tag sampah cocok dengan tempat sampah
                bool benar = false;

                if (currentTrash.CompareTag("SampahOrganik") && gameObject.CompareTag("OrganikBin"))
                    benar = true;
                else if (currentTrash.CompareTag("SampahAnorganik") && gameObject.CompareTag("AnorganikBin"))
                    benar = true;
                else if (currentTrash.CompareTag("SampahKhusus") && gameObject.CompareTag("KhususBin"))
                    benar = true;

                if (benar)
                {
                    Debug.Log("✅ Sampah dibuang di tempat yang benar!");
                    ScoreManager.instance.AddScore(10);
                }
                else
                {
                    Debug.Log("❌ Salah tempat sampah!");
                    ScoreManager.instance.AddScore(-5);
                }

                // Hapus sampah setelah dibuang
                Destroy(currentTrash.gameObject);
                currentTrash = null;
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
