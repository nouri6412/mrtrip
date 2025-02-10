const express = require('express');
const cors = require('cors');
const { createProxyMiddleware } = require('http-proxy-middleware');

const app = express();

// تنظیمات CORS
app.use(cors());

// تنظیمات Proxy
app.use('/api', createProxyMiddleware({
  target: 'https://airline.mrtrip.ir/api.php',
  changeOrigin: true,
  pathRewrite: {
    '^/api': '', // حذف /api از مسیر
  }
}));

// راه‌اندازی سرور
app.listen(5000, () => {
  console.log('Proxy server running on http://localhost:5000');
});