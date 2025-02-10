<?php
header('Access-Control-Allow-Origin: *');

// Exit early so the page isn't fully loaded for options requests
if (strtolower($_SERVER['REQUEST_METHOD']) == 'options') {
    exit();
}
$sign='sign';
include 'header.php';
$Method = $_GET['methodID'] ?? null;
if ($Method == "login") {
    login();
} else {
    if (validate_user()) {
        $airlineID = $_GET['airlineID'] ?? null;

        $apiMethod = "";

        if ($Method == "availability") {
            $apiMethod = "AvailabilityJS.jsp";
        }

        $airlineDB = $db->pdo_single('SELECT ba.URL AS url,ai.Airline as airline,ai.OfficeUser AS OfficeUser,ai.OfficePass AS OfficePass from mrtripir_newuser3.BaseURL ba JOIN mrtripir_newuser3.Airline ai ON ba.AirlineID=ai.ID WHERE ba.ID=:id', array("id" => $airlineID));
        $baseUrl = $airlineDB['url'];
        $airline = $airlineDB['airline'];
        $OfficeUser = $airlineDB['OfficeUser'];
        $OfficePass = $airlineDB['OfficePass'];
        include 'api/' . $Method.'.php';
    } else {
        echo json_encode([
            "status" => 0, "error" => 'خطا در احراز هویت'
        ],JSON_UNESCAPED_UNICODE );
        http_response_code(200);
    }
}
