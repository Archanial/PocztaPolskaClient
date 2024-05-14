# Poczta Polska API

Projekt implementuje publiczne API Poczty Polskiej za pomocą SOAP. 

Serwer został stworzony w technologii ASP.NET i wspiera http, jak i https. Udostępnia on swój WSDL na adresie /PostApi.svc.

Każda z dostępnych metod SOAP została zmapowana na odpowiadający jej adres REST, dzięki czemu serwer można testować za pomocą swaggera.

## Ustawienia projektu

Ustawienia projektu przechowywane są w pliku `appsettings.json`. Pola `ApiUsername` i `ApiPassword` odpowiadają za nazwę użytkownika i hasło, które musi zostać użyte podczas dostępu do serwera za pomocą protokołu SOAP. 

Obiekt `ApiCredentials` odpowiada za nazwę użytkownika i hasło użyte w komunikacji z API Poczty Polskiej. W przypadku braku własnych, zaleca się używanie dostarczonych, publicznych danych uwierzytelniających, czyli `sledzeniepp` (nazwa użytkownika) i `PPSA` (hasło).

Aby wystawić serwer na innym adresie niż standardowy (localhost) należy zmodyfikować plik launchSettings.json, a dokładniej pola `applicationUrl` w konfiguracji, która nas interesuje.

## Dostępne operacje

Aplikacja implementuje wszystkie dostępne operacje, zdefiniowane w pliku WSDL Poczty Polskiej. Są to:

### GetWelcomeMessage

Operacja odwołuje się do `witajAsync`. Oczekuje argumentu w postaci stringa z imieniem. Powinna zostać zwrócona odpowiedź w formie string, brzmiąca "Witam x" (gdzie x to argument wejściowy).

### GetVersion

Operacja odwołuje się do `wersjaAsync`. Nie posiada żadnych argumentów. Zwracana jest używana wersja API Poczty Polskiej.

### GetMaxShipments

Operacja odwołuje się do `maksymalnaLiczbaPrzesylekAsync`. Nie przyjmuje argumentów. Zwracana jest maksymalna liczba przesyłek, których dane można otrzymać, za pomocą pojedyńczego zapytania SOAP. W przypadku użycia publicznego hasła liczba ta wynosi 1.

### CheckShipments i CheckLocalShipments

Operacje odwołują się do `sprawdzPrzesylkiAsync` i `sprawdzPrzesylkiPlAsync`. Jako argument przyjmowana jest lista stringów. Każdy z nich powinien być numerem przesyłki. Zwrócona zostaje lista znalezionych przesyłek. Wersja `local` zwraca nieco więcej danych, lecz działa tylko na przesyłki krajowe.

W przypadku użycia publicznego hasła maksymalna liczba sprawdzanych przesyłek wynosi 1.

### CheckSingleShipment i CheckSingleLocalShipment

Operacje odwołują się do `sprawdzPrzesylkeAsync` i `sprawdzPrzesylkePlAsync`. Działają bardzo podobnie jak poprzednio opisane operacje, lecz zamiast listy stringów przyjmują pojedyńczy string zawierający numer przesyłki. Wersja `local` zwraca nieco więcej danych, lecz działa tylko na przesyłki krajowe.

### CheckShipmentsByDate i CheckLocalShipmentsByDate

Operacje odwołują się do `sprawdzPrzesylkiOdDoAsync` i `sprawdzPrzesylkiOdDoPlAsync`. Jako argumenty przyjmowane są lista stringów z numerami przesyłek, data startowa i końcowa, dla której powinny być sprawdzone przesyłki (w formacie DD-MM-YYYY). Wersja `local` zwraca nieco więcej danych, lecz działa tylko na przesyłki krajowe.

### GetSingleShipmentByBarCode

Operacja odwołuje się do `sprawdzPrzesylkeAsync`. Jako argument przyjmuje tablicę bajtów, która jest odpowiednio dekodowana na obraz, z którego potem jest odczytywany kod kreskowy. Jeżeli takowy zostanie poprawnie odczytany, numer wykorzystywany jest jako numer przesyłki.