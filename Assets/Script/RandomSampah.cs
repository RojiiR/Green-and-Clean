using UnityEngine;

public class RandomSampah : MonoBehaviour
{
    void Start()
    {
        // Ambil semua child object dari SampahAnorganik
        int childCount = transform.childCount;

        // Pilih satu angka acak dari 0 sampai jumlah anak - 1
        int randomIndex = Random.Range(0, childCount);

        // Loop semua anak dan matikan semua, kecuali yang randomIndex
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.gameObject.SetActive(i == randomIndex);
        }
    }
}
