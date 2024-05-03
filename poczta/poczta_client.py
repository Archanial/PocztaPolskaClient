import zeep
import os
import io
import PIL.Image as Image
from array import array
from zeep.transports import Transport
from requests import Session
import zeep.transports
import zeep.wsse as security
 
def initialize()
    print("Enter username:\n")
    username = input()
    print("Enter password:\n")
    password = input()
    
def mainLoop():
    choice = 0
    while choice != 4:
        clear()
        print("1. Change credentials")
        print("2. Get welcome message")
        print("3. Get version")
        print("4. Exit")    
        
        choice = input()
        if choice == 1:
            initialize()
        elif choice == 2:
            clear()
            print("Getting welcome message...")
            response = client.service.GetWelcomeMessage()
            print(response)
        elif choice == 3:
            clear()
            print("Getting version...")
            response = client.service.GetVersion()
            print(response)
        elif choice == 4:
            clear()
            print("Exiting...")
        else:
            clear()
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
                ),
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
    
def getWelcomeMessage(string name):
    client = initClient(method_urls["GetWelcomeMessage"])
    try:
        response = client.service.GetWelcomeMessage()
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

# set service URL
service_url = "https://localhost:5229/PostApi.svc"
 
# set the WSDL URL
wsdl_url = "https://localhost:5229/PostApi.svc?WSDL"

# method URLs
method_urls = {

    "GetWelcomeMessage": "/GetWelcomeMessage",
    "GetVersion": "/GetVersion",
}

initialize()
mainLoop()

 
# make the service call
#response = client.service.GetProducts()
 
# print the result
#print(response)

# Define the required namespaces
#client.wsdl.types.prefix_map['wsse'] = 'http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd'
#client.wsdl.types.prefix_map['wsu'] = 'http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd'

# try:
#    response = client.service.GetPhoto()
#    #print(response)
#    image = Image.open(io.BytesIO(response))
#    image.show()
# except Exception as e:
#    print(e)