using UnityEngine;

public class Coin : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Если в монету врезался объект с тегом "Player"
        if (other.CompareTag("Player"))
        {
            // Находим скрипт UIManager на сцене и добавляем 1 монету
            FindObjectOfType<UIManager>().AddCoin(1);

            // Уничтожаем монету
            Destroy(gameObject);
        }
    }
}