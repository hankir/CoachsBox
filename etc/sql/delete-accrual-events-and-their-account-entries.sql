/* Удаление проведенных начислений */
DELETE FROM accountentry
WHERE accountentry.Id IN (
	SELECT r.AccountEntryId FROM coachingaccountingevents AS c
	LEFT JOIN accountingeventresultingentry AS r ON c.Id = r.ProcessingStateId
	WHERE c.EventType_Name <> 'Payment'
);

/* Удаление из журнала транзакций все начисления */
DELETE FROM coachingaccountingevents WHERE coachingaccountingevents.EventType_Name <> 'Payment';