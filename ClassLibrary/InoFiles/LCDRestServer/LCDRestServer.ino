#include <stdio.h>
#include <ESP8266WebServer.h>
#include <ArduinoJson.h>

#include <Wire.h>   // standardowa biblioteka Arduino
#include <LiquidCrystal_I2C.h> // dolaczenie pobranej biblioteki I2C dla LCD

#define HTTP_REST_PORT 80
#define WIFI_RETRY_DELAY 500
#define MAX_WIFI_INIT_RETRY 50
#define LED_BUILTIN 2
const char* wifi_ssid = "TP-LINK_5B5C";
const char* wifi_passwd = "39251800";

LiquidCrystal_I2C lcd(0x27, 2, 1, 0, 4, 5, 6, 7, 3, POSITIVE);  // Ustawienie adresu ukladu na 0x27
ESP8266WebServer http_rest_server(HTTP_REST_PORT);

struct Device {
    String Ip;
    String Name;
    String Description;
} device;

struct LCDInfo{
  String UpLine;
  String DownLine;
  bool BackLight;
} lcdInfo;

void initDevice() {
device.Name = "LCD";
device.Description = "LCD Device";
  pinMode(LED_BUILTIN, OUTPUT);
  digitalWrite(LED_BUILTIN,LOW); 
} 
void SetLcdText(){
  lcd.clear();
    lcd.setCursor(0,0);
  lcd.print(lcdInfo.UpLine);
  lcd.setCursor(0,1);
  lcd.print(lcdInfo.DownLine);
  if(lcdInfo.BackLight == true){
    lcd.backlight();
  }
  else{
    lcd.noBacklight();
  }
}
void initLCD(){
  lcdInfo.UpLine = "Hello";
  lcdInfo.DownLine = "World";
  lcdInfo.BackLight = true;
  lcd.begin(16,2); 
  SetLcdText();
}



int init_wifi() {
    int retries = 0;

    Serial.println("Connecting to WiFi AP..........");

    WiFi.mode(WIFI_STA);
    WiFi.begin(wifi_ssid, wifi_passwd);
    // check the status of WiFi connection to be WL_CONNECTED
    while ((WiFi.status() != WL_CONNECTED) && (retries < MAX_WIFI_INIT_RETRY)) {
        retries++;
        delay(WIFI_RETRY_DELAY);
        Serial.print("#");
    }
    return WiFi.status(); // return the WiFi connection status
}
String ipToString(IPAddress ip){
  String s="";
  for (int i=0; i<4; i++)
    s += i  ? "." + String(ip[i]) : String(ip[i]);
  return s;
}
void GetInfo(){
    StaticJsonBuffer<200> jsonBuffer;
    JsonObject& jsonObj = jsonBuffer.createObject();
    char JSONmessageBuffer[200];
    device.Ip = ipToString(WiFi.localIP());
    jsonObj["Ip"] = device.Ip;
    jsonObj["Name"] = device.Name;
    jsonObj["Description"] = device.Description;
    jsonObj.prettyPrintTo(JSONmessageBuffer, sizeof(JSONmessageBuffer));
    http_rest_server.send(200, "application/json", JSONmessageBuffer);
}

void SetInfo(){
    StaticJsonBuffer<500> jsonBuffer;
    String post_body = http_rest_server.arg("plain");
    Serial.println(post_body);

    JsonObject& jsonBody = jsonBuffer.parseObject(http_rest_server.arg("plain"));

    Serial.print("HTTP Method: ");
    Serial.println(http_rest_server.method());
    
    if (!jsonBody.success()) {
        Serial.println("error in parsin json body");
        http_rest_server.send(400);
    }
    else {   
        if (http_rest_server.method() == HTTP_POST) {
            device.Name = jsonBody["Name"].asString();
            device.Description = jsonBody["Description"].asString();    
            http_rest_server.send(200);
        }
    }
}
void GetLCDInfo(){
     StaticJsonBuffer<200> jsonBuffer;
    JsonObject& jsonObj = jsonBuffer.createObject();
    char JSONmessageBuffer[200];
    jsonObj["UpLine"] = lcdInfo.UpLine;
    jsonObj["DownLine"] = lcdInfo.DownLine;
    jsonObj["BackLight"] = lcdInfo.BackLight;
    jsonObj.prettyPrintTo(JSONmessageBuffer, sizeof(JSONmessageBuffer));
    http_rest_server.send(200, "application/json", JSONmessageBuffer);
}

void SetLCDInfo(){
  StaticJsonBuffer<500> jsonBuffer;
    String post_body = http_rest_server.arg("plain");
    Serial.println(post_body);

    JsonObject& jsonBody = jsonBuffer.parseObject(http_rest_server.arg("plain"));

    Serial.print("HTTP Method: ");
    Serial.println(http_rest_server.method());
    
    if (!jsonBody.success()) {
        Serial.println("error in parsin json body");
        http_rest_server.send(400);
    }
    else {   
        if (http_rest_server.method() == HTTP_POST) {
            lcdInfo.UpLine = jsonBody["UpLine"].asString();
            lcdInfo.DownLine = jsonBody["DownLine"].asString();
            lcdInfo.BackLight = jsonBody["BackLight"];
            SetLcdText();
            http_rest_server.send(200);
        }
    }
}
void config_rest_server_routing() {
    http_rest_server.on("/", HTTP_GET, []() {
        http_rest_server.send(200, "text/html",
            "Welcome to the ESP8266 REST Web Server");
    });
    http_rest_server.on("/info", HTTP_GET, GetInfo);
    http_rest_server.on("/info", HTTP_POST, SetInfo);
    http_rest_server.on("/lcd", HTTP_GET, GetLCDInfo);
    http_rest_server.on("/lcd", HTTP_POST, SetLCDInfo);
}


void setup(void) {

      initDevice();
      initLCD();
        // Inicjalizacja LCD 2x16
    Serial.begin(115200);
    
    if (init_wifi() == WL_CONNECTED) {
        Serial.print("Connetted to ");
        Serial.print(wifi_ssid);
        Serial.print("--- IP: ");
        Serial.println(WiFi.localIP());
    }
    else {
        Serial.print("Error connecting to: ");
        Serial.println(wifi_ssid);
    }

    config_rest_server_routing();

    http_rest_server.begin();
    Serial.println("HTTP REST Server Started");

    
}

void loop(void) {
    http_rest_server.handleClient();
}
