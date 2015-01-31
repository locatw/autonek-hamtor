#! /usr/bin/env python

import httplib
import json
import os
import time
import urllib
import wiringpi2

class Config(object):
    def __init__(self, filepath):
        self._filepath = filepath

        self.light_switch_server = None

        self._load()

    def _load(self):
        f = open(self._filepath, 'r')
        config = json.load(f)

        self.light_switch_server = {
                'address': config['light-switch-server']['address'],
                'port': config['light-switch-server']['port']
            }

class PIRSensor(object):
    def __init__(self, gpio, gpio_pin):
        self._gpio = gpio
        self._gpio_pin = gpio_pin

    def detected(self):
        return self._gpio.digitalRead(self._gpio_pin) == self._gpio.HIGH

class HumanDetector(object):
    def __init__(self, config, pir_sensor):
        self._config = config
        self._pir_sensor = pir_sensor

    def run(self):
        prev_detected = False

        while True:
            try:
                detected = self._pir_sensor.detected()

                if detected and prev_detected == False:
                    self._operate_light_switch(detected)

                prev_detected = detected
                time.sleep(1)
            except Exception, e:
                # ignore the error when cannot connect to a server.
                # it's preferable to log this error.
                pass

    def _operate_light_switch(self, detected):
        state = 'on' if detected else 'off'

        body = urllib.urlencode({'state': state})
        headers = {'Content-type': 'application/x-www-form-urlencoded',
                   'Accept': 'text/plain'}

        conn = httplib.HTTPConnection(
                self._config.light_switch_server['address'],
                self._config.light_switch_server['port'],
                timeout = 3)
        conn.request('POST', '/switch', body, headers)

gpio = wiringpi2.GPIO(wiringpi2.GPIO.WPI_MODE_GPIO)    
gpio.pinMode(17, gpio.INPUT)

pir_sensor= PIRSensor(gpio, 17)

config_path = os.path.join(os.path.dirname(__file__), 'hamtor.config')
config = Config(config_path)
human_detector = HumanDetector(config, pir_sensor)

if __name__ == '__main__':
    human_detector.run()
