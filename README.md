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

var powers = Enumerable.Range(0, 5);

$@"// Generation time: {DateTime.Now}

namespace {Namespace}
{{
    public static class QuickMath
    {{
        {
            string.Join
            (
                Environment.NewLine,
                powers.Select(i =>
                    $@"public static double TwoToThePowerOf{i}() {{
                        return {Math.Pow(2, i)};
                    }}"
                )
            )
        }
    }}
}}"
```
### Result (formatted - to not hurt your eyes)
```
// Generation time: 4/18/2016 9:38:09 PM

namespace CsxTemplateTest
{
    public static class QuickMath
    {
        public static double TwoToThePowerOf0()
        {
            return 1;
        }
        public static double TwoToThePowerOf1()
        {
            return 2;
        }
        public static double TwoToThePowerOf2()
        {
            return 4;
        }
        public static double TwoToThePowerOf3()
        {
            return 8;
        }
        public static double TwoToThePowerOf4()
        {
            return 16;
        }
    }
}
```
## 
