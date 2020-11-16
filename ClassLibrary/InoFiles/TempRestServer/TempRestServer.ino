#include <stdio.h>
#include <ESP8266WebServer.h>
#include <ArduinoJson.h>



#define HTTP_REST_PORT 80
#define WIFI_RETRY_DELAY 500
#define MAX_WIFI_INIT_RETRY 50
#define LED_BUILTIN 2
const char* wifi_ssid = "TP-LINK_5B5C";
const char* wifi_passwd = "39251800";
const int B = 4275;               // B value of the thermistor
const int R0 = 100000;    
const int pinTempSensor = A0;
ESP8266WebServer http_rest_server(HTTP_REST_PORT);

struct Device {
    String Ip;
    String Name;
    String Description;
} device;


void initDevice() {
device.Name = "Temp";
device.Description = "Temp Device";
  pinMode(LED_BUILTIN, OUTPUT);
  digitalWrite(LED_BUILTIN,LOW); 
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
void GetTemp(){
     StaticJsonBuffer<200> jsonBuffer;
    JsonObject& jsonObj = jsonBuffer.createObject();
    char JSONmessageBuffer[200];
        int analog = analogRead(pinTempSensor);

    float R = 1023.0/analog-1.0;
    R = R0*R;

    float temperature = 1.0/(log(R/R0)/B+1/298.15)-273.15; // convert to temperature via datasheet
    jsonObj["Temp"] = temperature;

    jsonObj.prettyPrintTo(JSONmessageBuffer, sizeof(JSONmessageBuffer));
    http_rest_server.send(200, "application/json", JSONmessageBuffer);
}

void config_rest_server_routing() {
    http_rest_server.on("/", HTTP_GET, []() {
        http_rest_server.send(200, "text/html",
            "Welcome to the ESP8266 REST Web Server");
    });
    http_rest_server.on("/info", HTTP_GET, GetInfo);
    http_rest_server.on("/info", HTTP_POST, SetInfo);
    http_rest_server.on("/temp", HTTP_GET, GetTemp);

}


void setup(void) {

      initDevice();
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
