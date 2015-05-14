sc stop "Flying Gentleman Agent"
MD C:\Services\FlyingGentleman\Agent\
COPY \\linguine-build\F$\Applications\FlyingGentleman\LatestBuild\Agent\ C:\Services\FlyingGentleman\Agent\ /Y
cd C:\Services\FlyingGentleman\Agent\

REM Ensure that the correct configuration is used.
del FlyingGentleman.Agent.exe.config
ren Service.Config FlyingGentleman.Agent.exe.config

REM Start the Service.
sc start "Flying Gentleman Agent"