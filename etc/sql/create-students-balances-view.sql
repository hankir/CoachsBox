CREATE VIEW studentsbalances as
	SELECT studentaccounts.StudentId, SUM(accountentry.Amount_Quantity) AS Balance
	FROM accountentry
	JOIN studentaccounts ON studentaccounts.Id = accountentry.StudentAccountId
	WHERE accountentry.StudentAccountId IS NOT NULL GROUP BY accountentry.StudentAccountId