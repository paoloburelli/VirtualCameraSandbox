#!/bin/sh
curl https://github.com/paoloburelli/CamOn/archive/master.zip -L > CamOn-master.zip
unzip CamOn-master
mv CamOn-master/Assets/CamOn Assets/
rm -rf CamOn-master*
