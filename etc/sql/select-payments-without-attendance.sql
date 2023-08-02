-- Получить платжи за группы, по котором у учеников не было отметок посещений.
SELECT
	persons.Name_Surname AS 'Фамилия',
	persons.Name_Name AS 'Имя',
	persons.Name_Patronymic AS 'Отчество',
	groups.Name AS 'Группа',
	studentaccountingevents.WhenOccured AS 'Дата',
	studentaccountingevents.Amount_Quantity AS 'Сумма платежа',
	coachingserviceagreements.Description AS 'Тариф',
	coachingserviceagreements.Rate_Quantity AS 'Стоимость по тарифу'
FROM studentaccountingevents
JOIN studentaccounts ON studentaccounts.Id = studentaccountingevents.AccountId
JOIN students ON students.Id = studentaccounts.StudentId
JOIN persons ON persons.Id = students.PersonId
JOIN groups ON groups.Id = studentaccountingevents.GroupId
JOIN coachingserviceagreements ON coachingserviceagreements.Id = studentaccountingevents.ServiceAgreementId
WHERE studentaccountingevents.Discriminator = 'PaymentAccountingEvent'
	AND NOT exists	
	(
		SELECT *
		FROM attendancelogs
		JOIN attendancelogentry ON attendancelogentry.AttendanceLogId = attendancelogs.Id
		WHERE attendancelogs.GroupId = studentaccountingevents.GroupId AND attendancelogentry.StudentId = studentaccounts.StudentId
	)
ORDER BY persons.Name_Surname, persons.Name_Surname, persons.Name_Patronymic