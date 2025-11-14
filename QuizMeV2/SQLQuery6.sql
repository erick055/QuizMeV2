CREATE TABLE QuizQuestions (
    QuestionID INT PRIMARY KEY IDENTITY(1,1),
    QuizID INT NOT NULL,
    QuestionText NVARCHAR(MAX) NOT NULL,
    OptionA NVARCHAR(255) NOT NULL,
    OptionB NVARCHAR(255) NOT NULL,
    OptionC NVARCHAR(255) NOT NULL,
    OptionD NVARCHAR(255) NOT NULL,
    CorrectAnswer CHAR(1) NOT NULL, -- Will store 'A', 'B', 'C', or 'D'

    CONSTRAINT FK_QuizQuestions_Quizzes FOREIGN KEY (QuizID) REFERENCES Quizzes(QuizID)
    -- This ensures that if a quiz is deleted, its questions are also deleted
    ON DELETE CASCADE
);