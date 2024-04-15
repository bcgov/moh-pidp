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

router.get('/fhir', (req, res, next) => {
  try {
    fhirClient.request('Bundle')
    .then(response => {
      console.log(response)
      return res.json(response);
    }).catch(err => {
      console.log("erro : ", err);
      return res.json(err);
    });
 } catch(err) {
   console.log("error : ", err);
 }
});

module.exports = router;
