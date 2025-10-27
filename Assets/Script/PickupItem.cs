using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Header("Setup Pickup")]
    public Transform holdPosition;
    public GameObject pressFText;

    [Header("Tipe Sampah")]
    public string trashType; // "Organik", "Anorganik", atau "Khusus"

    [HideInInspector] public bool isHeld = false;
    private bool isInRange = false;
    private Transform player;
    private Collider col;

    private void Start()
    {
        col = GetComponent<Collider>();
        if (pressFText != null)
            pressFText.SetActive(false);

        // Auto-isi trashType dari tag
        if (CompareTag("SampahOrganik")) trashType = "Organik";
        else if (CompareTag("SampahAnorganik")) trashType = "Anorganik";
        else if (CompareTag("SampahKhusus")) trashType = "Khusus";
    }

    private void Update()
    {
        if (isInRange && !isHeld && Input.GetKeyDown(KeyCode.F))
        {
            // Cegah ambil dua sampah
            if (PlayerHasTrash()) return;
            PickUp();
        }

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
        col.enabled = false;
        if (pressFText != null) pressFText.SetActive(false);
        Debug.Log($"{trashType} diambil!");
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

    private bool PlayerHasTrash()
    {
        PickupItem[] allTrash = FindObjectsOfType<PickupItem>();
        foreach (var t in allTrash)
        {
            if (t.isHeld) return true;
        }
        return false;
    }
}
