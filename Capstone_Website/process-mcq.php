<?php 

// Server-side validation to make sure name is not NULL
if (empty($_POST["question"])) {
    die("Question required");
}
if (empty($_POST["answerA"]) && empty($_POST["answerB"]) && empty($_POST["answerC"]) && empty($_POST["answerD"])) {
    die("At least one answer required");
}

$userID = 1;
$correctAnswer = "";

// require database.php for database connection
$mysqli = require __DIR__ . "/database.php";

// sql insert statement to insert into the database
$sql = "INSERT INTO mcqflashcards(question, mcqA, mcqB, mcqC, mcqD, correctAnswer, userID)
        VALUES(?, ?, ?, ?, ?, ?, ?)";

// init for sql execution
$stmt = $mysqli->stmt_init();

// print error statement and die if problems preparing
if ( ! $stmt->prepare($sql)) {
    die("SQL error: " . $mysqli->error);
}

// creating correct answer variable
if ($_POST["checkA"] == "Correct") {
    $correctAnswer .= "A";
}
if($_POST["checkB"] == "Correct") {
    $correctAnswer .= "B";
}
if($_POST["checkC"] == "Correct") {
    $correctAnswer .= "C";
}
if($_POST["checkD"] == "Correct") {
    $correctAnswer .= "D";
}

// binding params to be added
$stmt->bind_param("ssssssi",
                  $_POST['question'],
                  $_POST['answerA'], 
                  $_POST['answerB'],
                  $_POST['answerC'],
                  $_POST['answerD'],
                  $correctAnswer,
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