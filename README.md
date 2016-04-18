# CsxTemplate
Template-engine that uses C# scripts to generate .cs files.

## Usage

* Create a template file: Right-click on a project -> Add -> New item -> Choose a name. Typically C# script files end with ".csx"
* Set template properties: Right-click on template file -> Properties -> Build Action: None, Custom Tool: CsxTemplateRunner
* Edit template file (see [example template file](#template))
* Visual Studio triggers a rebuild of the template whenever the template changes. You can trigger a rebuild manually by right-clicking on the template file and selecting "Run Custom Tool"

## Example
### Template
```
#r "System.Core" 
 
using System; 
using System.Collections.Generic; 
using System.Linq; 
 
$@"namespace {Namespace} {{ 
    //It is {DateTime.Now}. 
    //Sum of 1 to 10 is {Enumerable.Range(1, 10).Sum()}. 
    //{string.Join($"{Environment.NewLine} //", Enumerable 
        .Range(0, 10) 
        .Select(i => new { Name = $"Person {i}", Age = i + 20 }) 
        .Select(p => $"{p.Name} is {p.Age} years old")) 
    } 
}}"
```
### Result
```
namespace FixNuGetHintPathTestProject {
    //It is 4/18/2016 12:11:55 PM.
    //Sum of 1 to 10 is 55.
    //Person 0 is 20 years old
 //Person 1 is 21 years old
 //Person 2 is 22 years old
 //Person 3 is 23 years old
 //Person 4 is 24 years old
 //Person 5 is 25 years old
 //Person 6 is 26 years old
 //Person 7 is 27 years old
 //Person 8 is 28 years old
 //Person 9 is 29 years old
}
```
## 
