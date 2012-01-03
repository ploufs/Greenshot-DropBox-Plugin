REM copy file to GreenShot folder
@echo off
cls

set GreenShotPath=%1
set PlugInPath="\Plugins\GreenshotDropBoxPlugin"

MD %GreenShotPath%%PlugInPath%
copy "bin\Release\GreenshotDropBoxPlugin.gsp" %GreenShotPath%%PlugInPath%\GreenshotDropBoxPlugin.gsp

MD %GreenShotPath%\Languages\%PlugInPath%
copy "Languages\*.*" %GreenShotPath%\Languages\%PlugInPath%