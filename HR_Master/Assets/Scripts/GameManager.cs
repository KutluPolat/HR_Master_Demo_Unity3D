using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject characterModel;
    public Transform characterHolder;
    private List<GameObject> _spawnedCharacters = new List<GameObject>();
    private readonly Vector3 _position_FrontOfChair = new Vector3(0, 0, -2.3f); //x = xAxisOfCharacters, y = yAxisOfCharacters, z = zAxisOfChair

    private void Start()
    {
        SpawnCharacters();
        
    }

    private void Update()
    {
        MoveNextApplicantToChair();
    }

    private void SpawnCharacters()
    {
        var characterRotation = Quaternion.Euler(0, 180, 0);

        for (int i = 0; i < 10; i++) 
        {
            var spawnPosition = new Vector3(0, 0, i);
            _spawnedCharacters.Add(Instantiate(characterModel, spawnPosition, characterRotation, characterHolder));
        }
    }

    private void MoveNextApplicantToChair()
    {
        _spawnedCharacters[0].transform.position = Vector3.Lerp(_spawnedCharacters[0].transform.position, _position_FrontOfChair, 0.1f);
    }

    private int ReturnListCount()
    {
        var numbersOfElementsInTheList = 0;

        for(int i=0; i<_spawnedCharacters.Count; i++)
            if (_spawnedCharacters[i] != null)
                numbersOfElementsInTheList++;

        return numbersOfElementsInTheList;
    }
}
