#include <stdio.h>
#include <ESP8266WebServer.h>
#include <ArduinoJson.h>

#define HTTP_REST_PORT 80
#define WIFI_RETRY_DELAY 500
#define MAX_WIFI_INIT_RETRY 50

const int ledBuildIn = 2;

const char* wifi_ssid = "TP-LINK_5B5C";
const char* wifi_passwd = "39251800";

ESP8266WebServer http_rest_server(HTTP_REST_PORT);

struct Device {
    String Ip;
    String Name;
    String Description;
} device;

void initDevice() {
device.Name = "ESP";
device.Description = "Testing";
  
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


void config_rest_server_routing() {
    http_rest_server.on("/", HTTP_GET, []() {
        http_rest_server.send(200, "text/html",
            "Welcome to the ESP8266 REST Web Server");
    });
    http_rest_server.on("/info", HTTP_GET, GetInfo);
}

void setup(void) {

      initDevice();
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
