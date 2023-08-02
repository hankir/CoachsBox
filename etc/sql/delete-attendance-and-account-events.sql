/* Удаление проведенных транзакций по счету до июня */
DELETE FROM accountentry
WHERE accountentry.Id IN (
	SELECT r.AccountEntryId FROM coachingaccountingevents AS c
	LEFT JOIN accountingeventresultingentry AS r ON c.Id = r.ProcessingStateId
	WHERE c.WhenOccured < '2020-06-01 00:00:00'
);

/* Удаление журнала транзакций до июня */
DELETE FROM coachingaccountingevents WHERE coachingaccountingevents.WhenOccured < '2020-06-01 00:00:00';

/* Удаление отметок посещений до июня */
DELETE FROM attendancelogentry WHERE attendancelogentry.Date_Month_Number < 6 AND attendancelogentry.Date_Year <= 2020;