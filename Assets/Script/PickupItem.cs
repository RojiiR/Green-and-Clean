using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Header("Setup Pickup")]
    public Transform holdPosition;      // Posisi di player saat memegang
    public GameObject pressFText;       // Teks "Tekan F untuk ambil"

    [HideInInspector] public bool isHeld = false;
    private bool isInRange = false;
    private Transform player;
    private Collider col;

    private void Start()
    {
        col = GetComponent<Collider>();
        if (pressFText != null)
            pressFText.SetActive(false);
    }

    private void Update()
    {
        if (isInRange && !isHeld && Input.GetKeyDown(KeyCode.F))
        {
            PickUp();
        }

        // Selalu ikuti posisi hold kalau dipegang
        if (isHeld && holdPosition != null)
        {
            transform.position = holdPosition.position;
            transform.rotation = holdPosition.rotation;
        }
    }

    private void PickUp()
    {
        isHeld = true;
        transform.SetParent(holdPosition);
        col.enabled = false; // Nonaktifkan collider agar tidak nabrak
        if (pressFText != null) pressFText.SetActive(false);
        Debug.Log("Sampah diambil!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            player = other.transform;
            if (pressFText != null) pressFText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
            if (pressFText != null) pressFText.SetActive(false);
        }
    }
}
