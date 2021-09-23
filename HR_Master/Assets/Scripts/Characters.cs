using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters : Specialities
{
    public readonly List<(string, string, int)> character = new List<(string, string, int)>();

    public Characters()
    {
        var minimumValueOfSalaryRange = salaryRangeAndExperience[_randomIndexOfExperienceAndSalaryList].Item2;
        var maximumValueOfSalaryRange = salaryRangeAndExperience[_randomIndexOfExperienceAndSalaryList].Item3;

        character.Add((
            department[_randomIndexOfDepartmentList],
            salaryRangeAndExperience[_randomIndexOfExperienceAndSalaryList].Item1,
            Random.Range(minimumValueOfSalaryRange, maximumValueOfSalaryRange)
            )); // And now, we have a character with randomized department, experience and salary.
    }
}
