REM copy file to GreenShot folder
@echo off
cls

set GreenShotPath=%1
set PlugInPath="\Plugins\GreenshotDropboxPlugin"

MD %GreenShotPath%%PlugInPath%
copy "bin\Release\GreenshotDropboxPlugin.gsp" %GreenShotPath%%PlugInPath%\GreenshotDropboxPlugin.gsp

MD %GreenShotPath%\Languages\%PlugInPath%
copy "Languages\*.*" %GreenShotPath%\Languages\%PlugInPath%