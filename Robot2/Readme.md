# Robot2 - GetDocNumbers
Робот получает дату в GET-запросе, в формате

    yyyy-MM-dd

Сам запрос выглядит - при локальном тестировании - как 

    http://localhost:5002/GetDocNumbers?date=2022-07-04

Тогда робот отвечает таким json-чиком:

    {"date":"2022-07-04T00:00:00","value":170}


