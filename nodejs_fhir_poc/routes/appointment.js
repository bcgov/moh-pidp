const express = require("express");
const router = express.Router();
const fhirClientHelper = require("../helpers/fhirClientHelper");

router.post('/',async (req, res, next) => {
  try {
    const newAppointment ={
        "resourceType": "Appointment"
      };
    // Create Payload
    const payload = {
      resourceType: 'Appointment',
      body: newAppointment
    }
    let response = await fhirClientHelper.postData(payload);
    return res.json(response);
  } catch(err) { 
      console.log("error : ", err);
      return res.json(err);
  }
});

router.get('/', async(req, res, next) => {
    try {
      const response = await fhirClientHelper.get('Appointment');
      return res.json(response);
   } catch(err) {
     console.log("error : ", err);
     return res.json(err);
   }
});

router.patch('/:id', async(req, res, next) => {
    try {
      const JSONPatch = [{ op: 'replace', path: '/resourceType', value: 'Appointment' }];
      const payload = {
        resourceType: 'Appointment',
        id: req.params.id,
        JSONPatch
      };
      let response = await fhirClientHelper.patchData(payload);
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