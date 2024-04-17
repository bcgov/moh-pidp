const express = require("express");
const router = express.Router();

const Client = require('fhir-kit-client');
const fhirClient = new Client({
  baseUrl: 'http://localhost:8080/fhir'
});

router.post('/',async (req, res, next) => {
    try {
        const newAppointment ={
            "resourceType": "Appointment"
          };
          
          // Using async
          let response = await fhirClient.create({
            resourceType: 'Appointment',
            body: newAppointment,
          })
          console.log(response);
          return res.json(response);
    } catch(err) { 
        console.log("error : ", err);
        return res.json(err);
    }
});

router.get('/', (req, res, next) => {
    try {
        console.log("get apppointment route is called");
      fhirClient.request('Appointment')
      .then(response => {
        console.log(response)
        return res.json(response);
      }).catch(err => {
        console.log("error : ", err);
        return res.json(err);
      });
   } catch(err) {
     console.log("error : ", err);
   }
});

router.patch('/:id', async(req, res, next) => {
    try {
      const JSONPatch = [{ op: 'replace', path: '/resourceType', value: 'Appointment' }];
      let response = await fhirClient.patch({
        resourceType: 'Appointment',
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
  
router.delete('/:id', async(req, res, next) => {
    try {
        let response = await fhirClient.delete({ resourceType: 'Appointment', id: req.params.id });
        console.log(response);
        return res.json(response);
    } catch(err) {
        console.log("error : ", err);
        return res.json(err);
    }
});

module.exports = router;