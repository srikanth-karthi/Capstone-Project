const http = require('http');
const httpProxy = require('http-proxy');

const proxy = httpProxy.createProxyServer({});

const server = http.createServer((req, res) => {
  proxy.web(req, res, { target: 'http://localhost:5500' });
});

server.listen(8080, () => {
  console.log('Proxy server is running and forwarding traffic to port 8080');
});
