var express = require('express');
var router = express.Router();
const fhirClientHelper = require("../helpers/fhirClientHelper");

router.post('/',async (req, res, next) => {
  try {
    const newEvent ={
      "resourceType": "AdverseEvent"
    };
    
    // Create Payload
    const payload = {
      resourceType: 'AdverseEvent',
      body: newEvent,
    };

    let response = await fhirClientHelper.postData(payload); 
    return res.json(response);
  } catch(err) {
    console.log("error : ", err);
    return res.json(err);
  }
});

router.get('/', async (req, res, next) => {
    try {
      const response = await fhirClientHelper.get('Bundle');
      return res.json(response);
   } catch(err) {
     console.log("error : ", err);
     return res.json(err);
   }
  });
  
router.get('/:id', async(req, res, next) => {
  try {
    // Get Payload
    const payload = { resourceType: 'AdverseEvent', id: req.params.id }
    let response = await fhirClientHelper.getByID(payload);
    return res.json(response);
  } catch(err) {
    console.log("error : ", err);
    return res.json(err);
  }
});

router.patch('/:id', async(req, res, next) => {
  try {
    const JSONPatch = [{ op: 'replace', path: '/resourceType', value: 'AdverseEvent' }];
    // Patch Payload
    const payload = {
      resourceType: 'AdverseEvent',
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
    const payload = { 
      resourceType: 'AdverseEvent', 
      id: req.params.id 
    };
    let response = await fhirClientHelper.deleteData(payload);
    return res.json(response);
  } catch(err) {
    console.log("error : ", err);
    return res.json(err);
  }
})

module.exports = router;
