using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{
    // Поместите спавнер вниз, туда где должен заспавниться первый объект
    [SerializeField] private List<GameObject> objectPrefabs;
    [SerializeField] private float spawnInterval = 0f; // Фича для дебага, просто оставьте 0f
    [SerializeField] private int numberOfRows = 5; // Количество рядов
    [SerializeField] private int numberOfColumns = 10; // Количество объектов в каждом ряду
    [SerializeField] private float verticalSpacing = 1f; // Расстояние между объектами по Y
    [SerializeField] private float horizontalSpacing = 1f; // Расстояние между объектами по X

    void Awake()
    {
        StartCoroutine(SpawnObjects());
    }
    

    private IEnumerator SpawnObjects()
    {
        for (int row = 0; row < numberOfRows; row++)
        {
            for (int column = 0; column < numberOfColumns; column++)
            {
                GameObject randomPrefab = objectPrefabs[Random.Range(0, objectPrefabs.Count)];
                Vector3 spawnPosition = transform.position + new Vector3(column * horizontalSpacing, row * verticalSpacing, 0);

                // Создаем экземпляр объекта
                GameObject instance = Instantiate(randomPrefab, spawnPosition, Quaternion.identity);

                // Изменяем имя экземпляра
                instance.name = "Object" + column + "_" + row;

                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }
}
        
    