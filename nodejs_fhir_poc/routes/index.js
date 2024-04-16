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

router.post('/fhir',async (req, res, next) => {
  try {
    const newEvent ={
      "resourceType": "AdverseEvent"
    };
    
    // Using async
    let response = await fhirClient.create({
      resourceType: 'AdverseEvent',
      body: newEvent,
    })
    console.log(response);
    return res.json(response);
  } catch(err) {
    console.log("error : ", err);
    return res.json(err);
  }
});

router.get('/fhir/:id', async(req, res, next) => {
  try {
    // Using async
    let response = await fhirClient.read({ resourceType: 'AdverseEvent', id: req.params.id });
    console.log(response);
    return res.json(response);
  } catch(err) {
    console.log("error : ", err);
    return res.json(err);
  }
});

router.patch('/fhir/:id', async(req, res, next) => {
  try {
    const JSONPatch = [{ op: 'replace', path: '/resourceType', value: 'AdverseEvent' }];
    let response = await fhirClient.patch({
      resourceType: 'AdverseEvent',
      id: req.params.id,
      JSONPatch
    });
    console.log(response);
    return res.json(response);
  } catch(err) {
    console.log("error : ", err);
    return res.json(err);
  }
});

router.delete('/fhir/:id', async(req, res, next) => {
  try {
    let response = await fhirClient.delete({ resourceType: 'AdverseEvent', id: req.params.id });
    console.log(response);
    return res.json(response);
  } catch(err) {
    console.log("error : ", err);
    return res.json(err);
  }
})

module.exports = router;
