using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing;

namespace QuizMe_
{
    public partial class Quizzes : Form
    {
        private readonly string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=QuizMeDB;Trusted_Connection=True;";
        public Quizzes()
        {
            InitializeComponent();
        }
        private void Quizzes_Load(object sender, EventArgs e)
        {
            LoadAvailableQuizzes();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();

            this.Hide();
            settings.Show();
        }

        private void progressButton_Click(object sender, EventArgs e)
        {
            Progress progress = new Progress();
            this.Hide();


            progress.Show();
        }

        private void scheduleButton_Click(object sender, EventArgs e)
        {
            Schedule schedule = new Schedule();
            this.Hide();
            schedule.Show();
        }

        private void glossaryButton_Click(object sender, EventArgs e)
        {

        }

        private void quizzesButton_Click(object sender, EventArgs e)
        {

        }

        private void flashcardsButton_Click(object sender, EventArgs e)
        {
            Flashcards flashcards = new Flashcards();
            this.Hide();

            flashcards.Show();
        }

        private void dashboardButton_Click(object sender, EventArgs e)
        {
            Dashboard2 dashboard2 = new Dashboard2();
            this.Hide();

            dashboard2.Show();
        }

        private void createQuizzButton_Click(object sender, EventArgs e)
        {
            CreateQuizForm createForm = new CreateQuizForm();
            createForm.ShowDialog();

            if (createForm.QuizCreatedSuccessfully)
            {

                LoadAvailableQuizzes();


            }
        }
        private void LoadAvailableQuizzes()
        {

            flpAvailableQuizzes.Controls.Clear(); // Clear the list first

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT QuizID, Title, Description FROM Quizzes WHERE UserID = @UserID ORDER BY CreatedDate DESC";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@UserID", QuizMe_.SignIn.staticUserID); // Use your static user ID
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            // 1. Create the main panel for this quiz
                            Panel quizPanel = new Panel();
                            quizPanel.Size = new Size(flpAvailableQuizzes.Width - 25, 100);
                            quizPanel.BorderStyle = BorderStyle.FixedSingle;
                            quizPanel.Margin = new Padding(5);

                            // 2. Create Title Label
                            Label titleLabel = new Label();
                            titleLabel.Text = reader["Title"].ToString();
                            titleLabel.Font = new Font(this.Font.FontFamily, 12, FontStyle.Bold);
                            titleLabel.Location = new Point(10, 10);
                            titleLabel.AutoSize = true;

                            // 3. Create Description Label
                            Label descLabel = new Label();
                            descLabel.Text = reader["Description"].ToString();
                            descLabel.Location = new Point(10, 35);
                            descLabel.Size = new Size(quizPanel.Width - 150, 40); // Leave room for buttons
                            descLabel.AutoSize = false;

                            // 4. Create "Add Questions" Button
                            Button btnAddQuestions = new Button();
                            btnAddQuestions.Text = "Add Questions";
                            btnAddQuestions.Location = new Point(quizPanel.Width - 110, 8);
                            btnAddQuestions.Tag = reader["QuizID"]; // Store the ID in the button's Tag
                            btnAddQuestions.Click += new EventHandler(btnAddQuestions_Click);

                            // 5. Create "Start Quiz" Button
                            Button btnStartQuiz = new Button();
                            btnStartQuiz.Text = "Start";
                            btnStartQuiz.Location = new Point(quizPanel.Width - 110, 38);
                            btnStartQuiz.Tag = reader["QuizID"];
                            btnStartQuiz.Click += new EventHandler(btnStartQuiz_Click);

                            // 6. Create "Delete" Button
                            Button btnDeleteQuiz = new Button();
                            btnDeleteQuiz.Text = "Delete";
                            btnDeleteQuiz.BackColor = Color.LightCoral;
                            btnDeleteQuiz.Location = new Point(quizPanel.Width - 110, 68);
                            btnDeleteQuiz.Tag = reader["QuizID"];
                            btnDeleteQuiz.Click += new EventHandler(btnDeleteQuiz_Click);

                            // 7. Add all controls to the panel
                            quizPanel.Controls.Add(titleLabel);
                            quizPanel.Controls.Add(descLabel);
                            quizPanel.Controls.Add(btnAddQuestions);
                            quizPanel.Controls.Add(btnStartQuiz);
                            quizPanel.Controls.Add(btnDeleteQuiz);

                            // 8. Add the panel to the FlowLayoutPanel
                            flpAvailableQuizzes.Controls.Add(quizPanel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading quizzes: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void btnAddQuestions_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            int quizID = (int)clickedButton.Tag;

            AddQuestionsForm addForm = new AddQuestionsForm(quizID);
            addForm.ShowDialog();

            LoadAvailableQuizzes(); // Refresh list
        }

        // This event is for the "Start" button
        private void btnStartQuiz_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            int quizID = (int)clickedButton.Tag;

            // --- This is the new code ---
            QuizTakerForm quizForm = new QuizTakerForm(quizID);
            quizForm.ShowDialog();
            // --- End of new code ---
        }

        // This event is for the "Delete" button
        private void btnDeleteQuiz_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            int quizID = (int)clickedButton.Tag;

            DialogResult confirm = MessageBox.Show("Are you sure you want to delete this quiz?\nAll of its questions will be permanently removed.",
                "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        // Because we set "ON DELETE CASCADE" in the database,
                        // deleting the quiz will automatically delete all its questions.
                        string query = "DELETE FROM Quizzes WHERE QuizID = @QuizID AND UserID = @UserID";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@QuizID", quizID);
                            cmd.Parameters.AddWithValue("@UserID", QuizMe_.SignIn.staticUserID);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Refresh the list
                    LoadAvailableQuizzes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting quiz: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnStudy_Click(object sender, EventArgs e)
        {
            StudySets studySets = new StudySets();
            this.Hide();
            studySets.Show();
        }
    }
}
