<?php
if(!isset($sign))
{
return;
}
try {

    $url = $baseUrl . '/' . $apiMethod . '?AirLine=' . $airline;
    foreach ($_GET as $key => $param) {
        if ($param != 'airlineID' && $param != 'methodID') {
            $url .= '&' . $key . '=' . $param;
        }
    }
    $url .= '&OfficeUser' .  '=' . $OfficeUser;
    $url .= '&OfficePass' .  '=' . $OfficePass;

    $response = file_get_contents($url);

    if ($response === false) {
        throw new Exception('خطا در پارامترهای ورودی');
    }

    // پردازش پاسخ
    $data = json_decode($response, true);

    if (isset($data)) {
        echo json_encode($data);
        http_response_code(200);
    } else {
        throw new Exception('خطا در پاسخ سامانه هواپیمایی');
    }
} catch (Exception $e) {
    echo json_encode([
        'error' => $e->getMessage(),
    ]);
    http_response_code(500);
}
