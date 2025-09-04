using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.Enumerators
{
    public enum ObjectType
    {
        All_Objects = 0
        ,Calendar_Header = 2
        ,Calendar_Lines = 3
        ,User = 4
        ,Project = 5
        ,Collaborator = 6
        ,ActivityCollaborator = 7
        ,Specialty = 8
        ,ProjectCollaborators = 9
        ,Address = 10
        ,ProjectActivity = 11
        ,ActivityType = 12
        ,Branch = 13
        ,Client = 14
        ,SalesChannel = 15
        ,ProjectStep = 18
        ,Oportunitty = 19
        ,Note = 20
        ,ExpenseType = 21
        ,Profile = 22
        ,UserMessage = 23
        ,Expense = 24
    }

    public enum oProjectStepType
    {
        oOportunitty = 0,
        oProject = 1,
        ProjectStep = 2
    }
}
