var express = require('express');
var router = express.Router();

const Client = require('fhir-kit-client');
const fhirClient = new Client({
  baseUrl: 'http://localhost:8080/fhir'
});

router.get('/', function(req, res, next) {
  return res.json({
    message: "server running"
  })
});

module.exports = router;
