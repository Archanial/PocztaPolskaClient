# Poczta Polska API REST

Projekt implementuje publiczne API Poczty Polskiej za pomocą technologii REST. Każda z dostępnych metod została zaimplementowana jako odpowiadający endpoint.
Głównym celem projektu jest umożliwienie dostępu do API Poczty Polskiej za pomocą protokołu HTTP, co pozwala na łatwiejsze testowanie i integrację z innymi systemami.

# Kontroler
Projekt implementuje pojedyńczy kontroler `PostApiController`, który zawiera wszystkie dostępne endpointy.
Dostępny jest on pod adresem `/poczta`.
Każdy z endpointów jest w stanie zwrócić jedną z kilku odpowiedzi:
- BadRequest
- NoContent - zdalny serwer nie odpowiedział
- Wyjątek - wystąpił błąd podczas komunikacji z serwerem Poczty Polskiej
- Ok - odpowiedź zwrócona przez serwer Poczty Polskiej
- Unauthorized - błędne dane uwierzytelniające

## Klasa odpowiedzi bazowej
Każda odpowiedź zwracana przez kontroler dziedziczy po klasie `Response`, która zawiera pola `Content` i `Links`.
Pole `Content` jest polem generycznym i zawiera różne dane w zależności od odpowiedzi.
Pole `Links` zawiera linki do innych zasobów, które mogą być przydatne w kontekście odpowiedzi i implementuje idee HATEOAS.

## Endpointy
### GET /poczta/welcome
Zwraca powitanie od serwera Poczty Polskiej.
Przykładowa odpowiedź:
```json
{
    "content": "Witam x",
    "links": []
}
```

### GET /poczta/version
Zwraca wersję API Poczty Polskiej.
Przykładowa odpowiedź:
```json
{
    "content": "1.0",
    "links": []
}
```

### GET /poczta/getMaxShipments
Zwraca maksymalną liczbę przesyłek, które można sprawdzić jednym zapytaniem.
Przykładowa odpowiedź:
```json
{
    "content": 1,
    "links": []
}
```

### GET /poczta/checkShipments
Sprawdza status przesyłek na podstawie podanych numerów.
W przypadku braku odpowiedzi zwraca NoContent.
W przypadku braku podanych numerów zwraca BadRequest.
Przykładowa odpowiedź:

```json
{
    "content": [
        {
          "Komunikat": "data"
        }
    ],
    "links": []
}
```
Komunikat odpowiada klasie komunikat udostępniamy przez WSDL Poczty Polskiej.

### POST /poczta/checkLocalShipments
Sprawdza status przesyłek krajowych na podstawie podanych numerów.
W przypadku braku odpowiedzi zwraca NoContent.
W przypadku braku podanych numerów zwraca BadRequest.
Przykładowa odpowiedź:

```json
{
    "content": [
        {
            "Komunikat": "data"
        }
    ],
    "links": []
}
```
Komunikat odpowiada klasie komunikat udostępniamy przez WSDL Poczty Polskiej.

### POST /poczta/checkSingleShipment
Sprawdza status pojedynczej przesyłki na podstawie podanego numeru.
W przypadku braku odpowiedzi zwraca NoContent.
W przypadku braku podanego numeru zwraca BadRequest.
Przykładowa odpowiedź:
```json
{
  "content":{
    "danePrzesylki":{
      "dataNadania":"2024-05-31T00:00:00",
      "dataNadaniaSpecified":true,
      "format":null,
      "kodKrajuNadania":"PL",
      "kodKrajuPrzezn":"PL",
      "kodRodzPrzes":"PX2",
      "krajNadania":"Polska",
      "krajPrzezn":"Polska",
      "masa":0.1,
      "masaSpecified":true,
      "numer":"123",
      "proceduraSerwis":null,
      "rodzPrzes":"Pocztex",
      "urzadNadania":{
        "daneSzczegolowe":null,
        "nazwa":"PP Warszawa W101"
      },
      "urzadPrzezn":{
        "daneSzczegolowe":null,
        "nazwa":"UP Konin 2"
      },
      "zakonczonoObsluge":true,
      "zakonczonoObslugeSpecified":true,
      "zdarzenia":[
        {
          "czas":"2024-05-29 15:16",
          "jednostka":{
            "daneSzczegolowe":null,
            "nazwa":"Elektroniczny Nadawca"
          },
          "kod":"P_REJ_KN1",
          "konczace":false,
          "konczaceSpecified":true,
          "nazwa":"Otrzymano dane elektroniczne przesyłki",
          "przyczyna":null
        },
        {
          "czas":"2024-05-31 10:00",
          "jednostka":{
            "daneSzczegolowe":null,
            "nazwa":"PP Warszawa W101"
          },
          "kod":"P_NAD",
          "konczace":false,
          "konczaceSpecified":true,
          "nazwa":"Nadanie przesyłki",
          "przyczyna":null
        },
        {
          "czas":"2024-05-31 11:00",
          "jednostka":{
            "daneSzczegolowe":null,
            "nazwa":"Centralna Baza Danych ZST"
          },
          "kod":"P_WEMAIL",
          "konczace":false,
          "konczaceSpecified":true,
          "nazwa":"Wysłano powiadomienie e-mail",
          "przyczyna":null
        },
        {
          "czas":"2024-05-31 20:37",
          "jednostka":{
            "daneSzczegolowe":null,
            "nazwa":"WER Warszawa"
          },
          "kod":"P_WZL",
          "konczace":false,
          "konczaceSpecified":true,
          "nazwa":"Wysłanie przesyłki",
          "przyczyna":null
        },
        {
          "czas":"2024-06-01 01:43",
          "jednostka":{
            "daneSzczegolowe":null,
            "nazwa":"WER Komorniki"
          },
          "kod":"P_PZL",
          "konczace":false,
          "konczaceSpecified":true,
          "nazwa":"Nadejście",
          "przyczyna":null
        },
        {
          "czas":"2024-06-01 08:25",
          "jednostka":{
            "daneSzczegolowe":null,
            "nazwa":"WER Komorniki"
          },
          "kod":"P_WZL",
          "konczace":false,
          "konczaceSpecified":true,
          "nazwa":"Wysłanie przesyłki",
          "przyczyna":null
        },
        {
          "czas":"2024-06-01 14:12",
          "jednostka":{
            "daneSzczegolowe":null,
            "nazwa":"UP Konin 2"
          },
          "kod":"P_PZL",
          "konczace":false,
          "konczaceSpecified":true,
          "nazwa":"Nadejście",
          "przyczyna":null
        },
        {
          "czas":"2024-06-03 07:42",
          "jednostka":{
            "daneSzczegolowe":null,
            "nazwa":"UP Konin 2"
          },
          "kod":"P_KOD",
          "konczace":false,
          "konczaceSpecified":true,
          "nazwa":"Przygotowano do doręczenia",
          "przyczyna":null
        },
        {
          "czas":"2024-06-03 08:31",
          "jednostka":{
            "daneSzczegolowe":null,
            "nazwa":"UP Konin 2"
          },
          "kod":"P_WDML",
          "konczace":false,
          "konczaceSpecified":true,
          "nazwa":"Przekazano do doręczenia",
          "przyczyna":null
        },
        {
          "czas":"2024-06-03 08:31",
          "jednostka":{
            "daneSzczegolowe":null,
            "nazwa":"UP Konin 2"
          },
          "kod":"P_WD",
          "konczace":false,
          "konczaceSpecified":true,
          "nazwa":"Przygotowano do doręczenia",
          "przyczyna":null
        },
        {
          "czas":"2024-06-03 08:45",
          "jednostka":{
            "daneSzczegolowe":null,
            "nazwa":"Centralna Baza Danych ZST"
          },
          "kod":"P_WSMS",
          "konczace":false,
          "konczaceSpecified":true,
          "nazwa":"Wysłano powiadomienie sms",
          "przyczyna":null
        },
        {
          "czas":"2024-06-03 08:47",
          "jednostka":{
            "daneSzczegolowe":null,
            "nazwa":"Centralna Baza Danych ZST"
          },
          "kod":"P_WEMAIL",
          "konczace":false,
          "konczaceSpecified":true,
          "nazwa":"Wysłano powiadomienie e-mail",
          "przyczyna":null
        },
        {
          "czas":"2024-06-03 10:44",
          "jednostka":{
            "daneSzczegolowe":null,
            "nazwa":"UP Konin 2"
          },
          "kod":"P_D",
          "konczace":true,
          "konczaceSpecified":true,
          "nazwa":"Doręczono",
          "przyczyna":null
        },
        {
          "czas":"2024-06-03 10:44",
          "jednostka":{
            "daneSzczegolowe":null,
            "nazwa":"Centralny System Sterujący dla Mobilnego Listonosza"
          },
          "kod":"P_UKEPO",
          "konczace":false,
          "konczaceSpecified":true,
          "nazwa":"Udostępnienie podpisu odbiorcy",
          "przyczyna":null
        },
        {
          "czas":"2024-06-03 10:56",
          "jednostka":{
            "daneSzczegolowe":null,
            "nazwa":"Centralna Baza Danych ZST"
          },
          "kod":"P_WEMAIL",
          "konczace":false,
          "konczaceSpecified":true,
          "nazwa":"Wysłano powiadomienie e-mail",
          "przyczyna":null
        },
        {
          "czas":"2024-06-03 13:38",
          "jednostka":{
            "daneSzczegolowe":null,
            "nazwa":"UP Konin 2"
          },
          "kod":"P_RDML",
          "konczace":false,
          "konczaceSpecified":true,
          "nazwa":"Rozliczenie KD przez listonosza",
          "przyczyna":null
        }
      ]
    },
    "numer":"123",
    "status":1,
    "statusSpecified":true
  },
  "links":[

  ]
}
```

### POST /poczta/checkSingleLocalShipment
Sprawdza status pojedynczej przesyłki krajowej na podstawie podanego numeru.
W przypadku braku odpowiedzi zwraca NoContent.
W przypadku braku podanego numeru zwraca BadRequest.
Przykładowa odpowiedź:

```json
{
    "content": {
        "Przesylka": "data"
    },
    "links": []
}
```
Przesylka odpowiada klasie przyłka udostępnianej przez WSDL Poczty Polskiej.

### POST /poczta/checkShipmentsByDate
Sprawdza status przesyłek na podstawie podanej daty.
W przypadku braku odpowiedzi zwraca NoContent.
W przypadku braku podanej daty zwraca BadRequest.
W przypadku braku podanych numerów zwraca BadRequest.
Przykładowa odpowiedź:

```json
{
    "content": [
        {
            "Komunikat": "data"
        }
    ],
    "links": []
}
```

Komunikat odpowiada klasie komunikat udostępnianej przez WSDL Poczty Polskiej.

### POST /poczta/checkLocalShipmentsByDate
Sprawdza status przesyłek krajowych na podstawie podanej daty.
W przypadku braku odpowiedzi zwraca NoContent.
W przypadku braku podanej daty zwraca BadRequest.
W przypadku braku podanych numerów zwraca BadRequest.
Przykładowa odpowiedź:

```json
{
    "content": [
        {
          "Komunikat": "data"
        }
    ],
    "links": []
}
```
Komunikat odpowiada klasie komunikat udostępnianej przez WSDL Poczty Polskiej.

### POST /poczta/getSingleShipmentByBarCode
Pobiera informacje o przesyłce na podstawie kodu kreskowego.
W przypadku braku odpowiedzi zwraca NoContent.
W przypadku braku podanego kodu zwraca BadRequest.

Kod kreskowy musi zostać przekazany w formacie tekstu zakodowanego w base64.

Przykładowa odpowiedź:

```json
{
    "content": {
        "Przesylka": "data"
    },
    "links": []
}
```