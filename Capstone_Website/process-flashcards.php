<?php 

// Server-side validation to make sure name is not NULL
if (empty($_POST["question"])) {
    die("Question required");
}
if (empty($_POST["answer"])) {
    die("Answer required");
}

$userID = 1;

// require database.php for database connection
$mysqli = require __DIR__ . "/database.php";

// sql insert statement to insert into the database
$sql = "INSERT INTO flashcards(question, answer, userID)
        VALUES(?, ?, ?)";

// init for sql execution
$stmt = $mysqli->stmt_init();

// print error statement and die if problems preparing
if ( ! $stmt->prepare($sql)) {
    die("SQL error: " . $mysqli->error);
}


// binding params to be added
$stmt->bind_param("ssi",
                  $_POST['question'],
                  $_POST['answer'], 
                  $userID);



// execute statement and catch exception for duplicate emails                   
try {
    if ($stmt->execute()) {
    
        header("Location: home.php");
        exit;

    }
} catch (Exception $e) {

    echo "Failed. \n";
    die($mysqli->error . " " . $mysqli->errno);

}


?>