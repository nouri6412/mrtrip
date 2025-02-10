<?php
if(!isset($sign))
{
return;
}
use PDO as PDO;
use PDOException as PDOException;

class Database
{
    var $connection;
    function __construct()
    {
        try {
            // $serverName = "89.39.208.21,9994";
            $serverName = ".\MSSQLSERVER2016";
            $database = "Airplane"; // نام دیتابیس
            $username = "mrtripir_newuser3";      // نام کاربری
            $password = "F#08r5yb";
            $dsn = "sqlsrv:Server=$serverName;Database=$database;TrustServerCertificate=true";
            $this->connection  = new PDO($dsn, $username, $password);
            $this->connection->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
            //  $this->connection->exec("SET NAMES 'utf8'");
            //  $this->connection->exec("SET CHARACTER SET utf8");   
        } catch (PDOException $ex) {
            // echo "error database";
        }
    }
    function pdo_json($query, $param = array())
    {
        try {
            if (isset($this->connection)) {
                $prepared = $this->connection->prepare($query);
                $prepared->execute($param);
                $data = $prepared->fetchAll();
                $res = array();
                $index_row = 0;
                foreach ($data as $row) {
                    $row_data = array();
                    foreach ($row as $key => $col) {
                        if (!is_numeric($key)) {
                            $row_data[$key] = $col;
                        }
                    }
                    $res[$index_row] = $row_data;
                    $index_row = $index_row + 1;
                }
                return $res;
            } else {
                return array();
            }
        } catch (PDOException $e) {

            //error text info
            return $e->getMessage();
            //error array
            //   print_r($this->connection->errorInfo());
        }
    }
    function pdo($query, $param = array())
    {
        try {
            if (isset($this->connection)) {
                $prepared = $this->connection->prepare($query);
                $prepared->execute($param);
                $data = $prepared->fetchAll();
                return $data;
            } else {
                return array();
            }
        } catch (PDOException $e) {
            //error text info
            return $e->getMessage();
            //error array
            //   print_r($this->connection->errorInfo());
        }
    }
    function pdo_single($query, $param = array())
    {
        try {
            if (isset($this->connection)) {
                $prepared = $this->connection->prepare($query);
                $prepared->execute($param);
                $data = $prepared->fetchAll();
                if (isset($data[0])) {
                    return $data[0];
                } else {
                    return array();
                }
            } else {
                return array();
            }
        } catch (PDOException $e) {
            //error text info
            return $e->getMessage();
            //error array
            //   print_r($this->connection->errorInfo());
        }
    }
    function pdo_exc($query, $param = array(), $option = array())
    {
        try {
            if (isset($this->connection)) {
                $prepared = $this->connection->prepare($query);
                $data = $prepared->execute($param);
                if ($this->connection->lastInsertId() > 0) {
                    $data = $this->connection->lastInsertId();
                } else {
                    $data = $prepared->rowCount();
                }
                return $data;
            } else {
                return "0";
            }
        } catch (PDOException $e) {

            //error text info
            return $e->getMessage();
            //error array
            //   print_r($this->connection->errorInfo());
        }
    }
}
