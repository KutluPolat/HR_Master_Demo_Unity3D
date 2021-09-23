using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expectations : Specialities
{
    /* param1: Department
     * param2: Experience level
     * param3: The maximum salary that is spared for this employee. */

    public List<(string, string, int)> expectations = new List<(string, string, int)>();

    public Expectations()
    {
        expectations.Add((
            department[_randomIndexOfDepartmentList], 
            salaryRangeAndExperience[_randomIndexOfExperienceAndSalaryList].Item1, 
            salaryRangeAndExperience[_randomIndexOfExperienceAndSalaryList].Item3 - (200 * _randomIndexOfExperienceAndSalaryList) 
            ));

        // expectations.Item3 = Maximum value of salary range of the related experience level - (200 * index)

        // With this equation, The salary given by the company will be below the maximum salary that the employee can ask for.
        // Therefore, if the employee wants the job, he/she should request a salary close to the minimum limit of the salary range determined according to his/her experience.
    }
}
