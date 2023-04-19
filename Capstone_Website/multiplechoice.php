<?php

?>

<!DOCTYPE html>
<html lang="en">

<head>
    <title>Class Reality</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.9.1/font/bootstrap-icons.css">
    <link href="../css/homepage.css" rel="stylesheet">


</head>

<body id="bg" style="background-color:lightskyblue">

    <nav class="navbar navbar-expand-lg navbar-light bg-light">
        <div class="container-fluid">
            <a class="navbar-brand" href="home.php">Class Reality</a>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNavDropdown" aria-controls="navbarNavDropdown" aria-expanded="false" aria-label="Toggle Navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarNavDropdown">
                <ul class="navbar-nav me-auto order-0">
                <li class="nav-item">
                        <a class="nav-link active" href="home.php" aria-current="page">Home</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link active" href="flashcards.php" aria-current="page">Flashcards</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link active" href="multiplechoice.php" aria-current="page">Multiple Choice</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link active" href="assets.php" aria-current="page">Assets</a>
                </ul>
            </div>
        </div>
    </nav>
    <h1 style="text-align: center">Add MCQ</h1>

</body>

<form action="process-mcq.php" method="POST">
	<div class="centerBlock form-control">
		<main class="form-signin w-100 m-auto">

				<h1 class="h3 mb-3 fw-normal">Add MCQ</h1>

				<div class="form-floating">
					<input type="text" class="form-control" id="question", name="question">
					<label for="question">Question</label>
				</div>
                <br>

				<div class="form-floating">
					<input type="text" class="form-control" id="answerA", name="answerA">
					<label for="answerA">A.)</label>
				    <select class="form-control" id="checkA", name="checkA">
					    <option>Incorrect</option>
					    <option>Correct</option>
				    </select>
				</div>
                <br>

                <div class="form-floating">
					<input type="text" class="form-control" id="answerB", name="answerB">
					<label for="answerB">B.)</label>
                    <select class="form-control" id="checkB", name="checkB">
					    <option>Incorrect</option>
					    <option>Correct</option>
				    </select>
				</div>
                <br>

                <div class="form-floating">
					<input type="text" class="form-control" id="answerC", name="answerC">
					<label for="answerC">C.)</label>
                    <select class="form-control" id="checkC", name="checkC">
					    <option>Incorrect</option>
					    <option>Correct</option>
				    </select>
				</div>
                <br>

                <div class="form-floating">
					<input type="text" class="form-control" id="answerD", name="answerD">
					<label for="answerD">D.)</label>
                    <select class="form-control" id="checkD", name="checkD">
					    <option>Incorrect</option>
					    <option>Correct</option>
				    </select>
				</div>

				<button class="w-100 btn btn-lg btn-primary">Add Card</button>

		</main>
	</div>
	</form>


<script type="text/javascript" src="Scripts/jquery-2.1.1.min.js"></script>
<script type="text/javascript" src="Scripts/bootstrap.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>
</body>

</html>