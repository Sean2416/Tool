set tmpPath=%1
set projectName=%2

cd /d %tmpPath%
rmdir /Q /S %projectName%
del %tmpPath%\%projectName%.zip