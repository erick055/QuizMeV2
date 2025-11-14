CREATE TABLE Quizzes (
    QuizID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    -- Other fields like TotalQuestions, AverageScore, LastAttemptDate can be added later
    CONSTRAINT FK_Quizzes_Users FOREIGN KEY (UserID) REFERENCES Users(userid)
);