using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject characterModel;
    public Transform characterModelHolder;
    public float applicantsMovementSpeed = 1f;

    private GameObject btn_Restart, bckgrnd_of_OurExpectationsText, bckgrnd_of_ApplicantSpecialitiesText;
    private Expectations _expectations;
    private bool _isGameStarted, _isTheInterviewOngoing, _isApplicantAccepted, _isApplicantRejected, _letApplicantMoveOutsideOfTheRoom;
    private List<GameObject> _spawnedCharacters = new List<GameObject>();
    private int _indexOfTheNextApplicant, _correctDesicionCounter, _wrongDesicionCounter;

    private void Start()
    {
        SpawnCharacters();
        bckgrnd_of_OurExpectationsText = GameObject.Find("bckgrnd_OurExpectations");
        bckgrnd_of_OurExpectationsText.SetActive(false);
        bckgrnd_of_ApplicantSpecialitiesText = GameObject.Find("bckgrnd_ApplicantsSpecialities");
        bckgrnd_of_ApplicantSpecialitiesText.SetActive(false);
        btn_Restart = GameObject.Find("RestartButton");
        btn_Restart.SetActive(false);
        applicantsMovementSpeed *= Time.deltaTime;
    }

    private void Update()
    {
        InterviewForUnityEditor();
        InterviewForAndroid();
    }

    private void FixedUpdate()
    {
        MoveApplicantToChair();
        MoveApplicantOutOfTheRoom();
    }

    private void SpawnCharacters()
    {
        var characterRotation = Quaternion.Euler(0, 180, 0);

        var firstApplicantSpawnPosition = new Vector3(0, 0.05f, -1);
        _spawnedCharacters.Add(Instantiate(characterModel, firstApplicantSpawnPosition, characterRotation, characterModelHolder));

        for (int i = 0; i < 9; i++) 
        {
            var otherApplicantsSpawnPosition = new Vector3(0, 0.05f, 3);
            _spawnedCharacters.Add(Instantiate(characterModel, otherApplicantsSpawnPosition, characterRotation, characterModelHolder));
        }
    }

    private void InterviewForUnityEditor()
    {
#if UNITY_EDITOR
        if (_isTheInterviewOngoing)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                _spawnedCharacters[_indexOfTheNextApplicant].GetComponent<Animator>().SetTrigger("accepted");
                bckgrnd_of_OurExpectationsText.SetActive(false); // Close Backgrounds
                bckgrnd_of_ApplicantSpecialitiesText.SetActive(false); // Close Backgroundss
                GameObject.Find("txt_ApplicantsSpecialities").GetComponent<Text>().text = ""; // Delete former applicants specialities.
                Invoke("LetApplicantMoveOutsideOfTheRoom", 3.367f); // 3.367f = Cheer + Clapping animation length
                CompareSpecialities(true);
                SetExpectations(); // If we find what we're looking for, reset expectations.
                _isTheInterviewOngoing = false;
                _isApplicantAccepted = true;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                _spawnedCharacters[_indexOfTheNextApplicant].GetComponent<Animator>().SetTrigger("rejected");
                bckgrnd_of_OurExpectationsText.SetActive(false); // Close Backgrounds
                bckgrnd_of_ApplicantSpecialitiesText.SetActive(false); // Close Backgrounds
                GameObject.Find("txt_ApplicantsSpecialities").GetComponent<Text>().text = ""; // Delete former applicants specialities.
                Invoke("LetApplicantMoveOutsideOfTheRoom", 7.083f); // 7.083f = Standing Up + Defeated animation length
                CompareSpecialities(false);
                _isTheInterviewOngoing = false;
                _isApplicantRejected = true;
            }  
        }
#endif
    }
    private void InterviewForAndroid()
    {
#if PLATFORM_ANDROID
        if (_isTheInterviewOngoing && Input.touchCount > 0)
        {
            if(Input.touches[0].phase == TouchPhase.Moved)
            {
                if (Input.touches[0].deltaPosition.x > 0) // swipe towards right
                {
                    _spawnedCharacters[_indexOfTheNextApplicant].GetComponent<Animator>().SetTrigger("accepted");
                    GameObject.Find("txt_ApplicantsSpecialities").GetComponent<Text>().text = ""; // Delete former applicants specialities.
                    Invoke("LetApplicantMoveOutsideOfTheRoom", 3.367f); // 3.367f = Cheer + Clapping animation length
                    CompareSpecialities(true);
                    SetExpectations(); // If we find what we're looking for, reset expectations.
                    _isTheInterviewOngoing = false;
                    _isApplicantAccepted = true;
                }
                else // swipe towards left
                {
                    _spawnedCharacters[_indexOfTheNextApplicant].GetComponent<Animator>().SetTrigger("rejected");
                    GameObject.Find("txt_ApplicantsSpecialities").GetComponent<Text>().text = ""; // Delete former applicants specialities.
                    Invoke("LetApplicantMoveOutsideOfTheRoom", 7.083f); // 7.083f = Standing Up + Defeated animation length
                    CompareSpecialities(false);
                    _isTheInterviewOngoing = false;
                    _isApplicantRejected = true;
                }
            }
        }
#endif
    }
    public void CompareSpecialities(bool isAccepted)
    {
        
        var characterSpecialities = _spawnedCharacters[_indexOfTheNextApplicant].GetComponent<Character>().thisCharacter.character[0];
        var expectations = _expectations.expectations[0];

        // If this applicant is suitable for what the company is looking for.
        if (characterSpecialities.Item1 == expectations.Item1) // Department
            if (characterSpecialities.Item2 == expectations.Item2) // Experience
                if (characterSpecialities.Item3 <= expectations.Item3) // Salary
                {
                    if (isAccepted)
                    {
                        _correctDesicionCounter++;
                        return;
                    }
                    else
                    {
                        _wrongDesicionCounter++;
                        return;
                    };
                };

        // If this applicant is not suitable for what the company is looking for.
        if (isAccepted)
        {
            _wrongDesicionCounter++;
        }
        else
        {
            _correctDesicionCounter++;
        }
    }
    public void LetApplicantMoveOutsideOfTheRoom() => _letApplicantMoveOutsideOfTheRoom = true;

    private void SetApplicantSpecialitiesText()
    {
        bckgrnd_of_OurExpectationsText.SetActive(true); // Open Backgrounds
        bckgrnd_of_ApplicantSpecialitiesText.SetActive(true); // Open Backgrounds
        GameObject.Find("txt_ApplicantsSpecialities").GetComponent<Text>().text =
                "Here's my CV: \n" +
                "Department: " + _spawnedCharacters[_indexOfTheNextApplicant].GetComponent<Character>().thisCharacter.character[0].Item1 + "\n" +
                "Experience level: " + _spawnedCharacters[_indexOfTheNextApplicant].GetComponent<Character>().thisCharacter.character[0].Item2 + "\n" +
                "I want " + _spawnedCharacters[_indexOfTheNextApplicant].GetComponent<Character>().thisCharacter.character[0].Item3 + " caps.";
    }

    private void MoveApplicantToChair()
    {
        if (_isGameStarted && !_isTheInterviewOngoing)
        {
            if (_isApplicantRejected || _isApplicantAccepted)
                return;

            var nextSpawnedCharactersTransform = _spawnedCharacters[_indexOfTheNextApplicant].transform;

            if (_spawnedCharacters[_indexOfTheNextApplicant].transform.position.z > -2.3f)
            {
                _spawnedCharacters[_indexOfTheNextApplicant].GetComponent<Animator>().SetTrigger("walk");

                _spawnedCharacters[_indexOfTheNextApplicant].transform.position = new Vector3(
                   _spawnedCharacters[_indexOfTheNextApplicant].transform.position.x,
                   _spawnedCharacters[_indexOfTheNextApplicant].transform.position.y,
                   _spawnedCharacters[_indexOfTheNextApplicant].transform.position.z - applicantsMovementSpeed
                   );
            }
            else if(_spawnedCharacters[_indexOfTheNextApplicant].transform.eulerAngles.y > 150)
            {
                _spawnedCharacters[_indexOfTheNextApplicant].GetComponent<Animator>().SetTrigger("sit");
                nextSpawnedCharactersTransform.Rotate(Vector3.up * -50f * Time.deltaTime, Space.Self);
            }
            else
            {
                _isTheInterviewOngoing = true;
                Invoke("StartInterview", 1.2f); // Calling related method after one second.
            }
        }
    }

    private void StartInterview() 
    {
        Debug.Log("Start Interview");
        SetApplicantSpecialitiesText();
    } 

    private void MoveApplicantOutOfTheRoom()
    {
        if (_isApplicantAccepted && _letApplicantMoveOutsideOfTheRoom)
        {
            var nextSpawnedCharactersTransform = _spawnedCharacters[_indexOfTheNextApplicant].transform;

            if (nextSpawnedCharactersTransform.eulerAngles.y > 90)
                nextSpawnedCharactersTransform.Rotate(Vector3.up * -100f * Time.deltaTime, Space.Self);

            if (nextSpawnedCharactersTransform.position.x < 3)
            {
                nextSpawnedCharactersTransform.position = new Vector3(
                   nextSpawnedCharactersTransform.position.x + applicantsMovementSpeed,
                   nextSpawnedCharactersTransform.position.y,
                   nextSpawnedCharactersTransform.position.z
                   );
            }
            else
            {
                SetSpesificBooleansFalse();
                GiveTurnToNextApplicant();
            }
        }
        else if (_isApplicantRejected && _letApplicantMoveOutsideOfTheRoom)
        {
            var nextSpawnedCharactersTransform = _spawnedCharacters[_indexOfTheNextApplicant].transform;

            if (nextSpawnedCharactersTransform.eulerAngles.y < 270)
                nextSpawnedCharactersTransform.Rotate(Vector3.up * 100f * Time.deltaTime, Space.Self);

            if (nextSpawnedCharactersTransform.position.x > -3)
            {
                nextSpawnedCharactersTransform.position = new Vector3(
                   nextSpawnedCharactersTransform.position.x - applicantsMovementSpeed,
                   nextSpawnedCharactersTransform.position.y,
                   nextSpawnedCharactersTransform.position.z
                   );
            }
            else
            {
                SetSpesificBooleansFalse();
                GiveTurnToNextApplicant();
            }
        }
    }

    private int ReturnListCount()
    {
        var numbersOfElementsInTheList = 0;

        for(int i=0; i<_spawnedCharacters.Count; i++)
            if (_spawnedCharacters[i] != null)
                numbersOfElementsInTheList++;

        return numbersOfElementsInTheList;
    }

    private void SetExpectations() => _expectations = new Expectations();
   

    public void GiveTurnToNextApplicant()
    {
        if (_indexOfTheNextApplicant < (_spawnedCharacters.Count - 1))
        {
            Destroy(_spawnedCharacters[_indexOfTheNextApplicant]); // Destroy former applicant
            _indexOfTheNextApplicant++;
            SetSpesificBooleansFalse();
        }
        else
        {
            _isGameStarted = false;
            Destroy(_spawnedCharacters[_indexOfTheNextApplicant]);
            btn_Restart.SetActive(true);
            bckgrnd_of_OurExpectationsText.SetActive(false); // Close Backgrounds
            bckgrnd_of_ApplicantSpecialitiesText.SetActive(false); // Close Backgrounds
            GameObject.Find("txt_GameOver").GetComponent<Text>().text = "Correct desicions: " + _correctDesicionCounter + "\n" + "Wrong desicions: " + _wrongDesicionCounter;
        }
    }

    private void SetSpesificBooleansFalse()
    {
        _isApplicantAccepted = false;
        _isApplicantRejected = false;
        _letApplicantMoveOutsideOfTheRoom = false;
        _isTheInterviewOngoing = false;
    }

    public void Play()
    {
        _isGameStarted = true;
        SetExpectations();
        Destroy(GameObject.Find("PlayButton"));
    }
    public void Restart() => SceneManager.LoadScene("MainScene");
}
