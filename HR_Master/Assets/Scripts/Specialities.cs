using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Specialities
{
    protected int _randomIndexOfDepartmentList;
    protected int _randomIndexOfExperienceAndSalaryList;

    public Specialities()
    {
        _randomIndexOfDepartmentList = Random.Range(0, department.Count);
        _randomIndexOfExperienceAndSalaryList = Random.Range(0, salaryRangeAndExperience.Count);
    }

    public List<string> department = new List<string>()
    {
        "Art",
        "Develop",
        "Product"
    };

 
    /* param1: Experience level
     * param2: Minimum salary of related experience level
     * param3: Maximum salary of related experience level */
    public List<(string, int, int)> salaryRangeAndExperience = new List<(string, int, int)>() 
    {
        ("Beginner", 2800, 3500),
        ("Junior", 3500, 5000),
        ("Mid", 5000, 8000),
        ("Senior", 8000, 12000),
        ("Lead", 12000, 30000)
    };
}
