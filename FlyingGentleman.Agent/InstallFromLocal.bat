ECHO Starting Local Installation
sc stop "Flying Gentleman Agent"
MD C:\Services\FlyingGentleman\Agent\
COPY C:\Projects\FlyingGentleman\FlyingGentleman.Agent\bin\debug\ C:\Services\FlyingGentleman\Agent\
cd C:\Services\FlyingGentleman\Agent\

REM Install Requirements
MSIEXEC /quiet /passive /i SQLSysClrTypes_x64.msi
MSIEXEC /quiet /passive /i SQLSysClrTypes_x86.msi
MSIEXEC /quiet /passive /i SharedManagementObjects_x64.msi
MSIEXEC /quiet /passive /i SharedManagementObjects_x86.msi

REM Install the Gentleman.
C:\Windows\Microsoft.Net\Framework\v4.0.30319\InstallUtil.exe /i FlyingGentleman.Agent.exe
REM Ensure that the correct configuration is used.
del FlyingGentleman.Agent.exe.config
ren Service.Config FlyingGentleman.Agent.exe.config
REM Start the Service.
sc start "Flying Gentleman Agent"
pause