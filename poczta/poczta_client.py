import zeep
import os
import io
import PIL.Image as Image
from array import array
from zeep.transports import Transport
from requests import Session
import zeep.transports
import zeep.wsse as security
from datetime import datetime
import tkinter as tk
from tkinter import filedialog
 
def initialize():
    global username, password
    print("Enter username:")
    username = input()
    print("Enter password:")
    password = input()
    
def mainLoop():
    choice = "0"
    while True:
        clear()
        print("1. Change credentials")
        print("2. Get welcome message")
        print("3. Get version")
        print("4. Get max possible shipments")
        print("5. Get single shipment")
        print("6. Get single local shipment")
        print("7. Get multiple shipments")
        print("8. Get multiple local shipments")
        print("9. Get multiple shipments by dates")
        print("10. Get multiple local shipments by dates")
        print("11. Get shipment by barcode")
        print("12. Exit")    
        
        choice = input()
        clear()
        print(choice)
        if choice == "1":
            initialize()
        elif choice == "2":
            print("Enter name:")
            name = input()
            print("Getting welcome message...")
            getWelcomeMessage(name)
        elif choice == "3":
            print("Getting version...")
            getVersion()
        elif choice == "4": 
            print("Getting max possible shipments...")
            getMaxShipments()
        elif choice == "5": 
            print("Getting single shipment...")
            print("Enter tracking number:")
            trackingNumber = input()
            checkSingleShipment(trackingNumber)
        elif choice == "6": 
            print("Getting single local shipment...")
            print("Enter tracking number:")
            trackingNumber = input()
            checkSingleLocalShipment(trackingNumber)
        elif choice == "7": 
            print("Checking shipments...")
            print("Enter tracking numbers separated by space:")
            trackingNumbers = input()
            checkShipments(trackingNumbers.split())
        elif choice == "8": 
            print("Checking local shipments...")
            print("Enter tracking numbers separated by space:")
            trackingNumbers = input()
            checkLocalShipments(trackingNumbers.split())
        elif choice == "9": 
            print("Checking shipments by dates...")
            print("Enter tracking numbers separated by space:")
            trackingNumbers = input()
            print("Enter starting date in DD-MM-YYYY format:")
            try:
                startDateString = input()
                startDate = datetime.strptime(startDateString, '%d-%m-%Y')
                print("Enter ending date in DD-MM-YYYY format:")
                endingDateString = input()
                endDate = datetime.strptime(endingDateString, '%d-%m-%Y')
            except ValueError:
                print("Invalid date format")
                break
            checkShipmentsByDate(trackingNumbers, startDate, endDate)
        elif choice == "10": 
            print("Checking local shipments by dates...")
            print("Enter tracking numbers separated by space:")
            trackingNumbers = input()
            print("Enter starting date in DD-MM-YYYY format:")
            try:
                startDateString = input()
                startDate = datetime.strptime(startDateString, '%d-%m-%Y')
                print("Enter ending date in DD-MM-YYYY format:")
                endingDateString = input()
                endDate = datetime.strptime(endingDateString, '%d-%m-%Y')
            except ValueError:
                print("Invalid date format")
                break
            checkLocalShipmentsByDate(trackingNumbers, startDate, endDate)
        elif choice == "11": 
            print("Checking shipment by barcode...")
            root = tk.Tk()
            root.withdraw()
            filePath = filedialog.askopenfilename()
            if(filePath == ""):
                print("No file chosen.")
            else:
                try:
                    image = Image.open(filePath)
                except (IOError, OSError) as e:
                    print(f"Error: {e}. File at '{filePath}' is not a valid image file.")
                    break
                getSingleShipmentByBarCode(imageToByteArray(image))
        elif choice == "12": 
            print("Exiting...")
            return
        else:
            print("Invalid choice")
            
        print("Press any key to continue...")
        input()    
        
def clear():
    os.system('cls' if os.name == 'nt' else 'clear')
    
def initClient(method_url):
    # create the header element
    header = zeep.xsd.Element(
        "Header",
        zeep.xsd.ComplexType(
            [
                zeep.xsd.Element(
                    "{http://www.w3.org/2005/08/addressing}Action", zeep.xsd.String()
                ),
                zeep.xsd.Element(
                    "{http://www.w3.org/2005/08/addressing}To", zeep.xsd.String()
                )
            ]
        ),
    )
    # set the header value from header element
    header_value = header(Action=method_url, To=service_url)
    session = Session()
    session.verify = False
    transport = Transport(session=session)
    transport.verify = False
    
    # initialize zeep client
    user_name_token = security.UsernameToken(username, password)
    client = zeep.Client(wsdl=wsdl_url, transport=transport, wsse=user_name_token)
    client.transport.session.verify = False
     
    return client
    
def getWelcomeMessage(name):
    client = initClient(method_urls["GetWelcomeMessage"])
    try:
        response = client.service.GetWelcomeMessage(name)
        print(response)
    except Exception as e:
        print(e)
 
def getVersion():
    client = initClient(method_urls["GetVersion"])
    try:
        response = client.service.GetVersion()
        print(response)
    except Exception as e:
        print(e)
        
def checkSingleShipment(number):
    client = initClient(method_urls["CheckSingleShipment"])
    try:
        response = client.service.CheckSingleShipment(number)
        print(response)
    except Exception as e:
        print(e)

def checkSingleLocalShipment(number):
    client = initClient(method_urls["CheckSingleLocalShipment"])
    try:
        response = client.service.CheckSingleLocalShipment(number)
        print(response)
    except Exception as e:
        print(e)

def checkShipmentsByDate(number, startDate, endDate):
    client = initClient(method_urls["CheckShipmentsByDate"])
    try:
        response = client.service.CheckShipmentsByDate(number, startDate, endDate)
        print(response)
    except Exception as e:
        print(e)

def checkLocalShipmentsByDate(number, startDate, endDate):
    client = initClient(method_urls["CheckLocalShipmentsByDate"])
    try:
        response = client.service.CheckLocalShipmentsByDate(number, startDate, endDate)
        print(response)
    except Exception as e:
        print(e)

def getMaxShipments():
    client = initClient(method_urls["GetMaxShipments"])
    try:
        response = client.service.GetMaxShipments()
        print(response)
    except Exception as e:
        print(e)

def checkLocalShipments(numbers):
    client = initClient(method_urls["CheckLocalShipments"])
    try:
        response = client.service.CheckLocalShipments(numbers)
        print(response)
    except Exception as e:
        print(e)

def checkShipments(numbers):
    client = initClient(method_urls["CheckShipments"])
    try:
        response = client.service.CheckShipments(numbers)
        print(response)
    except Exception as e:
        print(e)

def getSingleShipmentByBarCode(image):
    client = initClient(method_urls["GetSingleShipmentByBarCode"])
    try:
        response = client.service.GetSingleShipmentByBarCode(image)
        print(response)
    except Exception as e:
        print(e)

def imageToByteArray(image):
    byte_io = io.BytesIO()
    imageFormat = image.format if image.format else 'PNG'  # Use 'PNG' format as default if format is unknown
    image.save(byte_io, format=imageFormat)
    byte_data = byte_io.getvalue()
    return byte_data

# set service URL
# service_url = "https://localhost:5229/PostApi.svc"
service_url = "http://localhost:5229/PostApi.svc"
 
# set the WSDL URL
# wsdl_url = "https://localhost:5229/PostApi.svc?WSDL"
wsdl_url = "http://localhost:5229/PostApi.svc?WSDL"

# method URLs
method_urls = {

    "GetWelcomeMessage": "/GetWelcomeMessage",
    "GetVersion": "/GetVersion",
    "CheckSingleShipment": "/CheckSingleShipment",
    "CheckSingleLocalShipment": "/CheckSingleLocalShipment",
    "CheckShipmentsByDate": "/CheckShipmentsByDate",
    "CheckLocalShipmentsByDate": "/CheckLocalShipmentsByDate",
    "GetMaxShipments": "/GetMaxShipments",
    "CheckLocalShipments": "/CheckLocalShipments",
    "CheckShipments": "/CheckShipments",
    "GetSingleShipmentByBarCode": "/GetSingleShipmentByBarCode"
}

initialize()
mainLoop()