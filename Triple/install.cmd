@echo off
echo Creating shortcut in your startup folder ...
powershell "$s=(New-Object -COM WScript.Shell).CreateShortcut('%userprofile%\Start Menu\Programs\Startup\Triple.lnk');$s.TargetPath='%~dp0Triple.exe';$s.Save()"
echo Done.
pause