@ECHO OFF
ECHO About to uun Unit Tests
ECHO .
"C:\Program Files (x86)\OpenCover\OpenCover.Console.exe" -filter:+[*]* -target:"packages\NUnit.2.5.10.11092\tools\nunit-console.exe" -register:user -targetargs:"FlyingGentleman.Tests.nunit /config=Debug /framework=v4.0.30319 /noshadow" -output:CodeCoverageReport\coverage.xml
ECHO .
ECHO About to Create Report
ECHO .
.\Dependencies\CodeCoverage\ReportGenerator\ReportGenerator.exe .\CodeCoverageReport\coverage.xml .\CodeCoverageReport\ Html
ECHO .
ECHO Complete!
PAUSE