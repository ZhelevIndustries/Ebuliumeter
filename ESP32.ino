#include <ThingSpeak.h>
#include <WiFi.h>
#include <Wire.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_BME280.h>
#include <SPI.h>
#include "Adafruit_MAX31865.h"
#include <OneWire.h>
#include <DallasTemperature.h>

#include "Math.h"
#include <iostream>


#define CHANNEL_ID 1957486
#define API_WRITE_KEY "PDO9G1FIAPWAO7MF"
#define API_READ_KEY "FGGN37H12IG5S307"
#define SEALEVELPRESSURE_HPA (10113.25)

#define Cooler_pin 32
#define Pump_pin   25
#define Heater_pin 26
#define Cooler_pump_pin 33
#define Klapa_pin 27
#define Leveler_pin 13
#define Start_Button_pin 15

Adafruit_BME280 bme; // definirane na bme datchik
WiFiClient client; // syzdavane na wifi klient

String data; // niz izpolzvan za chetene na danni
bool measureing = false; // buleva promenliva indikirashta puskane/spirane na nagrevatel
bool filled = false;
bool wifi_connection = true;
const char* ssid = "viper_z"; // id na dadenata wifi mreja
const char* password = "gugi_pisko"; // parola na dadenata wifi mreja
unsigned long delayTime; // vremeto za chakane mejdu 2 operacii

const int oneWireBus = 4;  // pin na onewire   
OneWire oneWire(oneWireBus); // definirane na onewire
DallasTemperature sensors(&oneWire); // inizializirane na dallastemperature datchika

#define MAXDO 19
#define MAXCS 5
#define MAXCLK 18
#define MAXSDI 23
Adafruit_MAX31865 thermocouple = Adafruit_MAX31865(MAXCS, MAXSDI, MAXDO, MAXCLK); // inizializirane na MAX31865 datchika

void WiFi_Connect()
{
  WiFi.begin(ssid, password); // svyrzane s wifi mreja
  Serial.print("Connecting to WiFi");
  int not_connected = 0;
  while(WiFi.status() != WL_CONNECTED) // cikyl prodyljavasht do ostanovqvane na vryzka s mrejata
  {
      not_connected += 1;
      Serial.print("N");
      delay(500);
      if(not_connected == 10) { break; wifi_connection = true; }
  }

  if(not_connected < 10)
  {
    Serial.println("Connected to WiFi");
    Serial.print("IP: ");
    Serial.println(WiFi.localIP());
  }
}

void setup() 
{
  Serial.begin(9600); // zadavane na baudrate 9600 na serien port 1
  Serial2.begin(9600); // zadavane na baudrate 9600 na serien port 2
  WiFi_Connect(); // svyrzane s wifi mreja
  sensors.begin(); // puskane na dallastemperature datchika
  thermocouple.begin(MAX31865_4WIRE); // puskane na MAX31865 datchika
  if(wifi_connection == true) { ThingSpeak.begin(client); /* ostanovqvane na vryzka s ThingSpeak */ }

  Wire.begin();

  bool status; // definirane na promenliva za status na BME datchika
  status = bme.begin(0x76);  // proverka za statusa na BME datchika
   
  if (!status) // proverka na ostanovenata vryzka s BME datchika
  {
    Serial.println("Could not find a valid BME280 sensor, check wiring!");
    while (1);
  }

  Serial.println("-- Default Test --");
  delayTime = 500;

  Serial.println();

  pinMode(Heater_pin, OUTPUT);
  pinMode(Cooler_pump_pin, OUTPUT);
  pinMode(Cooler_pin, OUTPUT);
  pinMode(Pump_pin, OUTPUT);
  pinMode(Klapa_pin, OUTPUT);
  pinMode(Leveler_pin, INPUT);

  digitalWrite(Heater_pin, LOW);
  digitalWrite(Cooler_pump_pin, LOW);
  digitalWrite(Cooler_pin, LOW);
  digitalWrite(Pump_pin, LOW);
  digitalWrite(Klapa_pin, LOW);
}

bool stationary = false;
bool after_m = false; // promenliva kazvashta dali e sled izmevaniqta na alkoholno sydyrjanie na vinoto
int count_m = 0; // broqch za koe podred broene e
float pressure; // nalqgane
float final_t; // finalna temperatura
float cool_t; // temperatura na ohladitelnata technost
int loop_counter = 0; // sluji kato timer
bool klapa_on = false;
bool pump_on = false;
bool heater_on = false;

void loop() 
{ 
  float ratio = thermocouple.readRTD();
  ratio /= 32768;
  float resis = ratio * 430; // vzimane na syprotivlenieto na datchika
  measureing = false; // zadavane che nqma izmervane
  
  cooler_regulation(); // regulirane na ohladitelq
  digitalWrite(Cooler_pump_pin, HIGH);
  
  if(Serial.available() > 0) // proverqvane dali ima danni na Serien port 1
  {
    data = Serial.readString(); // chetene na dannite ot Serien port 1
    if(data == "ESP") { Serial.print("APARAT"); } // indikator za nalichie na ustroistvo
    if(data == "START") { measureing = true; } // indikator za zapochvane
    if(data == "CLEAN") 
    {
      loop_counter = 0;
      digitalWrite(Klapa_pin, HIGH);
      digitalWrite(Pump_pin, HIGH);
      while(loop_counter < 400)
      {
        delay(1000);
        loop_counter += 1;
      } 
      loop_counter = 0;
      digitalWrite(Klapa_pin, LOW);
      digitalWrite(Pump_pin, LOW);
    }
  }

  if(Serial2.available() > 0) // chetene na dannite ot Serien port 1
  {
    if(Serial2.readString()[2] == 0x05) { measureing = true; } // deistvie pri natisnat buton start
  }

  if(digitalRead(Start_Button_pin) == LOW) { measureing = true; }

  if(measureing) //  proverqvane dali shte se izmerva alkohola
  {
    pressure=bme.readPressure() / 100.0F; // prochitane na nalqganeto v kpa
    Serial.print(pressure); // izprashtene na nalqganeto po serien port 1
    
    printt2(pressure); // printirane na nalqganeto na displeya
    klapa_on = true;
    delay(1000);
  }
  float prev_temp = 0; // predishna temp
  float temp = 0; // segashna temp
  
  while(measureing)
  {
    temp = thermocouple.temperature(100, 430); // prochitane na temperaturata
    Serial.print(temp);
    sensors.requestTemperatures();
    cooler_regulation();
    printt3(temp);
    printt2(pressure);
    printt1(sensors.getTempCByIndex(0));

    if(Serial.available() > 0) // proverqvane dali ima danni na Serien port 1
    {
      data = Serial.readString(); // chetene na dannite ot Serien port 1
      if(data == "STOP") 
      { 
        digitalWrite(Klapa_pin, LOW);
        digitalWrite(Pump_pin, LOW);
        digitalWrite(Heater_pin, LOW);
        break;
      } // indikator za nalichie na ustroistvo
    }

    if(klapa_on == true) { digitalWrite(Klapa_pin, HIGH); }
    if(pump_on == true) { digitalWrite(Pump_pin, HIGH); }
    if(heater_on == true) { digitalWrite(Heater_pin, HIGH); }
    
    delay(1000);
    loop_counter += 1;
      
    if(loop_counter == 10 && klapa_on == true) { loop_counter = 0; digitalWrite(Klapa_pin, LOW); pump_on = true; klapa_on = false; }
    if(digitalRead(Leveler_pin) == LOW /*loop_counter == 15*/ && pump_on == true) { loop_counter = 0; digitalWrite(Pump_pin, LOW); heater_on = true; pump_on = false; filled = true; digitalWrite(Heater_pin, HIGH); }
    
    if(heater_on == true && temp < prev_temp + 0.1 && temp > prev_temp - 0.1) // proverqva dali segashnata temperatura e v nqkakvi granici s predishnata temp
    {
      count_m++;
    }
    else
    {
      prev_temp = temp;
      count_m = 0;
    }

    if(count_m == 20) // proveravq dali 20 izmervaniq podred e poddyrjava priblizitelno ednakva temperatura
    {
      final_t = temp; // finalna temperatura
      after_m = true; // puskane na istina sled izmervaneto
      measureing = false; // veche nqma da izmerva
      Serial.print("Done"); // izprashtane na serien port 1 che izmervaneto e proklyuchilo
      digitalWrite(Heater_pin, LOW); // izklyuchvane na nagrevatelq
      heater_on = false;
      loop_counter = 0;
    }
  }
  
  if(after_m)
  {
    double delta_t = boiling_point(pressure) - final_t; // smqtane na razlikata v temperaturite
    if(wifi_connection == true)
    {
      ThingSpeak.setField(1, final_t);
      ThingSpeak.setField(2, pressure);
      ThingSpeak.setField(3, calc_ethanol(delta_t));

      ThingSpeak.writeFields(CHANNEL_ID, API_WRITE_KEY); // izprashtane na danni v oblaka 
    }
    Serial.print(calc_ethanol(delta_t)); //izprashtane na alkoholnoto sydyrvanie po serient port 1
    printt0(calc_ethanol(delta_t)); // izpisvane na alkoholnoto sydyrvanie na displeya
    printt3(final_t);
    printt1(sensors.getTempCByIndex(0));
    printt2(pressure);
    after_m = false; // spirane na funkcii sled izmervaniq
  }
  delay(delayTime);
}

void printValuesDS() // printirane na stoinostite ot dallastemperature datchika
{
  sensors.requestTemperatures(); 
  float temperatureC = sensors.getTempCByIndex(0);
  float temperatureF = sensors.getTempFByIndex(0);

  Serial.print("The dallas temperature sensor has detected - ");
  Serial.println(temperatureC);
}

void printt3(float temp) // printirane na temperaturata na vinoto na displeya
{
    Serial2.print("t3.txt=\"");
    Serial2.print(temp);
    Serial2.print("\"");
    Serial2.write(0xFF);
    Serial2.write(0xFF);
    Serial2.write(0xFF);
}

void printt2(float pressure) // printirane na nalqganeto na displeya
{
    Serial2.print("t2.txt=\"");
    Serial2.print(pressure);
    Serial2.print("\"");
    Serial2.write(0xFF);
    Serial2.write(0xFF);
    Serial2.write(0xFF);
}

void printt1(float temp) // printirane na temperaturata na ohladitelq na displeya
{
    Serial2.print("t1.txt=\"");
    Serial2.print(temp);
    //Serial2.print("13.3");
    Serial2.print("\"");
    Serial2.write(0xFF);
    Serial2.write(0xFF);
    Serial2.write(0xFF);
}

void printt0(float alcohol) // printirane na alkoholnoto sydyrvanie na display
{
    Serial2.print("t0.txt=\"");
    Serial2.print(alcohol);
    Serial2.print("\"");
    Serial2.write(0xFF);
    Serial2.write(0xFF);
    Serial2.write(0xFF);
}

void cooler_regulation()
{
  sensors.requestTemperatures(); // vzimane na temperatura na vsichki dallastemperature datchici svyrzani
  cool_t = sensors.getTempCByIndex(0); // vzimane na temperatura na ohladitelnata technost
  printt1(cool_t);
  if(cool_t > 12.0) { On_Peltie(); }
  else { Off_Peltie(); }
}

void On_Peltie()
{
  digitalWrite(Cooler_pin, HIGH); // puskane na ohladitel
}

void Off_Peltie()
{
  digitalWrite(Cooler_pin, LOW);  // spirane na ohladitel
}

void On_Pump()
{
  digitalWrite(Pump_pin, HIGH); // puskane na pompa
}

void Off_Pump()
{
  digitalWrite(Pump_pin, LOW);  //spirane na pompa
}

void On_Heater()
{
  digitalWrite(Heater_pin, HIGH); // puskane na nagrevatel
}

void Off_Heater()
{
  digitalWrite(Heater_pin, LOW);  // spirane na nagrevatel
}

void On_Cooler_Pump()
{
  digitalWrite(Cooler_pump_pin, HIGH);
}

void Off_Cooler_Pump()
{
  digitalWrite(Cooler_pump_pin, LOW);
}
