ALTER TABLE Flashcards
ADD StudySetID INT NULL;

-- This adds the official database link (foreign key)
ALTER TABLE Flashcards
ADD CONSTRAINT FK_Flashcards_StudySets
    FOREIGN KEY (StudySetID) REFERENCES StudySets(StudySetID);

-- This adds the 'Status' column needed for the progress bar (from Step 5.1)
ALTER TABLE Flashcards
ADD Status INT NOT NULL DEFAULT 0;