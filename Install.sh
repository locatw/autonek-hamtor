#! /bin/sh

sudo mv /home/pi/bin/Hamtor/Hamtor.sh /etc/init.d/Hamtor
if [ $? -ne 0 ]
then
	echo "failed to move Hamtor.sh"
	exit 1
fi

cd /etc/init.d/

sudo chmod 755 Hamtor
sudo chown root:root Hamtor
sudo update-rc.d Hamtor defaults 99 1