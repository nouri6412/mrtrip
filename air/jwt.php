<?php
class jwt 
{
    function __construct(){

    }
    function base64UrlEncode($data) {
        return rtrim(strtr(base64_encode($data), '+/', '-_'), '=');
    }
    
    function base64UrlDecode($data) {
        return base64_decode(strtr($data, '-_', '+/'));
    }
    function generateJWT($payload, $secretKey) {
        // هدر توکن
        $header = json_encode([
            'alg' => 'HS256', // الگوریتم امضا
            'typ' => 'JWT'    // نوع توکن
        ]);
    
        // پیلود توکن
        $payload = json_encode($payload);
    
        // کدگذاری هدر و پیلود
        $encodedHeader = $this->base64UrlEncode($header);
        $encodedPayload = $this->base64UrlEncode($payload);
    
        // ایجاد امضا
        $signature = hash_hmac('sha256', "$encodedHeader.$encodedPayload", $secretKey, true);
        $encodedSignature = $this->base64UrlEncode($signature);
    
        // ایجاد توکن نهایی
        return "$encodedHeader.$encodedPayload.$encodedSignature";
    }
    function validateJWT($token, $secretKey) {
        // تقسیم توکن به سه بخش
        $parts = explode('.', $token);
        if (count($parts) !== 3) {
            return false; // توکن نامعتبر
        }
    
        list($encodedHeader, $encodedPayload, $encodedSignature) = $parts;
    
        // ایجاد امضا برای بررسی
        $signature = hash_hmac('sha256', "$encodedHeader.$encodedPayload", $secretKey, true);
        $calculatedSignature = $this->base64UrlEncode($signature);
    
        // مقایسه امضاها
        if ($calculatedSignature !== $encodedSignature) {
            return false; // توکن نامعتبر
        }
    
        // دیکد کردن پیلود
        $payload = json_decode($this->base64UrlDecode($encodedPayload), true);

        // بررسی زمان انقضا (اگر وجود دارد)
        if (isset($payload['exp']) && $payload['exp'] < time()) {
            return false; // توکن منقضی شده
        }
    
        return $payload; // توکن معتبر
    }
}
