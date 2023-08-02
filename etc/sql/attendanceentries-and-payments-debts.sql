SELECT attendancelogentry.StudentId, COUNT(*), COUNT(*) * 150,
 (
 	SELECT Sum(accountentry.Amount_Quantity) from accountentry
	JOIN studentaccounts ON accountentry.StudentAccountId = studentaccounts.Id
	WHERE
		accountentry.Discriminator = 'StudentAccountEntry' AND
		accountentry.Date > "2020-09-01" AND
		accountentry.Date <= "2020-10-14" AND 
		accountentry.EntryType_Name = 'Payment' AND
		attendancelogentry.StudentId = studentaccounts.StudentId
	GROUP BY studentaccounts.StudentId
 ) - (COUNT(*) * 150) AS Balance
FROM attendancelogentry
JOIN attendancelogs ON attendancelogs.Id = attendancelogentry.AttendanceLogId AND attendancelogs.GroupId = 56
WHERE
 	attendancelogentry.Date_Month_Number = 9
	AND attendancelogentry.IsTrialTraining = 0
	AND (attendancelogentry.AbsenceReason_Reason IS NULL OR attendancelogentry.AbsenceReason_Reason = 'WithoutValidExcuse')
GROUP BY attendancelogentry.StudentId;