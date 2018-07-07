sleep 4
cd ..
del %1
copy Update\%1 %1
rd /S /Q Update
rm update.bat
