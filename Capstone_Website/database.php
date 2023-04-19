<?php

// database sign-in credentials (default for now)
$host = "localhost";
$dbname = "classreality";
$username = "root";
$password = "";

// signing into the database using credentials
$conn = mysqli_connect("localhost", $username, $password, $dbname);

// Check connection
if (!$conn) {
    die("Connection failed: " . mysqli_connect_error());
}



return $conn;