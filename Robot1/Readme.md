Это - робот, простой, как рельсы.


По ИНН или ОГРН он находит в публичном сервисе чуть больше информации о компании.


Тестирование: на запрос


http://localhost:5001/PbInfo?query=7706259826


робот должен ответить таким json-чиком:


{"ShortName":"ООО \"НОВОТЕСТ\"","LongName":"ОБЩЕСТВО С ОГРАНИЧЕННОЙ ОТВЕТСТВЕННОСТЬЮ \"НЕЗАВИСИМАЯ ЭКСПЕРТНАЯ КОМПАНИЯ \"НОВОТЕСТ\"","RegDate":"2002-02-19T00:00:00","PostUchetDate":"2010-06-08T00:00:00","Ogrn":"1027739044112","Inn":"7706259826","Kpp":"772501001","LegalAddress":"115280,МОСКВА Г.,,,,УЛ. ЛЕНИНСКАЯ СЛОБОДА,Д. 19,,ЭТАЖ 3","isIp":false,"liquidated":false,"TempCompanyUrl":null,"PdfTempUrl":null}

Проверка робота для загрузки инфы по ИП:

http://localhost:5001/PbInfo?query=773378609630

робот должен ответить таким json-чиком:

{"ShortName":"ЦЫБА АРТЁМ АНДРЕЕВИЧ","LongName":"ЦЫБА АРТЁМ АНДРЕЕВИЧ","RegDate":"2010-10-01T00:00:00","PostUchetDate":"2010-10-01T00:00:00","Ogrn":"310774627400402","Inn":"773378609630","Kpp":null,"LegalAddress":null,"isIp":true,"liquidated":false,"TempCompanyUrl":"https://pb.nalog.ru/company.html?token=D73C2C6CE01B955602D17514D288C87047554B1EDB7024D1A31F546F217D1CFF7EC03A5493E2FDD4DEC274D71B6E5FE43F8FD89E2516D4139585416D698577FA","PdfTempUrl":"https://rmsp.nalog.ru/excerpt.pdf?token=E6AAE35821590E331CE0EEC9051299219E1B2BD943D014250DB81C81B3CBFC340757F99238C0B6B0C8D50244F5D918DB"}

Инсталляция:


После загрузки из github репозитория нужно из основной директории построить проект командой 


dotnet publish -c Release


Потом переложить получившиеся файлы на сервер, предназначенный для запуска контейнера, и построить образ командой


docker build -t pb-image -f Dockerfile .


Потом создаём экземпляр контейнера и запускаем его командой


docker run --name=pb-cont -it --expose=5001 -p 5001:5001 --mount type=bind,source=/DData/Pb/Logs,target=/App/Logs  -d pb-image


При этом директория /DData/Pb/Logs должна быть создана в хостовой системе
