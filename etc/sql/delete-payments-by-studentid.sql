/* �������� ������� ������ ��� ���������� �������� */
DELETE FROM coachingaccountingevents WHERE id = (
	SELECT accountingeventresultingentry.ProcessingStateId from accountingeventresultingentry
	RIGHT JOIN accountentry ON accountingeventresultingentry.AccountEntryId = accountentry.Id
	JOIN studentaccounts ON accountentry.StudentAccountId = studentaccounts.Id
	WHERE studentaccounts.StudentId = [������������� ��������] AND accountentry.EntryType_Name = 'Payment' AND accountentry.Date = '2020-07-03 09:48:11.686360'
);

/* �������� ������� � ������� ������� �� ����� ��� ���������� �������� */
DELETE FROM accountentry WHERE accountentry.StudentAccountId = (
	SELECT studentaccounts.Id FROM studentaccounts WHERE studentaccounts.StudentId = [������������� ��������]
) AND accountentry.EntryType_Name = 'Payment' AND accountentry.Date = '2020-07-03 09:48:11.686360'