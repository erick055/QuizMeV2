using System;
using System.Data.SqlClient; // Required for database operations
using System.Windows.Forms;

namespace QuizMe_
{
    public partial class CreateQuizForm : Form
    {
        // Connection string (use your existing one)
        private readonly string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=QuizMeDB;Trusted_Connection=True;";

        // Property to indicate if a quiz was successfully created
        public bool QuizCreatedSuccessfully { get; private set; } = false;
        public int NewQuizID { get; private set; } // To store the ID of the newly created quiz

        public CreateQuizForm()
        {
            InitializeComponent();
        }

        private void btnSaveQuiz_Click(object sender, EventArgs e)
        {
            // Basic validation
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close(); // Simply close the form
        }

        private void btnSaveQuiz_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtQuizTitle.Text))
            {
                MessageBox.Show("Please enter a Quiz Title.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString)) // connectionString is defined at top of class
                {
                    con.Open();
                    // We use UserID from your SignIn class
                    string query = "INSERT INTO Quizzes (UserID, Title, Description, CreatedDate) VALUES (@UserID, @Title, @Description, @CreatedDate); SELECT SCOPE_IDENTITY();";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@UserID", QuizMe_.SignIn.staticUserID);
                        cmd.Parameters.AddWithValue("@Title", txtQuizTitle.Text.Trim());
                        cmd.Parameters.AddWithValue("@Description", txtQuizDescription.Text.Trim());
                        cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                        NewQuizID = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }

                MessageBox.Show("Quiz '" + txtQuizTitle.Text + "' created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                QuizCreatedSuccessfully = true;
                this.Close();
            }
            catch (Exception ex)
            {
                // THIS IS THE MESSAGE YOU SHOULD BE SEEING
                MessageBox.Show("An error occurred while saving the quiz: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                QuizCreatedSuccessfully = false;
            }
        }
    }
}