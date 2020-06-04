@echo off
set file=KnoledgeBase\test4.txt
echo FC
bin\Release\iengine FC %file%
echo BC
bin\Release\iengine BC %file%
echo TT
bin\Release\iengine TT %file%
PAUSE