#! /bin/sh

### BEGIN INIT INFO
# Provides: Hamtor
# Required-Start: $network
# Required-Stop:
# Should-Start:
# Default-Start: 2 3 4 5
# Default-Stop: 0 1 6
# Short-Description:
# Description:
### END INIT INFO

program=/home/pi/bin/Hamtor/Hamtor.exe
pid=/var/run/Hamtor.pid
command=usr/bin/mono

start() {
	echo -n "Starting $program: "
	if [ -f $pid ]
	then
		echo "$program already running"
		exit 1
	fi
	start-stop-daemon --start --pidfile $pid --make-pidfile --background \
		--exec $command -- $program
	if [ $? -eq 0 ]
	then
		echo "Success"
		exit 0
	else
	    echo "Fail"
		exit 1
	fi
}

stop() {
	echo -n "Stopping $program: "
	if [ ! -f $pid ]
	then
		echo "not running"
		exit 1
	fi
	kill `cat $pid`
	if [ $? -eq 0 ]
	then
		rm $pid
		echo "Success"
		exit 0
	else
		echo "Fail"
		exit 1
	fi
}

 status() {
	if [ -f $pid ]
	then
		echo "$0 PID:`cat $pid`"
	fi
}

case "$1" in
	start) start ;;
	stop) stop ;;
	status) status ;;
esac
		