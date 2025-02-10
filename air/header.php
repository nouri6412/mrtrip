<?php
if(!isset($sign))
{
return;
}
define('secretKey', 'mrtriP_farshiD_Feizdar_AirLinE_136');
define('username', 'admin_farshid');
define('password', '@!admin_farshid!@');
include "db_driver.php";
header("Content-Type: application/json");
$db = new Database();
// $db->pdo_exc("insert into mrtripir_newuser3.BaseURL(AirlineID,URL) VALUES(:id,:url)",array("id"=>1,"url"=>"test"));
// $data=$db->pdo_json('select * from mrtripir_newuser3.BaseURL');
// echo json_encode($data);
// http_response_code(200);
// return;

include 'jwt.php';

function login()
{
    try {
        $username = $_POST['username'] ?? null;
        $password = $_POST['password'] ?? null;

        if ($username ==username && $password ==password) {
            $payload = [
                'user_id' => 136136123,
                'username' => $username,
                'email' => 'farshid@mrtrip.ir',
                'iat' => time(), // زمان ایجاد توکن
                'exp' => time() + 86400 // زمان انقضای توکن (1 ساعت بعد)
            ];
            $jwt = new jwt;
            $token = $jwt->generateJWT($payload, secretKey);
            echo json_encode(["status"=>1,"token"=>$token]);
            http_response_code(200);
        }
        else{
            throw new Exception('نام کاربری و یا رمز عبور اشتباه است');
        }
    } catch (Exception $e) {
        echo json_encode([
            "status"=>0,"error"=>$e->getMessage()
        ],JSON_UNESCAPED_UNICODE);
        http_response_code(200);
    }
}

function validate_user()
{
    try {
        $jwt = new jwt;
        $token = $_SERVER['Authorization'] ?? null;
        if(isset($token))
        {
            $payload = $jwt->validateJWT($token, secretKey);
            if ($payload) {
                return true;
            }
        }
    } catch (Exception $e) {
      
    }
    return false;
}

