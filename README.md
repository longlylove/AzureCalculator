# Testing AzureCalculator

This solutions contains 3 projects for setting up framework to test the following
UI -  https://calculator-web.azurewebsites.net/
API - https://calculator-web.azurewebsites.net/api/calculate

Solution structure
> AzureCalculator.sln (solution)
>> 1. Framework.csproj (project)
>> 2. AzureCalculator.csproj (UI project)
>> 3. AzureCalculatorAPI.csproj (API project)

Requirements for setting up and run
1. Visual Studio or vstest console
2. .NET framework 4.6.1 or above
3. internet connection to Nuget.org

Steps to run via Visual Studio:

***** For UI project *****
1. Restore nuget for the solution and build
2. Using VS test explorer to ensure all the tests are discovered
3. Using VS Test > Test Settings > Select Test Settings File, select the local.runsettings file (in the Settings folder under the project)
4. In the test explorer, right click and run a particular test (ie. AddingNumbers_Add_1_Digit) or run full suite (ie. AzureCalculator.Features)
5. After test run completed, under the project \bin\Debug folder, find the Reports folder
6. Test report should be available under a subfolder (ie. AzureCalculator_UI_20200717_013552) as a .html file (ie. AzureCalculator_UI.html). Any failed test will have an according screen-capture (ie. AddingNumbers_20200717_013641.png) in the same subfolder.
7. A test-report example is also included in the solution foler named TestReportExamples

***** For API project *****
1. Restore nuget for the solution and build
2. Using VS test explorer to ensure all the tests are discovered
3. Under the \bin\Debug folder, create a file named appSettings.config with below content

```xml
<?xml version="1.0" encoding="utf-8"?>
<appSettings>
	<add key="CalcApiKey" value="kBG9dKn6V9dRXa5Uosi4z9Sj7sIKguDR86UZJfuS7oQ3IlGFqQ0krA=="/>
	<add key="GenerateReport" value="True"/>
</appSettings> 
```

4. In the test explorer, right click and run a particular test (ie. AddingNumbers_Add_01) or run full suite (ie. AzureCalculatorAPI.Features)
5. After test run completed, under the project \bin\Debug folder, find the Reports folder
6. Test report should be available under a subfolder (ie. AzureCalculator_API_20200717_012344) as a .html file (ie. AzureCalculator_API.html).
7. A test-report example is also included in the solution foler named TestReportExamples

Any question please feel free to reach out to me :D. Have fun.
