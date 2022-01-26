const express = require("express");
const { join } = require("path");
const app = express();

app.get("/", (req, res) => {
    res.sendFile(join(__dirname, "index.html"));
    });

app.get("/TemplateData/*.gz", (req, res) => {
    res.setHeader('Content-Encoding', 'gzip')
    res.sendFile(join(__dirname, req.url));
    });

app.get("/TemplateData/*", (req, res) => {
    res.sendFile(join(__dirname, req.url));
    });

app.get("/Build/*.gz", (req, res) => {
    res.setHeader('Content-Encoding', 'gzip')
    res.sendFile(join(__dirname, req.url));
    });

app.get("/Build/*", (req, res) => {
    res.sendFile(join(__dirname, req.url));
    });

// Listen on http
app.listen(3000, () => console.log("Application running on port 3000"));