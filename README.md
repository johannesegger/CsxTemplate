# CsxTemplate
Template-engine that uses C# scripts to generate .cs files.

## Usage

* Create a template file: Right-click on a project -> Add -> New item -> Choose a name. Typically C# script files end with ".csx"
* Set template properties: Right-click on template file -> Properties -> Build Action: None, Custom Tool: CsxTemplateRunner
* Edit template file (see example template file)
* Visual Studio triggers a rebuild of the template whenever the template changes. You can trigger a rebuild manually by right-clicking on the template file and selecting "Run Custom Tool"

## Example template file
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
        .Select(p => $"Person {p.Age}")) 
    } 
}}"
```
