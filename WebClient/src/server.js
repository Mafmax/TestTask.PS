import express from 'express';
import cors from 'cors';
import dns from 'dns';
import proxy from 'express-http-proxy';

const app = express();
const hostname = '0.0.0.0';
const port = 3000;
app.use(cors());
dns.lookup(process.env.TESTTASKPS_API_HOST, (err, address, family) => {
    if (address) {
        process.env.TESTTASKPS_API_RESOLVED_HOST = address;
    }
});
app.use(express.static("public"));

app.use((req, res) => {
    let baseUrl = getBaseUrl();
    proxy.web(req, res, {target: baseUrl});
});

app.listen(port, hostname, () => {
    console.log(`Server running at http://${hostname}:${port}/`);
});

function getBaseUrl() {
    let apiHost = process.env.TESTTASKPS_API_RESOLVED_HOST || "localhost";
    let apiPort = process.env.TESTTASKPS_API_PORT || 5116;
    return `http://${apiHost}:${apiPort}`
}
