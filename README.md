# LoRa-Flue-Smoke-Monitoring
LoRa Flue Smoke Gas Monitoring Repository contains all file to config a LoRa device, The Things Network and create an Android application.
We have developed and studied a system to track the smoke level in a flue and send data by a LoRa transmitter. This could be useful for very aspects:

private telemetry og boiler
smart city certification of flue (every year in Italy we spend 70 euro to do this check)
scientific scope because the pollution from flue are the 60% of the total pollution
Francesco Cassini - Matteo Ginesi - Lorenzo Leschiera The IOT course is taught by by Ioannis Chatzigiannakis La Sapienza - Rome




GasAlert24
Gas sensor - I.O.T. Exam project - Spring 2019
Contributors:	email
Francesco Cassini	cassini.785771@studenti.uniroma1.it
Lorenzo Leschiera	ginesi.1832198@studenti.uniroma1.it
Matteo Ginesi	leschiera.1325697@studenti.uniroma1.it
dragino LoRaWAN Gateway configuration
If pre-existent data are present, please reset dragino:

Press toggle button while dragino is running for about 30 seconds.
Connect to dragino access point with any device (pc suggested). Open on your browser at link: 10.130.1.1 or via url: dragino-<same code of WiFi Access Point>

Login	
user:	root
pass:	dragino
dragino network settings
Network > Internet Access
Select WiFi connection
Set WiFi password and SSID
Set a page for ping test (like www.google.it)
Save
Network > Access Point
Disable WiFi AP
Save
Unsaved changes
Save & Apply
Wait for about 2 minutes and reboot.
Setting up WLAN connection
Network > Internet Access
Select WLAN connection
Unsaved changes
Save & Apply
Wait for about 2 minutes and reboot.
Setting up TTN data
Open TheThingsNetwork and go to Gateways settings.
Register a new Gateway with legacy packed forwarder
Copy the address on the back of the dragino and add a number like between (00 00) and (FF FF).
Add a description.
Set Frequency Plan
Register your gateway.
Open on your browser dragino IP (based on your network).

Sensor > LoraWAN
Server address: (example) router.eu.thethings.network
Save & Apply
Sensor > IOT Server
IoT Server: LoRaWAN
Save & Apply
Firmware
LoRaWan on Mbed
All the comunication system is based on LoRaWAN stack protocol managed by STM32 microcontroller board DISCO-L072CZ, programmed by Mbed (www.mbed.com). Firmware uses Mbed OS 5 to provide drivers and pre-built code for:

LoRaWAN radio module: SX1276
Analog Input/Output
Digital GPIOs
Debug and tracing
RS232 over USB stack
Code is divided in two main blocks:

System initialization: main.cpp
mbed-os configuration (LoRaWAN login): mbed_app.json
System initialization
In the main.cpp file, there are all the functions/declarations needed to deploy the send data function. The user has to change the ID constant in the file, as a hexadecimal value of 16 bit (uint16_t):

...
    #define ID              0xAAFF
...
Sensor, for the board used in the project, is addressed on pin analog 1 (A1). Change it in case of need. Pin A0 can't be used.

...
    AnalogIn sensor(A1);
...
In the send_data() function the user can set any kind of encoding of transmitted data, writing the correct message into the tx_buffer and settings the correct dimension value of it, into packet_len variable.

    static void send_message()
    {
        uint16_t packet_len;
        int16_t retcode;

        /*
            Reading sensor data (unsigned int 16 bit)
            Writing ID and sensor data into transmission buffer
            Sending data to Gateway by LoRaWAN stack
        */
        uint16_t data = sensor.read_u16();
        packet_len = sprintf(
            (char *) tx_buffer, "%4X%u", ID, data);

        printf("\r\ndata sent: %s\r\n", tx_buffer);
    }
LoRaWAN login configuration
In the mbed_app.json file, each user NEED TO CONFIGURE their own TheTingsNetwork credential to their proper application.

Note:

This code works only for APB protocol. The OOTA way isn't verificated yet. For all information about TheTingsNetwork procedures, please refer to www.thethingsnetwork.com.

In order to change radio module, set the proper value in:

...
    "config": {
        "lora-radio": {
            "help": "Which radio to use (options: SX1272,SX1276)",
            "value": "SX1276"
        },
For TheThingsNetwork credentials, change:

...
    "target_overrides": {
        "*": {

            "lora.appskey": "{C-STYLE HEX App Session Key}",
            "lora.nwkskey": "{C-STYLE HEX Network Session Key}",
            "lora.device-address": "0x<HEX DATA OF device address>"
