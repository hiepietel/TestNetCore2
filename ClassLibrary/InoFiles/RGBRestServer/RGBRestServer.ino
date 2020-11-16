#include <stdio.h>
#include <ESP8266WebServer.h>
#include <ArduinoJson.h>

#define HTTP_REST_PORT 80
#define WIFI_RETRY_DELAY 500
#define MAX_WIFI_INIT_RETRY 50

#include <Adafruit_NeoPixel.h>

const uint8_t LICZBA_DIOD_RGB = 32;
const uint8_t PIN_GPIO = 13;

Adafruit_NeoPixel strip = Adafruit_NeoPixel(LICZBA_DIOD_RGB, PIN_GPIO, NEO_GRB + NEO_KHZ800);

struct Device {
    String Ip;
    String Name;
    String Description;
    String Function;
} device;

struct RGB {
    int Red;
    int Green;
    int Blue;
    int Brightness;
} rgb;

const char* wifi_ssid = "TP-LINK_5B5C";
const char* wifi_passwd = "39251800";

ESP8266WebServer http_rest_server(HTTP_REST_PORT);
void ShowRGB(){
                for(int i=0; i<LICZBA_DIOD_RGB; i++)
                {
                  
                  strip.setPixelColor(i, rgb.Red, rgb.Green, rgb.Blue);
                  strip.setBrightness(rgb.Brightness);
                  
                  strip.show();
                }
}
void InitRGB(){    
          strip.begin();   
  int r = random(255);
  int g = random(255);
  int b = random(255);
                for(int i=0; i<LICZBA_DIOD_RGB; i++)
                {
                  strip.setPixelColor(i, r, g, b);
                  strip.setBrightness(255);
                  delay(10);
                  strip.show();
                }
                delay(200);
                for(int i=LICZBA_DIOD_RGB -1 ; i > -1 ; i--)
                {
                  strip.setPixelColor(i, 0, 0, 0);
                  strip.setBrightness(255);
                  
                  delay(10);
                  strip.show();
                }
}
void initDevice() {
  device.Name = "RGB_TABLE";
  device.Description = "RGB Table Device";
  device.Function = "RGB"; 
  pinMode(LED_BUILTIN, OUTPUT);
  digitalWrite(LED_BUILTIN,HIGH); 
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
    jsonObj["Function"] = device.Function;
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

void GetRGB() {
    StaticJsonBuffer<200> jsonBuffer;
    JsonObject& jsonObj = jsonBuffer.createObject();
    char JSONmessageBuffer[200];

    if (rgb.Red > 255 || rgb.Green > 255 || rgb.Blue > 255 || rgb.Brightness > 255 )
        http_rest_server.send(204);
    else if (rgb.Red < 0  || rgb.Green < 0  || rgb.Blue < 0  || rgb.Brightness < 0  )
        http_rest_server.send(204);    
    else {
        jsonObj["Red"] = rgb.Red;
        jsonObj["Green"] = rgb.Green;
        jsonObj["Blue"] = rgb.Blue;
        jsonObj["Brightness"] = rgb.Brightness;
        jsonObj.prettyPrintTo(JSONmessageBuffer, sizeof(JSONmessageBuffer));
        http_rest_server.send(200, "application/json", JSONmessageBuffer);
    }
}

void SetRGB(){
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
            rgb.Red = jsonBody["Red"];
            rgb.Green = jsonBody["Green"];
            rgb.Blue = jsonBody["Blue"];
            rgb.Brightness = jsonBody["Brightness"];
          ShowRGB();
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
    http_rest_server.on("/rgb", HTTP_GET, GetRGB);
    http_rest_server.on("/rgb", HTTP_POST, SetRGB);
}

void setup(void) {
    Serial.begin(115200);
    initDevice();
    InitRGB();
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


    Serial.println();
    Serial.println("RGB OK");
}

void loop(void) {
    http_rest_server.handleClient();
}
